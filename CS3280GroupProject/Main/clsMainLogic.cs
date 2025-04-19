using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CS3280GroupProject.Items;

namespace CS3280GroupProject.Main
{
    internal class clsMainLogic
    {
        internal class clsInvoice
        {
            public int InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; }
            public double TotalCost { get; set; }
            public List<Item> Items { get; set; }

            public clsInvoice()
            {
                Items = new List<Item>();
            }

            public clsInvoice(int invoiceNumber, DateTime invoiceDate, double totalCost, List<Item> items)
            {
                InvoiceNumber = invoiceNumber;
                InvoiceDate = invoiceDate;
                TotalCost = totalCost;
                Items = items;
            }
        }
        

        public List<Item> GetAllItems()
        {
            try
            {
                var sql = new Items.clsItemsSQL();
                string sqlQuery = sql.GetAllItems();
                List<Item> items = ExecuteSQLAndMapToItemList(sqlQuery);
                return items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
                return null;
            }
        }


        /// saves or updates an invoice
        public void SaveNewInvoice(clsInvoice invoice)
        {
            try
            {
                if (invoice.InvoiceNumber == 0)
                {
                    // insert a new invoice
                    var sql = new clsMainSQL();
                    string sqlQuery = clsMainSQL.InsertNewInvoice(invoice.InvoiceDate, invoice.TotalCost);
                    ExecuteSQLNonQuery(sqlQuery);

                    // update invoice number after insert
                    string maxInvoiceNumStr = GetMaxInvoiceNumber();
                    if (int.TryParse(maxInvoiceNumStr, out int maxInvoiceNum))
                    {
                        invoice.InvoiceNumber = maxInvoiceNum;
                    }
                    else
                    {
                        throw new Exception("unable to retrieve new invoice number");
                    }
                }
                else
                {
                    // update existing invoice
                    string sqlQuery = clsMainSQL.UpdateInvoice(invoice.InvoiceNumber.ToString(), invoice.TotalCost);
                    ExecuteSQLNonQuery(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error saving invoice: " + ex.Message);
                throw;
            }
        }




        /// retrieves an invoice by invoice number.
        public clsInvoice GetInvoice(string invoiceNumber)
        {
            try
            {
                var sql = new clsMainSQL();
                string sqlQuery = clsMainSQL.GetInvoiceData(invoiceNumber);
                clsInvoice invoice = ExecuteSQLAndMapToInvoice(sqlQuery);
                return invoice;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
                return null;
            }
        }

        private clsInvoice ExecuteSQLAndMapToInvoice(string sqlQuery)
        {
            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;
                var ds = db.ExecuteSQLStatement(sqlQuery, ref iRetVal);

                if (iRetVal > 0)
                {
                    var row = ds.Tables[0].Rows[0];

                    var invoice = new clsInvoice
                    {
                        InvoiceNumber = int.Parse(row["InvoiceNum"].ToString()),
                        InvoiceDate = DateTime.Parse(row["InvoiceDate"].ToString()),
                        TotalCost = double.Parse(row["TotalCost"].ToString()),
                        Items = new List<Items.Item>()
                    };

                    return invoice;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error loading invoice: " + ex.Message);
                return null;
            }
        }

        
        private void ExecuteSQLNonQuery(string sqlQuery)
        {
            var db = new CS3280GroupProject.Common.clsDataAccess();
            int iRetVal = 0;
            db.ExecuteNonQuery(sqlQuery, ref iRetVal);
        }


        private List<Item> ExecuteSQLAndMapToItemList(string sqlQuery)
        {
            List<Item> items = new List<Item>();

            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;
                var ds = db.ExecuteSQLStatement(sqlQuery, ref iRetVal);

                for (int i = 0; i < iRetVal; i++)
                {
                    var item = new Item
                    {
                        ItemName = ds.Tables[0].Rows[i]["ItemDesc"].ToString(),
                        Price = decimal.Parse(ds.Tables[0].Rows[i]["Cost"].ToString())
                    };
                    items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error loading items: " + ex.Message);
            }

            return items;
        }


        /// edits an existing invoice and its items
        public void EditInvoice(clsInvoice oldInvoice, clsInvoice newInvoice)
        {
            try
            {
                string sqlQuery = clsMainSQL.UpdateInvoice(newInvoice.InvoiceNumber.ToString(), newInvoice.TotalCost);
                ExecuteSQLNonQuery(sqlQuery);

                DeleteLineItems(newInvoice.InvoiceNumber);

                SaveLineItems(newInvoice.InvoiceNumber);

                MessageBox.Show("invoice updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error updating invoice: " + ex.Message);
            }
        }

        /// executes a scalar sql query and returns a single value
        private string ExecuteSQLScalar(string sqlQuery)
        {
            var db = new CS3280GroupProject.Common.clsDataAccess();
            var result = db.ExecuteScalarSQL(sqlQuery);

            return result?.ToString() ?? "TBD";
        }


        /// retrieves the max invoice number from the database
        public string GetMaxInvoiceNumber()
        {
            try
            {
                string sqlQuery = CS3280GroupProject.Main.clsMainSQL.GetMaxInvoiceNumber();
                return ExecuteSQLScalar(sqlQuery);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error retrieving highest invoice number: " + ex.Message);
                return "TBD";
            }
        }

        /// saves all the items for an invoice into the LineItems table
        public void SaveLineItems(int invoiceNumber)
        {
            try
            {
                DeleteLineItems(invoiceNumber);

                int lineItemNumber = 1;

                foreach (var item in App.Current.Windows.OfType<MainWindow>().First().dgInvoiceItems.Items)
                {
                    if (item == null)
                        continue;

                    var itemName = (string)item.GetType().GetProperty("ItemName")?.GetValue(item, null);

                    string itemCode = GetItemCodeFromName(itemName);

                    string sqlQuery = $"INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) " +
                                      $"VALUES ({invoiceNumber}, {lineItemNumber}, '{itemCode}')";

                    ExecuteSQLNonQuery(sqlQuery);

                    lineItemNumber++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error saving line items: " + ex.Message);
            }
        }


        /// looks up the item code based on item name
        private string GetItemCodeFromName(string itemName)
        {
            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;

                string sql = $"SELECT ItemCode FROM ItemDesc WHERE ItemDesc = '{itemName}'";

                var ds = db.ExecuteSQLStatement(sql, ref iRetVal);

                if (iRetVal > 0)
                {
                    return ds.Tables[0].Rows[0]["ItemCode"].ToString();
                }
                else
                {
                    throw new Exception("item code not found for item " + itemName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error looking up item code: " + ex.Message);
                return "UNKNOWN";
            }
        }

        /// deletes all line items for an invoice
        public void DeleteLineItems(int invoiceNumber)
        {
            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;

                string sql = $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNumber}";

                db.ExecuteNonQuery(sql, ref iRetVal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error deleting items: " + ex.Message);
            }
        }

        /// uh. gets items for the combo box
        public List<Item> GetItemsForComboBox()
        {
            try
            {
                var itemsLogic = new CS3280GroupProject.Items.clsItemsLogic();
                return itemsLogic.LoadItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show("error loading items: " + ex.Message);
                return new List<Item>();
            }
        }

        /// get the items on an invoice
        public List<Item> GetItemsOnInvoice(int invoiceNum)
        {
            List<Item> invoiceItems = new List<Item>();

            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;
                string sqlQuery = $@"
            SELECT ItemDesc.ItemDesc, ItemDesc.Cost
            FROM LineItems
            INNER JOIN ItemDesc ON LineItems.ItemCode = ItemDesc.ItemCode
            WHERE LineItems.InvoiceNum = {invoiceNum}";

                var ds = db.ExecuteSQLStatement(sqlQuery, ref iRetVal);

                for (int i = 0; i < iRetVal; i++)
                {
                    var item = new Item
                    {
                        ItemName = ds.Tables[0].Rows[i]["ItemDesc"].ToString(),
                        Price = decimal.Parse(ds.Tables[0].Rows[i]["Cost"].ToString())
                    };
                    invoiceItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error loading items: " + ex.Message);
            }

            return invoiceItems;
        }

        /// updates the total cost for a specific invoice
        public void UpdateInvoiceTotal(int invoiceNumber, double newTotalCost)
        {
            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;
                string sqlQuery = $"UPDATE Invoices SET TotalCost = {newTotalCost} WHERE InvoiceNum = {invoiceNumber}";
                db.ExecuteNonQuery(sqlQuery, ref iRetVal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error updating total: " + ex.Message);
            }
        }

        /// Deletes an invoice and its related line items
        public void DeleteInvoice(int invoiceNumber)
        {
            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;

                // delete items
                string deleteLineItemsSQL = $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNumber}";
                db.ExecuteNonQuery(deleteLineItemsSQL, ref iRetVal);

                // delete invoice
                string deleteInvoiceSQL = $"DELETE FROM Invoices WHERE InvoiceNum = {invoiceNumber}";
                db.ExecuteNonQuery(deleteInvoiceSQL, ref iRetVal);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error deleting invoice: " + ex.Message);
            }
        }




    }
}
