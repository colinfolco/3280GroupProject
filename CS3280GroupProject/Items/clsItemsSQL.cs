﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3280GroupProject.Items
{
    public class clsItemsSQL
    {
        // Get all items
        public string GetAllItems()
        {
            return "SELECT ItemID, ItemName, Price FROM Items;";
        }

        // Update existing item
        public string UpdateItem(int itemID, string itemName, decimal price)
        {
            return $"UPDATE Items SET ItemName = '{itemName.Replace("'", "''")}', Price = {price} WHERE ItemID = {itemID};";
        }

        // Add new item
        public string InsertItem(string itemName, decimal price)
        {
            return $"INSERT INTO Items (ItemName, Price) VALUES ('{itemName.Replace("'", "''")}', {price});";
        }
    }
}
