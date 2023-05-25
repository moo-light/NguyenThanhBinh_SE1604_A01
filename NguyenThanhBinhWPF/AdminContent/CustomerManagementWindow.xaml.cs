using BusinessObject;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NguyenThanhBinhWPF.Utils;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace NguyenThanhBinhWPF.AdminContent
{
    /// <summary>
    /// Interaction logic for CustomerManagementWindow.xaml
    /// </summary>
    public partial class CustomerManagementWindow : Window
    {
        private ObservableCollectionListSource<Customer>? _customers = null;
        private ICustomerRepository _customerRepository;
        public ObservableCollectionListSource<Customer>? Customers { get => _customers; set => _customers = value; }
        public Customer? Customer { get; set; } = null;
        public CustomerAddOrUpdateWindow CustomerAddOrUpdateWindow { get; }

        public CustomerManagementWindow(ICustomerRepository customerRepository, CustomerAddOrUpdateWindow customerAddOrUpdateWindow)
        {
            InitializeComponent();
            DataContext = this;
            _customerRepository = customerRepository;
            CustomerAddOrUpdateWindow = customerAddOrUpdateWindow;
            Customers = _customerRepository.GetCustomers;
        }

        private void Window_Closing(object sender, CancelEventArgs e) => this.CancelWindowClosing(sender, e);

        private void btnSearch_Click(object sender, RoutedEventArgs e) => LoadCustomerList();

        internal void LoadCustomerList()
        {
            Customers = _customerRepository.SearchCustomerInfomation(txtSearchCriteria.Text);
            lvCustomerList.ItemsSource = Customers;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            CustomerAddOrUpdateWindow.IsUpdate = false;
            _customers = null;
            CustomerAddOrUpdateWindow.Customer = GetCustomerObj();
            CustomerAddOrUpdateWindow.PrepareWindow().ShowDialog();
            LoadCustomerList();
            lvCustomerList.SelectedIndex = lvCustomerList.Items.Count - 1;
        }
        private void lvCustomerList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerAddOrUpdateWindow.IsUpdate = true;
            if (lvCustomerList.SelectedIndex < 0 || Customer == null) return;
            int index = lvCustomerList.SelectedIndex;// update item index
            _customers = null;
            CustomerAddOrUpdateWindow.Customer = GetCustomerObj(getID: true);// get customer Object
            CustomerAddOrUpdateWindow.PrepareWindow().ShowDialog();
            LoadCustomerList();


        }
        private Customer GetCustomerObj(bool getID = false)
        {
            try
            {
                return new Customer
                {
                    CustomerId = getID ? Customer.CustomerId : Customers.OrderBy(x => x.CustomerId).Last().CustomerId + 1, //fixing Identifycation not increasing     
                    CustomerName = Customer?.CustomerName,
                    Email = Customer?.Email,
                    Birthday = Customer?.Birthday,
                    Country = Customer?.Country,
                    Password = Customer?.Password,
                    City = Customer?.City,
                    IsAdmin = false,
                };
            }
            catch
            {
                // new empty Customer
                if (getID == false) return new Customer
                {
                    CustomerId = getID ? Customer.CustomerId : Customers.OrderBy(x => x.CustomerId).Last().CustomerId + 1, //fixing Identifycation not increasing     
                    CustomerName ="" , 
                    City ="" , 
                    Country ="" , 
                    Email ="" , 
                    IsAdmin =false , 
                    Birthday =null , 
                    Password ="" , 
                };

            }
            return null;
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            bool notConfirm = MessageBox.Show("Are You Sure?", $"Deleting {Customer?.Email}", MessageBoxButton.YesNo) != MessageBoxResult.Yes;

            if (notConfirm) return; // return if not Confirm
            if (Customer == null) return; // return if Customer isn't Seleted
            try
            {
                _customerRepository.DeleteCustomer(Customer);
                LoadCustomerList();
                Customer = null;
                lvCustomerList.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Remove Customer Unsuccessful \nex: {ex.Message}");
            }
        }


    }
}
