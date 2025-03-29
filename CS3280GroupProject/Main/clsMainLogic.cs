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

        /// edits an existing invoice.
        public void EditInvoice(clsInvoice oldInvoice, clsInvoice newInvoice)
        {
            try
            {
                // get the SQL query neededto update the invoice
                string sqlQuery = clsMainSQL.UpdateInvoice(newInvoice.InvoiceNumber.ToString(), newInvoice.TotalCost);

                // execute the SQL query
                ExecuteSQLNonQuery(sqlQuery);
                MessageBox.Show("Invoice updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
