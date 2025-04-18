using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CS3280GroupProject.Common;
using CS3280GroupProject.Main;

namespace CS3280GroupProject.Search
{
    internal class clsSearchSQL
    {
        clsSearchLogic searchLogic;
        clsInvoice invoice;
        clsDataAccess dataAccess;

        /// <summary>
        /// constructor
        /// </summary>
        public clsSearchSQL()
        {
            invoice = new clsInvoice();
            dataAccess = new clsDataAccess();
        }

        /// <summary>
        /// this gets the invoices from the database
        /// </summary>
        /// <returns></returns>
        public string Invoices()
        {
            return "SELECT * FROM Invoices";
        }

        /// <summary>
        /// gets SQL invoice number
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsInvoice> GetInvoiceNum(string invoiceNumber)
        {
            try
            {

                string sqlQuery = "SELECT * FROM Invoices WHERE InvoiceNum = " + invoiceNumber;
                return searchLogic.GetInvoiceTypes(sqlQuery);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }

        /// <summary>
        /// gets SQL Date
        /// </summary>
        /// <param name="invoiceDate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsInvoice> GetInvoiceDate(string invoiceDate)
        {
            try
            {
                string sqlQuery = "SELECT * FROM Invoices WHERE InvoiceDate = #" + invoiceDate + "#";
                return searchLogic.GetInvoiceTypes(sqlQuery);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }

        /// <summary>
        /// gets SQL total cost
        /// </summary>
        /// <param name="totalCost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsInvoice> GetTotalCost(string totalCost)
        {
            try
            {
                string sqlQuery = "SELECT * FROM Invoices WHERE TotalCost = " + totalCost;
                return searchLogic.GetInvoiceTypes(sqlQuery);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }

    }
}
