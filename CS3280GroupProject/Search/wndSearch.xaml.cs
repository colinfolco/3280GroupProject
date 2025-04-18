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

        private List<clsInvoice> allInvoices;
        private List<clsInvoice> displayedInvoices;


        //------------------------------------------------------------------------------------------------------
        // Dummy to hold the selected invoice ID
        public string SelectedInvoiceID { get; private set; } = "12345";
        //------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public void InitializeSearch()
        {

            try
            {
                PopulateComboBoxes();

                allInvoices = searchLogic.GetAllInvoices();

                DisplayInvoices(allInvoices);


            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void PopulateComboBoxes()
        {

            try
            {
                clsSearchLogic searchLogic = new clsSearchLogic();

                List<(string InvoiceNum, string InvoiceDate, string TotalCost)> invoices = searchLogic.GetInvoice();

                List<string> invoiceNumbers = new List<string>();
                List<string> invoiceDates = new List<string>();
                List<string> totalCosts = new List<string>();

                foreach (var invoice in invoices)
                {
                    invoiceNumbers.Add(invoice.InvoiceNum);
                    invoiceDates.Add(invoice.InvoiceDate);
                    totalCosts.Add(invoice.TotalCost);
                    
                }

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
        /// 
        /// </summary>
        /// <param name="invoices"></param>
        /// <exception cref="Exception"></exception>
        private void DisplayInvoices(List<clsInvoice> invoices)
        {

            try
            {

                dataGrid.ItemsSource = invoices;
                displayedInvoices = invoices;

            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }

        }

        private void btnSelectInvoice_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                clsInvoice selectedInvoice = (clsInvoice)dataGrid.SelectedItem;

                clsInvoice.SelectedInvoiceID = Convert.ToInt32(selectedInvoice.InvoiceNumber);

                this.Hide();

                this.Close();



            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

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

        private void DisplayComboBoxes(ComboBox selectedComboBox)
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
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }

        }

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

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnSelectInvoice.IsEnabled = dataGrid.SelectedItem != null;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                    MethodInfo.GetCurrentMethod().Name + " --> " + ex.Message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox selectedComboBox = (ComboBox)sender;

                // Disable other ComboBoxes
                DisableComboBoxes(selectedComboBox);

                // Filter displayed invoices based on ComboBox selection
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

                btnClearFilter.IsEnabled = true;

                // Refresh DataGrid with filtered invoices
                DisplayInvoices(displayedInvoices);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private static void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " --> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }


    }
}
