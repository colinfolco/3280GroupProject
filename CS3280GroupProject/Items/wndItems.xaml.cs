using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CS3280GroupProject.Items
{
    /// <summary>
    /// Interaction logic for item management window
    /// </summary>
    public partial class wndItems : Window
    {
        private readonly clsItemsLogic _itemsLogic = new clsItemsLogic();
        public bool ItemsModified { get; private set; }

        /// <summary>
        /// Initializes item management window
        /// </summary>
        public wndItems()
        {
            InitializeComponent();
            LoadItemsData();
        }

        /// <summary>
        /// Loads items into the DataGrid
        /// </summary>
        private void LoadItemsData()
        {
            try
            {
                dgItems.ItemsSource = _itemsLogic.LoadItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles item deletion requests
        /// </summary>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var item = button?.DataContext as Item;

                if (item != null && dgItems.ItemsSource is List<Item> items)
                {
                    items.Remove(item);
                    dgItems.Items.Refresh();
                    ItemsModified = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles new item creation
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInputs()) return;

                var newItem = new Item
                {
                    ItemID = txtItemCode.Text.Trim(),
                    ItemName = txtItemName.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text)
                };

                if (dgItems.ItemsSource is List<Item> items)
                {
                    items.Add(newItem);
                    dgItems.Items.Refresh();
                    ClearInputs();
                    ItemsModified = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Validates user input for new items
        /// </summary>
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtItemCode.Text))
            {
                MessageBox.Show("Item Code is required", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                MessageBox.Show("Item Name is required", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out _))
            {
                MessageBox.Show("Invalid price format", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clears input fields after successful addition
        /// </summary>
        private void ClearInputs()
        {
            txtItemCode.Clear();
            txtItemName.Clear();
            txtPrice.Clear();
        }

        /// <summary>
        /// Commits changes to the database
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgItems.ItemsSource is List<Item> modifiedItems)
                {
                    if (_itemsLogic.SaveChanges(modifiedItems))
                    {
                        ItemsModified = true;
                        MessageBox.Show("Changes saved successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Closes window without saving
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e) => Close();
    }
}