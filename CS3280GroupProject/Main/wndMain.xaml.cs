using CS3280GroupProject.Common;
using CS3280GroupProject.Items;
using CS3280GroupProject.Search;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS3280GroupProject
{
    public partial class MainWindow : Window
    {
        private CS3280GroupProject.Main.clsMainLogic logic;

        // Testing Search Part-----------------
        wndSearch wndSearch;
        // ------------------------------------

        public MainWindow()
        {
            InitializeComponent();
            logic = new CS3280GroupProject.Main.clsMainLogic();
            LoadItemsComboBox();

            // Testing Search Part-----------------
            wndSearch = new wndSearch();
            // ------------------------------------

        }

        /// opens the items window and updates the combo box
        private void btnEditItems_Click(object sender, RoutedEventArgs e)
        {
            var itemsWindow = new CS3280GroupProject.Items.wndItems();
            itemsWindow.ShowDialog();

            if (itemsWindow.ItemsModified)
            {
                MessageBox.Show("Items have been updated. refreshing the list.");
                LoadItemsComboBox();
            }
        }

        // Testing Search Part -------------------------------------------------------------------------------------------------

        // I commented out your method --> (private void btnSearchInvoice_Click) that is bellow this method for testing on the event of clicking search for your part
        // you may make changes as required

        // all good. i had to edit it a lot so i hope everything still works everywhere? i haven't noticed an issue with the search window.
        private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                wndSearch = new wndSearch();
                wndSearch.InitializeSearch();
                bool? result = wndSearch.ShowDialog();
                this.Show();

                if (result == true)
                {
                    string invoiceID = wndSearch.SelectedInvoiceID;

                    if (!string.IsNullOrEmpty(invoiceID))
                    {
                        var invoice = logic.GetInvoice(invoiceID);

                        if (invoice != null)
                        {
                            DisplayInvoiceDetails(invoice);
                        }
                        else
                        {
                            MessageBox.Show("Invoice not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }



        //----------------------------------------------------------------------------------------------------------------------
        // This method bellow that is commented, is your original method you had...

        /// opens the search window and gets the invoice ID
        //private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        //{
        //    var searchWindow = new wndSearch();
        //    if (searchWindow.ShowDialog() == true)
        //    {
        //        string invoiceID = searchWindow.SelectedInvoiceID;
        //        var invoice = logic.GetInvoice(invoiceID);

        //        if (invoice != null)
        //        {
        //            MessageBox.Show("Invoice ID: " + invoiceID);
        //            DisplayInvoiceDetails(invoice);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Invoice not found.");
        //        }
        //    }
        //}

        //--------------------------------------------------------------------------------------------------------------------------

        /// loads items into the combo box
        private void LoadItemsComboBox()
        {
            try
            {
                var itemsLogic = new CS3280GroupProject.Items.clsItemsLogic();
                var items = itemsLogic.LoadItems();
                cmbItems.Items.Clear();

                if (items != null && items.Count > 0)
                {
                    foreach (var item in items)
                    {
                        cmbItems.Items.Add(item);
                    }
                    cmbItems.DisplayMemberPath = "ItemName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error loading items: " + ex.Message);
            }
        }


        /// when the selected item changes update the cost textbox
        private void cmbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbItems.SelectedItem != null)
                {
                    var selectedItem = (Item)cmbItems.SelectedItem; // cast directly to Item
                    txtItemCost.Text = selectedItem.Price.ToString("0.00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading item cost: " + ex.Message);
            }
        }


        /// displays invoice details
        private void DisplayInvoiceDetails(CS3280GroupProject.Main.clsMainLogic.clsInvoice invoice)
        {
            txtInvoiceNumber.Text = invoice.InvoiceNumber.ToString();
            txtInvoiceDate.Text = invoice.InvoiceDate.ToShortDateString();
            txtTotalCost.Text = invoice.TotalCost.ToString("0.00");
            LockInvoiceFields();

            LoadInvoiceItems(invoice.InvoiceNumber);
        }

        /// the most painful button of all. creates an invoice
        private void btnCreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtInvoiceDate.Text))
                {
                    MessageBox.Show("please enter an invoice date.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTotalCost.Text))
                {
                    MessageBox.Show("please enter a total cost.");
                    return;
                }

                if (!double.TryParse(txtTotalCost.Text, out double totalCost))
                {
                    MessageBox.Show("please enter a valid number for total cost.");
                    return;
                }

                if (totalCost < 0)
                {
                    MessageBox.Show("total cost cannot be negative.");
                    return;
                }

                if (!DateTime.TryParse(txtInvoiceDate.Text, out DateTime invoiceDate))
                {
                    MessageBox.Show("please enter a valid invoice date.");
                    return;
                }

                int invoiceNumber;
                bool isNewInvoice = string.IsNullOrWhiteSpace(txtInvoiceNumber.Text);

                if (isNewInvoice)
                {
                    invoiceNumber = 1;
                    string maxInvoiceNumStr = logic.GetMaxInvoiceNumber();
                    if (int.TryParse(maxInvoiceNumStr, out int maxInvoiceNum))
                    {
                        invoiceNumber = maxInvoiceNum + 1;
                    }
                }
                else
                {
                    invoiceNumber = int.Parse(txtInvoiceNumber.Text);
                }

                var invoice = new CS3280GroupProject.Main.clsMainLogic.clsInvoice
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = invoiceDate,
                    TotalCost = totalCost
                };

                logic.SaveNewInvoice(invoice);

                logic.DeleteLineItems(invoiceNumber);
                logic.SaveLineItems(invoiceNumber);

                MessageBox.Show($"invoice {invoiceNumber} saved successfully.");

                txtInvoiceNumber.Text = invoiceNumber.ToString();
                LockInvoiceFields();
                btnCreateInvoice.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("error saving invoice: " + ex.Message);
            }
        }




        /// unlocks invoice fields so user can edit them
        private void UnlockInvoiceFields()
        {
            txtInvoiceDate.IsReadOnly = false;
            cmbItems.IsEnabled = true;
            btnAddItem.IsEnabled = true;
            btnRemoveItem.IsEnabled = true;
            dgInvoiceItems.IsEnabled = true;
        }




        /// unlocks the invoice fields for editing when clicked
        private void btnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtInvoiceNumber.IsReadOnly = false;
                txtInvoiceDate.IsReadOnly = false;
                txtTotalCost.IsReadOnly = false;
                cmbItems.IsEnabled = true;
                btnAddItem.IsEnabled = true;
                btnRemoveItem.IsEnabled = true;
                dgInvoiceItems.IsEnabled = true;
                btnCreateInvoice.IsEnabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unlocking invoice fields: " + ex.Message);
            }
        }




        /// locks invoice fields so user can't edit them
        private void LockInvoiceFields()
        {
            txtInvoiceNumber.IsReadOnly = true;
            txtInvoiceDate.IsReadOnly = true;
            txtTotalCost.IsReadOnly = true;
            cmbItems.IsEnabled = false;
            btnAddItem.IsEnabled = false;
            btnRemoveItem.IsEnabled = false;
            dgInvoiceItems.IsEnabled = false;
        }


        /// adds the selected item to the invoice when clicked
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbItems.SelectedItem == null)
                {
                    MessageBox.Show("please select an item to add.");
                    return;
                }

                var selectedItem = (Item)cmbItems.SelectedItem;

                var newItem = new
                {
                    ItemName = selectedItem.ItemName,
                    Cost = selectedItem.Price
                };

                dgInvoiceItems.Items.Add(newItem);
                UpdateTotalCost();
                MessageBox.Show($"item '{selectedItem.ItemName}' added.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("error adding item: " + ex.Message);
            }
        }


        /// removes the selected item from the invoice when clicked
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgInvoiceItems.SelectedItem == null)
                {
                    MessageBox.Show("Select an item to remove");
                    return;
                }

                dgInvoiceItems.Items.Remove(dgInvoiceItems.SelectedItem);
                UpdateTotalCost();
                MessageBox.Show("Item removed");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// recalculates the total cost based on all items currently in the datagrid
        private void UpdateTotalCost()
        {
            try
            {
                double total = 0;

                foreach (var item in dgInvoiceItems.Items)
                {
                    if (item == null) continue;

                    var costProperty = item.GetType().GetProperty("Cost");
                    if (costProperty != null)
                    {
                        var costValue = costProperty.GetValue(item, null);
                        if (costValue != null && double.TryParse(costValue.ToString(), out double cost))
                        {
                            total += cost;
                        }
                    }
                }

                txtTotalCost.Text = total.ToString("0.00");

                int invoiceNumber;
                if (int.TryParse(txtInvoiceNumber.Text, out invoiceNumber) && invoiceNumber != 0)
                {
                    var db = new CS3280GroupProject.Common.clsDataAccess();
                    string sqlQuery = $"UPDATE Invoices SET TotalCost = {total} WHERE InvoiceNum = {invoiceNumber}";
                    int iRetVal = 0;
                    db.ExecuteNonQuery(sqlQuery, ref iRetVal);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating total cost in database: " + ex.Message);
            }
        }



        /// updates the total cost in the database for the current invoice
        private void UpdateTotalCostInDatabase()
        {
            try
            {
                if (txtInvoiceNumber.Text != "TBD")
                {
                    int invoiceNumber = int.Parse(txtInvoiceNumber.Text);
                    double totalCost = double.Parse(txtTotalCost.Text);

                    var sqlQuery = CS3280GroupProject.Main.clsMainSQL.UpdateInvoice(invoiceNumber.ToString(), totalCost);

                    var db = new CS3280GroupProject.Common.clsDataAccess();
                    int iRetVal = 0;
                    db.ExecuteNonQuery(sqlQuery, ref iRetVal);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating total cost in database: " + ex.Message);
            }
        }

        /// loads all items for a specific invoice into the datagrid
        private void LoadInvoiceItems(int invoiceNum)
        {
            try
            {
                var db = new CS3280GroupProject.Common.clsDataAccess();
                int iRetVal = 0;

                string sqlQuery = $@"
            SELECT ItemDesc.ItemDesc, ItemDesc.Cost
            FROM LineItems
            INNER JOIN ItemDesc ON LineItems.ItemCode = ItemDesc.ItemCode
            WHERE LineItems.InvoiceNum = {invoiceNum}";

                var ds = db.ExecuteSQLStatement(sqlQuery, ref iRetVal);

                dgInvoiceItems.Items.Clear();

                for (int i = 0; i < iRetVal; i++)
                {
                    var item = new
                    {
                        ItemName = ds.Tables[0].Rows[i]["ItemDesc"].ToString(),
                        Cost = decimal.Parse(ds.Tables[0].Rows[i]["Cost"].ToString())
                    };
                    dgInvoiceItems.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading invoice items: " + ex.Message);
            }
        }
        /// clears invoice fields
        private void ClearInvoiceFields()
        {
            txtInvoiceNumber.Text = "";
            txtInvoiceDate.Text = "";
            txtTotalCost.Text = "";
            dgInvoiceItems.Items.Clear();
        }


        /// deletes the currently loaded invoice
        private void btnDeleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtInvoiceNumber.Text) || txtInvoiceNumber.Text == "TBD")
                {
                    MessageBox.Show("No invoice selected to delete.");
                    return;
                }

                var result = MessageBox.Show("Are you sure you want to delete this invoice?", "Confirm Delete", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    int invoiceNumber = int.Parse(txtInvoiceNumber.Text);

                    // delete from LineItems and Invoices
                    var db = new CS3280GroupProject.Common.clsDataAccess();
                    int iRetVal = 0;

                    string deleteLineItemsSQL = $"DELETE FROM LineItems WHERE InvoiceNum = {invoiceNumber}";
                    db.ExecuteNonQuery(deleteLineItemsSQL, ref iRetVal);

                    string deleteInvoiceSQL = $"DELETE FROM Invoices WHERE InvoiceNum = {invoiceNumber}";
                    db.ExecuteNonQuery(deleteInvoiceSQL, ref iRetVal);

                    MessageBox.Show("Invoice deleted successfully.");

                    // clear fields after deleting
                    ClearInvoiceFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting invoice: " + ex.Message);
            }
        }





    }
}
