using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using CS3280GroupProject.Common;

namespace CS3280GroupProject.Items
{
    /// <summary>
    /// Handles business logic for item management operations
    /// </summary>
    public class clsItemsLogic
    {
        private readonly clsItemsSQL _sql = new clsItemsSQL();
        private List<Item> _originalItems = new List<Item>();

        /// <summary>
        /// Loads all items from the database
        /// </summary>
        public List<Item> LoadItems()
        {
            try
            {
                var db = new clsDataAccess();
                int numRows = 0;
                var ds = db.ExecuteSQLStatement(_sql.GetAllItems(), ref numRows);

                _originalItems = ds.Tables[0].AsEnumerable().Select(row => new Item
                {
                    ItemID = row["ItemCode"]?.ToString() ?? "",
                    ItemName = row["ItemDesc"]?.ToString() ?? "",
                    Price = decimal.Parse(row["Cost"]?.ToString() ?? "0")
                }).ToList();

                return new List<Item>(_originalItems);
            }
            catch (Exception ex)
            {
                HandleError("Error loading items", ex);
                return new List<Item>();
            }
        }

        /// <summary>
        /// Saves all changes to the database
        /// </summary>
        public bool SaveChanges(List<Item> currentItems)
        {
            try
            {
                var db = new clsDataAccess();
                int rowsAffected = 0;

                // Process updates and inserts
                foreach (var item in currentItems)
                {
                    if (_originalItems.Any(i => i.ItemID == item.ItemID))
                    {
                        db.ExecuteNonQuery(_sql.UpdateItem(item.ItemID, item.ItemName, item.Price), ref rowsAffected);
                    }
                    else
                    {
                        db.ExecuteNonQuery(_sql.InsertItem(item.ItemID, item.ItemName, item.Price), ref rowsAffected);
                    }
                }

                // Process deletions
                foreach (var originalItem in _originalItems)
                {
                    if (currentItems.All(i => i.ItemID != originalItem.ItemID))
                    {
                        var usageCount = db.ExecuteScalarSQL(_sql.CheckItemUsage(originalItem.ItemID));
                        if (Convert.ToInt32(usageCount) > 0)
                        {
                            throw new Exception($"Item {originalItem.ItemID} is used in invoices and cannot be deleted");
                        }
                        db.ExecuteNonQuery(_sql.DeleteItem(originalItem.ItemID), ref rowsAffected);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                HandleError("Error saving items", ex);
                return false;
            }
        }

        private void HandleError(string context, Exception ex) =>
            MessageBox.Show($"{context}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    /// <summary>
    /// Represents an inventory item
    /// </summary>
    public class Item
    {
        public string ItemID { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}