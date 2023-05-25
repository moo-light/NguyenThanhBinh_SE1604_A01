using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class OrderDetailWindow : Window
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public Order? Order { get; internal set; }
        public IList<OrderDetail> Details { get; set; }

        public OrderDetailWindow(IOrderDetailRepository orderDetailRepository)
        {
            DataContext = this;
            _orderDetailRepository = orderDetailRepository;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Details = _orderDetailRepository.GetOrderDetailsFromOrderId(Order.OrderId).ToList();
            spOrder.DataContext = Order;
            dgOrderDetail.ItemsSource = Details.Select(x => new
            {
                x.UnitPrice,
                x.Quantity,
                x.Discount,
                x.FlowerBouquet.FlowerBouquetName,
                x.FlowerBouquet.Description,
            });
        }
    }
}
