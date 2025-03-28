using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CS3280GroupProject.Items
{
    public partial class wndItems : Window
    {
        // Public flag to notify main window of changes
        public bool ItemsModified { get; private set; } = false;

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            /* 
             * When saving changes:
             * 1. Business logic validates data
             * 2. SQL class generates update/insert statements
             * 3. ItemsModified flag set to true to notify main window
             */
            ItemsModified = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Close without saving
            this.Close();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder method for btnAdd_Click
            // Implementation will be added later
        }
    }
}
