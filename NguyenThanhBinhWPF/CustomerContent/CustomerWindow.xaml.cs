using BusinessObject;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NguyenThanhBinhWPF.CustomerContent
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        private CustomerProfileWindow? _customerProfileWindow;
        private OrderDetailWindow? _orderDetailWindow;
        private Customer signedCustomer = null!;

        public Customer SignedCustomer { get => signedCustomer; internal set => signedCustomer = value; }
        public IList<Order> OrderHistories { get; set; }
        public CustomerPlaceOrderWindow CustomerPlaceOrderWindow { get; }

        public CustomerWindow(IOrderRepository orderRepository, ICustomerRepository customerRepository,
                              CustomerPlaceOrderWindow customerPlaceOrderWindow)
        {
            DataContext = this;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            CustomerPlaceOrderWindow = customerPlaceOrderWindow;
            InitializeComponent();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.Owner.Show();
        }

        private void LoadCustomerData()
        {
            OrderHistories = _orderRepository.GetCustomerOrderHistories(signedCustomer);
            gvOrderHistory.ItemsSource = OrderHistories;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCustomerData();
        }

        private void gvOrderHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gvOrderHistory.SelectedItem == null) return;

            Order? order = gvOrderHistory.SelectedItem as Order;
            _orderDetailWindow = App.ServiceProvider.GetService<OrderDetailWindow>();
            _orderDetailWindow.Order = order;
            _orderDetailWindow.Title = $"Order {order.OrderId}";
            _orderDetailWindow.Show();

        }

        private void btnViewProfile_Click(object sender, RoutedEventArgs e)
        {
            _customerProfileWindow = App.ServiceProvider.GetService<CustomerProfileWindow>();
            _customerProfileWindow.Profile = _customerRepository.GetCustomerByID(signedCustomer.CustomerId);
            bool? dialogResult = _customerProfileWindow.ShowDialog();
            if (dialogResult == true)
            {
                signedCustomer = _customerProfileWindow.Profile;
                _customerRepository.DetachCustomer(_customerProfileWindow.Profile);
                _customerRepository.UpdateCustomer(SignedCustomer);
            }

        }

        private void btnMakeOrder_Click(object sender, RoutedEventArgs e)
        {
            CustomerPlaceOrderWindow.Order = new Order {
                OrderId = _orderRepository.GetOrders.OrderBy(x => x.OrderId).Last().OrderId+1,
                OrderStatus= "Pending",
                CustomerId = SignedCustomer.CustomerId,
                Customer = SignedCustomer,
            };
            CustomerPlaceOrderWindow.LoadWindowData().ShowDialog();
            LoadCustomerData();
        }

        
    }
}
