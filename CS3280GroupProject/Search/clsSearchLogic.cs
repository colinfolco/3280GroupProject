using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CS3280GroupProject.Common;

namespace CS3280GroupProject.Search
{
    internal class clsSearchLogic
    {

        private clsSearchSQL searchSQL;
        private clsDataAccess dataAccess;

        public clsSearchLogic()
        {
            searchSQL = new clsSearchSQL();
            dataAccess = new clsDataAccess();
        }

        /// <summary>
        /// calls clsInvoice to get invoice number, date, and total cost
        /// and returns invoice
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="invoiceDate"></param>
        /// <param name="totalCost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<(string InvoiceNum, string InvoiceData, string TotalCost)> GetInvoice()
        {
            try
            {
                List<(string InvoiceNum, string InvoiceData, string TotalCost)> invoices = new List<(string, string, string)>();

                //string sqlQuery = new clsSearchSQL().Invoices();
                string sqlQuery = searchSQL.Invoices();
                int rowsAffected = 0;
                DataSet invoiceDS = dataAccess.ExecuteSQLStatement(sqlQuery, ref rowsAffected);

                if (rowsAffected > 0)
                {
                    DataTable dataTable = invoiceDS.Tables[0];

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string invoiceNum = row["InvoiceNum"].ToString();
                        string invoiceDate = row["InvoiceDate"].ToString();
                        string totalCost = row["TotalCost"].ToString();

                        invoices.Add((invoiceNum, invoiceDate, totalCost));
                    }
                }
                return invoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsInvoice> GetInvoiceTypes(string sqlQuery)
        {
            try
            {
                int rows = 0;
                DataSet invoiceDataSet = dataAccess.ExecuteSQLStatement(sqlQuery, ref rows);
                List<clsInvoice> invoices = new List<clsInvoice>();

                if (rows > 0)
                {
                    DataTable invoiceTable = invoiceDataSet.Tables[0];
                    foreach (DataRow row in invoiceTable.Rows)
                    {
                        clsInvoice invoice = new clsInvoice
                        {

                        };
                        invoices.Add(invoice);
                    }
                }
                return invoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }

        public List<clsInvoice> GetAllInvoices()
        {

            try
            {
                List<clsInvoice> invoices = new List<clsInvoice>();

                string sqlQuery = new clsSearchSQL().Invoices();
                int rowsAffected = 0;
                DataSet invoiceDataSet = dataAccess.ExecuteSQLStatement(sqlQuery, ref rowsAffected);

                if (rowsAffected > 0)
                {
                    DataTable invoiceTable = invoiceDataSet.Tables[0];

                    foreach (DataRow row in invoiceTable.Rows)
                    {
                        clsInvoice invoice = new clsInvoice();
                        invoice.InvoiceNumber = row["InvoiceNum"].ToString();
                        invoice.InvoiceDate = row["InvoiceDate"].ToString();
                        invoice.TotalCost = row["TotalCost"].ToString();

                        invoices.Add(invoice);
                    }
                }
                return invoices;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }






        }








    }
}
