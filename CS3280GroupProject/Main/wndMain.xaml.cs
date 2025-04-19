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

        /// opens the items window and updates the combo box if it was modified
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

        private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                this.Hide();
                wndSearch = new wndSearch();
                wndSearch.InitializeSearch();
                wndSearch.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name +
                    "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
        }

        //----------------------------------------------------------------------------------------------------------------------
        // This method bellow that is commented, is your original method you had...

        /// opens the search window and extracts the invoice ID
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


        /// when the selected item changes, update the cost textbox
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
        }

        /// creates a new invoice when clicked
        private void btnCreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtInvoiceNumber.Text = "TBD";

                DateTime invoiceDate = DateTime.Parse(txtInvoiceDate.Text);
                double totalCost = double.Parse(txtTotalCost.Text);

                var newInvoice = new CS3280GroupProject.Main.clsMainLogic.clsInvoice
                {
                    InvoiceNumber = 0,
                    InvoiceDate = invoiceDate,
                    TotalCost = totalCost
                };

                logic.SaveNewInvoice(newInvoice);
                logic.SaveLineItems(int.Parse(logic.GetMaxInvoiceNumber()));
                MessageBox.Show("Invoice created.");

                string maxInvoiceNum = logic.GetMaxInvoiceNumber();
                txtInvoiceNumber.Text = maxInvoiceNum;

                LockInvoiceFields();
                txtInvoiceNumber.Text = logic.GetMaxInvoiceNumber();
                btnCreateInvoice.IsEnabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating invoice: " + ex.Message);
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




    }
}
