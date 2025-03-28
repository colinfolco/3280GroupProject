using CS3280GroupProject.Items;
using CS3280GroupProject.Search;
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

        private void btnEditItems_Click(object sender, RoutedEventArgs e)
        {
            // Open the Items window as a modal dialog
            var itemsWindow = new wndItems();
            itemsWindow.ShowDialog();
        }

        private void btnSearchInvoice_Click(object sender, RoutedEventArgs e)
        {
            // Open the Search window as a modal dialog
            var searchWindow = new wndSearch();
            searchWindow.ShowDialog();
        }
    }
}