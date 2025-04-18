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
                        cmbItems.Items.Add(item.ItemName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading items: " + ex.Message);
            }
        }


        /// displays invoice details
        private void DisplayInvoiceDetails(CS3280GroupProject.Main.clsMainLogic.clsInvoice invoice)
        {
            txtInvoiceNumber.Text = invoice.InvoiceNumber.ToString();
            txtInvoiceDate.Text = invoice.InvoiceDate.ToShortDateString();
            txtTotalCost.Text = invoice.TotalCost.ToString("C");
        }

        /// creates a new invoice when clicked
        private void btnCreateInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int invoiceNumber = int.Parse(txtInvoiceNumber.Text);
                DateTime invoiceDate = DateTime.Parse(txtInvoiceDate.Text);
                double totalCost = double.Parse(txtTotalCost.Text);

                var newInvoice = new CS3280GroupProject.Main.clsMainLogic.clsInvoice
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = invoiceDate,
                    TotalCost = totalCost
                };

                logic.SaveNewInvoice(newInvoice);
                MessageBox.Show("Invoice created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating invoice: " + ex.Message);
            }
        }

        /// edits the selected invoice when clicked
        private void btnEditInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int invoiceNumber = int.Parse(txtInvoiceNumber.Text);
                DateTime invoiceDate = DateTime.Parse(txtInvoiceDate.Text);
                double totalCost = double.Parse(txtTotalCost.Text);

                var updatedInvoice = new CS3280GroupProject.Main.clsMainLogic.clsInvoice
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = invoiceDate,
                    TotalCost = totalCost
                };

                logic.EditInvoice(updatedInvoice, updatedInvoice);
                MessageBox.Show("Invoice updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating invoice: " + ex.Message);
            }
        }

        /// adds the selected item to the invoice when the button is clicked
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbItems.SelectedItem == null)
                {
                    MessageBox.Show("Please select an item to add.");
                    return;
                }

                string selectedItem = cmbItems.SelectedItem.ToString();
                double itemCost;

                if (!double.TryParse(txtItemCost.Text, out itemCost))
                {
                    MessageBox.Show("Invalid cost.");
                    return;
                }

                var newItem = new
                {
                    ItemName = selectedItem,
                    Cost = itemCost
                };

                dgInvoiceItems.Items.Add(newItem);

                MessageBox.Show($"Item '{selectedItem}' added.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding item: " + ex.Message);
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
                MessageBox.Show("Item removed");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
