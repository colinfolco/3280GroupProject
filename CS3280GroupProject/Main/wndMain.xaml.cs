using CS3280GroupProject.Items;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        // After search window is closed, check property SelectedInvoiceID in the Search window to see if an invoice is selected. If so, load the invoice.
        // After Items window is closed, check property HasItemsBeenChanged in the Items window to see if any items were updated. If so, re-load items into the combo box.


        private void mnuEditItems_Click(object sender, RoutedEventArgs e)
        {
            var itemsWindow = new wndItems();
            itemsWindow.ShowDialog(); // Modal dialog

            if (itemsWindow.ItemsModified)
            {
                /* 
                 * When Items window closes with changes:
                 * 1. Refresh combo box using clsMainLogic
                 * 2. Reload items from database
                 */
                //freshItemsComboBox();
            }
        }

    private void mnuSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = new Search.wndSearch();
            searchWindow.ShowDialog(); // Opens as a modal dialog
        }



    }
}