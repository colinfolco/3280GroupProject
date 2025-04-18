using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupProject.Common
{
    public class clsDataAccess
    {

        //this gets the ConnectionDirectory and stores it in the string sConnectionString
        private string sConnectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public clsDataAccess()
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




    }
}
