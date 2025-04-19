using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupProject.Items
{
    public class clsItemsLogic
    {
        private clsItemsSQL _sql = new clsItemsSQL();

        public List<Item> LoadItems()
        {
            List<Item> items = new List<Item>();

            try
            {
                // get the sql string to select all items
                string sqlQuery = _sql.GetAllItems();

                // create a data access object
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;

                // execute the query
                var ds = db.ExecuteSQLStatement(sqlQuery, ref iRetVal);

                // loop through results
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    Item item = new Item
                    {
                        ItemID = row["ItemCode"].ToString(),
                        ItemName = row["ItemDesc"].ToString(),
                        Price = decimal.Parse(row["Cost"].ToString())
                    };
                    items.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error loading items: " + ex.Message);
            }

            return items;
        }
        /* i also edited this out to write something
        public bool SaveChanges(List<Item> modifiedItems)
        {
            // Execute _sql.UpdateItem() for each modified item (not yet implemented)
            return true;
        }
        */

        public bool SaveChanges(List<Item> modifiedItems)
        {
            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                foreach (var item in modifiedItems)
                {
                    string sql = $"UPDATE ItemDesc SET ItemDesc = '{item.ItemName.Replace("'", "''")}', Cost = {item.Price} WHERE ItemCode = '{item.ItemID}'";
                    int iRetVal = 0;
                    db.ExecuteNonQuery(sql, ref iRetVal);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

    public class Item
    {
        public string ItemID { get; set; }  // <-- changed to string
        public string ItemName { get; set; }
        public decimal Price { get; set; }
    }
}