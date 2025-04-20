using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Examples from the database help document:
UPDATE Invoices SET TotalCost = 1200 WHERE InvoiceNum = 123
INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values (123, 1, 'AA')
INSERT INTO Invoices (InvoiceDate, TotalCost) Values (#4/13/2018#, 100)
SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = 123
select ItemCode, ItemDesc, Cost from ItemDesc
SELECT LineItems, ItemCode, ItemDesc, ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc WHERE LineItems.ItemCode = ItemDesc.ItemCode AND LineItems.InvoiceNum = 5000
*/

namespace CS3280GroupProject.Main
{
    internal class clsMainSQL
    {
        /// <summary>
        /// gets invoice data by invoice number
        /// </summary>
        public static string GetInvoiceData(string invoiceNumber)
        {
            return $"SELECT * FROM Invoices WHERE InvoiceNum = {invoiceNumber}";
        }



        /// <summary>
        /// get all the invoices...
        /// </summary>
        public static string GetAllInvoices()
        {
            return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
        }

        /// <summary>
        /// inserts a new invoice
        /// </summary>
        public static string InsertNewInvoice(DateTime invoiceDate, double totalCost)
        {
            return $"INSERT INTO Invoices (InvoiceDate, TotalCost) " +
                   $"VALUES ('{invoiceDate:yyyy-MM-dd}', {totalCost})";
        }


        /// <summary>
        /// updates the total cost of an invoice
        /// </summary>
        public static string UpdateInvoice(string invoiceNumber, double newTotal)
        {
            return $"UPDATE Invoices SET TotalCost = {newTotal} WHERE InvoiceNum = {invoiceNumber}";
        }


        /// <summary>
        /// deletes an invoice by invoice number
        /// </summary>
        public static string DeleteInvoice(string invoiceNumber)
        {
            return $"DELETE FROM Invoices WHERE InvoiceNum = '{invoiceNumber}'";
        }

        /// <summary>
        /// get the max invoice number
        /// </summary>
        public static string GetMaxInvoiceNumber()
        {
            return "SELECT MAX(InvoiceNum) FROM Invoices";
        }



    }
}

