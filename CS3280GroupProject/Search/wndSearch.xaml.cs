using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS3280GroupProject.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        MainWindow wndMainWindow;
        clsSearchSQL SearchLogic;
    //------------------------------------------------------------------------------------------------------
        // Dummy to hold the selected invoice ID
        public string SelectedInvoiceID { get; private set; } = "12345";
        public wndSearch()
        {
            InitializeComponent();
        }

        private void btnSelectInvoice_Click(object sender, RoutedEventArgs e)
        {
            // For now, just set a static ID
            SelectedInvoiceID = "12345";
            this.DialogResult = true;
            this.Close();
        }

        // Cancel button to close the window without selecting
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    //------------------------------------------------------------------------------------------------------
        private void cbInvoiceNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbInvoiceDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbTotalCosts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                wndMainWindow = new MainWindow();
                this.Hide();
                wndMainWindow.Show();

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }
        }
    }
}
