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
        /// retrieves invoice data by invoice number.
        public static string GetInvoiceData(string invoiceNumber)
        {
            return $"SELECT * FROM Invoices WHERE InvoiceNum = '{invoiceNumber}'";
        }

        /// retrieves all invoices.
        public static string GetAllInvoices()
        {
            return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
        }

        /// inserts a new invoice.
        public static string InsertNewInvoice(string invoiceNumber, DateTime invoiceDate, double totalCost)
        {
            return $"INSERT INTO Invoices (InvoiceNum, InvoiceDate, TotalCost) " +
                   $"VALUES ('{invoiceNumber}', '{invoiceDate:yyyy-MM-dd}', {totalCost})";
        }

        /// updates the total cost of an invoice.
        public static string UpdateInvoice(string invoiceNumber, double newTotal)
        {
            return $"UPDATE Invoices SET TotalCost = {newTotal} WHERE InvoiceNum = '{invoiceNumber}'";
        }

        /// deletes an invoice by invoice number.
        public static string DeleteInvoice(string invoiceNumber)
        {
            return $"DELETE FROM Invoices WHERE InvoiceNum = '{invoiceNumber}'";
        }
    }
}
