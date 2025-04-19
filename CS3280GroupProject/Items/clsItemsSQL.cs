using System;

namespace CS3280GroupProject.Items
{
    /// <summary>
    /// Generates SQL statements for item management
    /// </summary>
    public class clsItemsSQL
    {
        /// <summary>
        /// SQL to retrieve all items
        /// </summary>
        public string GetAllItems() =>
            "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc ORDER BY ItemCode;";

        /// <summary>
        /// SQL to update existing item
        /// </summary>
        public string UpdateItem(string itemCode, string itemName, decimal price) =>
            $"UPDATE ItemDesc SET " +
            $"ItemDesc = '{itemName.Replace("'", "''")}', " +
            $"Cost = {price} " +
            $"WHERE ItemCode = '{itemCode}';";

        /// <summary>
        /// SQL to insert new item
        /// </summary>
        public string InsertItem(string itemCode, string itemName, decimal price) =>
            $"INSERT INTO ItemDesc (ItemCode, ItemDesc, Cost) " +
            $"VALUES ('{itemCode.Replace("'", "''")}', " +
            $"'{itemName.Replace("'", "''")}', {price});";

        /// <summary>
        /// SQL to delete item
        /// </summary>
        public string DeleteItem(string itemCode) =>
            $"DELETE FROM ItemDesc WHERE ItemCode = '{itemCode}';";

        /// <summary>
        /// SQL to check item usage in invoices
        /// </summary>
        public string CheckItemUsage(string itemCode) =>
            $"SELECT COUNT(InvoiceNum) FROM LineItems WHERE ItemCode = '{itemCode}';";
    }
}