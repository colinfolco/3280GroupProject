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
        /// represents an invoice
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

        /// retrieves a list of all items from the database.
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
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        /// saves a new invoice to the database.
        public void SaveNewInvoice(clsInvoice invoice)
        {
            try
            {
                var sql = new clsMainSQL();
                string sqlQuery = clsMainSQL.InsertNewInvoice(invoice.InvoiceNumber.ToString(), invoice.InvoiceDate, invoice.TotalCost);
                ExecuteSQLNonQuery(sqlQuery);
                MessageBox.Show("Invoice saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        /// placeholder to execute SQL and map results to a clsInvoice.
        private clsInvoice ExecuteSQLAndMapToInvoice(string sqlQuery)
        {
            clsInvoice invoice = new clsInvoice();
            return invoice;
        }

        /// placeholder to execute non query SQL commands
        private void ExecuteSQLNonQuery(string sqlQuery)
        {
        }

        /// placeholder to execute SQL and map results to a list.
        private List<Item> ExecuteSQLAndMapToItemList(string sqlQuery)
        {
            List<Item> items = new List<Item>();
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

                MessageBox.Show("Invoice updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating invoice: " + ex.Message);
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
                MessageBox.Show("Error retrieving max invoice number: " + ex.Message);
                return "TBD";
            }
        }

        /// saves all the items for an invoice into the LineItems table
        public void SaveLineItems(int invoiceNumber)
        {
            try
            {
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
                MessageBox.Show("Error saving line items: " + ex.Message);
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
                    throw new Exception("Item code not found for item name: " + itemName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error looking up item code: " + ex.Message);
                return "UNKNOWN";
            }
        }

        /// deletes all line items for an invoice
        private void DeleteLineItems(int invoiceNumber)
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
                MessageBox.Show("Error deleting line items: " + ex.Message);
            }
        }









    }
}
