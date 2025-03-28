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
            // Execute _sql.GetAllItems() and return items
            return new List<Item>(); // Implementation omitted
        }

        public bool SaveChanges(List<Item> modifiedItems)
        {
            // Execute _sql.UpdateItem() for each modified item
            return true; // Implementation omitted
        }
    }

    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
    }
}
