using System;
using System.Collections.Generic;
using System.Configuration;
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
using CS3280GroupProject.Common;

namespace CS3280GroupProject.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        MainWindow wndMainWindow;
        clsSearchLogic searchLogic;
        clsInvoice getInvoice;

        //the list to hold allInvoices
        private List<clsInvoice> allInvoices;
        //the list of the displayed Invoices
        private List<clsInvoice> displayedInvoices;

        //------------------------------------------------------------------------------------------------------
        // Dummy to hold the selected invoice ID
        public string SelectedInvoiceID { get; private set; } = "12345";
        //------------------------------------------------------------------------------------------------------

        /// <summary>
        /// constructor
        /// </summary>
        public wndSearch()
        {
            InitializeComponent();

            searchLogic = new clsSearchLogic();
            getInvoice = new clsInvoice();
            InitializeSearch();
            btnClearFilter.IsEnabled = false;
            btnSelectInvoice.IsEnabled = false;

        }

        /// <summary>
        /// this initializes the search window and
        /// populates the comboboxes, and displayes invoices
        /// in the dataGrid
        /// </summary>
        public void InitializeSearch()
        {

            try
            {
                //calls local method to polulate the comboboxes
                PopulateComboBoxes();
                //calls method in clsSearchLogic to get all invoices
                allInvoices = searchLogic.GetAllInvoices();
                //calls local method to display invoices for the dataGrid
                DisplayInvoices(allInvoices);

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// this method populates the comboboxes
        /// </summary>
        private void PopulateComboBoxes()
        {
            try
            {
                // this gets the list of the invoices Tuple
                List<(string InvoiceNum, string InvoiceDate, string TotalCost)> invoices = searchLogic.GetInvoice();

                //creates the 3 lists for each type of invoice
                List<string> invoiceNumbers = new List<string>();
                List<string> invoiceDates = new List<string>();
                List<string> totalCosts = new List<string>();

                //adds each type to the list in from the invoices
                foreach (var invoice in invoices)
                {
                    invoiceNumbers.Add(invoice.InvoiceNum);
                    invoiceDates.Add(invoice.InvoiceDate);
                    totalCosts.Add(invoice.TotalCost);                    
                }
                //adds the orginised type of list to the combobox's 
                cbInvoiceNumber.ItemsSource = invoiceNumbers;
                cbInvoiceDate.ItemsSource = invoiceDates;
                cbTotalCosts.ItemsSource = totalCosts;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// this method displays the invoices in the dataGrid
        /// </summary>
        /// <param name="invoices"></param>
        /// <exception cref="Exception"></exception>
        private void DisplayInvoices(List<clsInvoice> invoices)
        {
            try
            {
                //populates the dataGrid
                dataGrid.ItemsSource = invoices;
                displayedInvoices = invoices;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// this handles the event when the user clicks on Select Invoice btn
        /// (other windows might need this)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectInvoice_Click(object sender, RoutedEventArgs e) // I had to rewrite some of this. It had 0 references so I hope it doesn't break anything?
        {
            try
            {
                clsInvoice selectedInvoice = (clsInvoice)dataGrid.SelectedItem;

                SelectedInvoiceID = selectedInvoice.InvoiceNumber.ToString(); // I changed how things are converted here
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// this method is used for btnClearFilter it clears the datGrid
        /// and reEnables the comboboxes and displayes the invoices again
        /// i.e the "Reset"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //clears combobox selections
                cbInvoiceNumber.SelectedIndex = -1;
                cbInvoiceDate.SelectedIndex = -1;
                cbTotalCosts.SelectedIndex = -1;

                //enables comboboxs
                cbInvoiceNumber.IsEnabled = true;
                cbInvoiceDate.IsEnabled = true;
                cbTotalCosts.IsEnabled = true;

                DisplayInvoices(allInvoices);
                btnClearFilter.IsEnabled = false;

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// this disables the other ComboBoxes when one
        /// of them is clicked, this avoids mismatching when searching 
        /// for a specific invoice (just click on clear btn to select the other ComboBoxes)
        /// </summary>
        /// <param name="selectedComboBox"></param>
        /// <exception cref="Exception"></exception>
        private void DisableComboBoxes(ComboBox selectedComboBox)
        {
            try
            {
                if (selectedComboBox != cbInvoiceNumber)
                {
                    cbInvoiceNumber.IsEnabled = false;
                    cbInvoiceNumber.SelectedIndex = -1;
                }
                if (selectedComboBox != cbInvoiceDate)
                {
                    cbInvoiceDate.IsEnabled = false;
                    cbInvoiceDate.SelectedIndex = -1;
                }
                if (selectedComboBox != cbTotalCosts)
                {
                    cbTotalCosts.IsEnabled = false;
                    cbTotalCosts.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{MethodInfo.GetCurrentMethod().DeclaringType.Name}.{MethodInfo.GetCurrentMethod().Name} -> {ex.Message}");
            }

        }

        /// <summary>
        /// method that handles the event when invoice is clicked
        /// in the dataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //btnSelectInvoice is enabled when an invoice is selected from the dataGrid
                btnSelectInvoice.IsEnabled = dataGrid.SelectedItem != null;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }

        }

        /// <summary>
        /// this method gets called when the user clicks on any specific
        /// combobox and displays that selected invoice in the dataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox selectedComboBox = (ComboBox)sender;

                //disable other ComboBoxes
                DisableComboBoxes(selectedComboBox);

                //filters displayed invoices based on ComboBox selection and strores that selection
                //in "displayedInvoices" to be displayed
                if (selectedComboBox == cbInvoiceNumber)
                {

                    string selectedInvoiceNumber = (string)selectedComboBox.SelectedItem;
                    displayedInvoices = allInvoices.Where(i => i.InvoiceNumber == selectedInvoiceNumber).ToList();
                }
                else if (selectedComboBox == cbInvoiceDate)
                {

                    string selectedInvoiceDate = (string)selectedComboBox.SelectedItem;
                    displayedInvoices = allInvoices.Where(i => i.InvoiceDate == selectedInvoiceDate).ToList();
                }
                else if (selectedComboBox == cbTotalCosts)
                {

                    string selectedTotalCost = (string)selectedComboBox.SelectedItem;
                    displayedInvoices = allInvoices.Where(i => i.TotalCost == selectedTotalCost).ToList();
                }
                //enables clear filter after selection
                btnClearFilter.IsEnabled = true;

                // Refresh DataGrid with filtered invoices
                DisplayInvoices(displayedInvoices);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// method to handle errors
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private static void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " --> " + sMessage);
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);

                string errorFilePath = ConfigurationManager.AppSettings["ErrorLogFilePath"];
                System.IO.File.AppendAllText(errorFilePath, Environment.NewLine + "HandleError Exception: " + ex.Message);

            }
        }

    }
}
