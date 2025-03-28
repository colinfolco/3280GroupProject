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
    }
}