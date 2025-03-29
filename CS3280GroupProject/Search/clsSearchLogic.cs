using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CS3280GroupProject.Common;

namespace CS3280GroupProject.Search
{
    class clsSearchLogic
    {

        clsSearchLogic SearchSQL;

        public clsSearchLogic()
        {
            SearchSQL = new clsSearchLogic();
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
        public List<clsInvoice> GetInvoices(string invoiceNumber, string invoiceDate, string totalCost)
        {
            try
            {
                List<clsInvoice> invoices = new List<clsInvoice>();

                if (!string.IsNullOrEmpty(invoiceNumber))
                {
                    //call method to get invoices by invoice number
                    //List<clsInvoice> invoiceByNumber = SearchSQL.GetInvoiceNum(invoiceNumber);
                 

                }
                if (!string.IsNullOrEmpty(invoiceDate))
                {
                    //call methos to get invoices by invoice date
                    //List<clsInvoice> invoiceByDate = SearchSQL.GetInvoiceDate(invoiceDate);

                }
                if (!string.IsNullOrEmpty(totalCost))
                {
                    //call methos to get invoices by total cost
                    //List<clsInvoice> invoiceByTotalCost = SearchSQL.GetTotalCost(totalCost);

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
