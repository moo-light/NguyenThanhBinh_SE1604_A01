using BusinessObject;
using NguyenThanhBinhWPF.Utils;
using Repository;
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
    /// Interaction logic for OrderAddOrUpdateWindow.xaml
    /// </summary>
    public partial class OrderAddOrUpdateWindow : Window, INotifyPropertyChanged
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private IList<Customer>? _customers;
        private Order _order;
        private IList<string> _orderStatusList;

        public bool IsUpdate { get; set; }
        public IList<Customer>? Customers
        {
            get => _customers; set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }
        public Order SelectedOrder
        {
            get => _order; set
            {
                _order = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }
        public IList<string> OrderStatusList
        {
            get => _orderStatusList; set
            {
                _orderStatusList = value;
                OnPropertyChanged(nameof(OrderStatusList));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public OrderAddOrUpdateWindow(ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            DataContext = this;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            Customers = _customerRepository.GetCustomers;
            OrderStatusList = new string[]
            {
                "Pending","Shipping","Done"
            }.ToList();

            cbCustomer.SelectedIndex = 0;
            cbOrderStatus.SelectedIndex = 0;
        }
        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //SelectedOrder = GetOrderObj();
                if (IsUpdate)
                {
                    _orderRepository.UpdateOrder(SelectedOrder);
                }
                else
                {
                    // Add Order here doesn't make sense
                }
                MessageBox.Show(SelectedOrder.OrderStatus);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{btnAction.Content} Unsucessful! \n{ex.Message}");
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e) => this.CancelWindowClosing(sender, e);
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal Window LoadOrderData()
        {
            cbOrderStatus.Text = SelectedOrder.OrderStatus.Trim();
            btnAction.Content = IsUpdate ? "Update" : "Add";
            return this;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        internal Order GetOrderObj()
        {
            Order? order;
            if (!IsUpdate)
                order = new Order { OrderId = _orderRepository.GetOrders.Last().OrderId + 1 };
            else
            {
                order = _orderRepository.GetOrderByID(int.Parse(txtOrderId.Text));
            }
            try
            {
                order.OrderStatus = cbOrderStatus.Text;
                order.OrderDate = dpOrderDate.SelectedDate ?? throw new InvalidOperationException("Order Date Cannot be Null");
                order.ShippedDate = dpShippedDate.SelectedDate;
                order.CustomerId = cbCustomer.SelectedValue as int?;
                order.Customer = cbCustomer.SelectedItem as Customer;
                order.Total = SelectedOrder.Total;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                order = null;
            }
            return order;
        }
    }
}
