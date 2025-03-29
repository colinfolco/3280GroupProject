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
    class clsSearchSQL
    {
        clsMainSQL MainSQL;
        clsInvoice invoice;

        private string sConnectionString;
        //this gets the ConnectionDirectory and stores it in the string sConnectionString
        public clsSearchSQL()
        {
            try
            {
                sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Directory.GetCurrentDirectory() + "\\Invoice.accdb";

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }

        /// <summary>
        /// This executes the SQL staement returning the SQL values as well as the ammpont of items that are in the table
        /// This is used to populate ItemComboBox along with other enteties that display information
        /// </summary>
        /// <param name="sSQL"></param>
        /// <param name="iRetVal"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataSet ExecuteSQLStatement(string sSQL, ref int iRetVal)
        {

            try
            {
                //creates a new DataSet
                DataSet ds = new DataSet();

                using (OleDbConnection conn = new OleDbConnection(sConnectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        //Opens the connection to the database
                        conn.Open();
                        //adds the info for the SelectCommand using the SQL statement and the connection object
                        adapter.SelectCommand = new OleDbCommand(sSQL, conn);
                        adapter.SelectCommand.CommandTimeout = 0;
                        //fills up the DataSet w/ data
                        adapter.Fill(ds);
                    }

                }
                //set the number of values returned
                iRetVal = ds.Tables[0].Rows.Count;
                //returns the DataSet
                return ds;

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
                DataSet invoiceDataSet = ExecuteSQLStatement(sqlQuery, ref rows);
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
                return GetInvoiceTypes(sqlQuery);

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
                return GetInvoiceTypes(sqlQuery);

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
                return GetInvoiceTypes(sqlQuery);

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }


    }
}
