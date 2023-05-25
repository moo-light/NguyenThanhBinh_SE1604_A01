using BusinessObject;
using NguyenThanhBinhWPF.Utils;
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
    /// Interaction logic for CustomerPlaceOrderWindow.xaml
    /// </summary>
    public partial class CustomerPlaceOrderWindow : Window, INotifyPropertyChanged
    {
        private Order _order;
        private OrderDetail _selectedOrderDetail;
        private IList<FlowerBouquet> _flowerBouquets;
        private FlowerBouquet _selectedFlowerBouquet;
        private readonly IOrderRepository _orderRepository;
        private ObservableCollection<OrderDetail> _orderDetails = new ObservableCollection<OrderDetail>();
        private readonly IFlowerBouquetRepository _flowerBouquetRepository;

        public CustomerPlaceOrderWindow(IFlowerBouquetRepository flowerBouquetRepository, IOrderRepository orderRepository)
        {
            DataContext = this;
            InitializeComponent();
            _flowerBouquetRepository = flowerBouquetRepository;
            _orderRepository = orderRepository;

            FlowerBouquets = _flowerBouquetRepository.GetFlowerBouquets.ToList();
            SelectedFlowerBouquet = FlowerBouquets.FirstOrDefault();
        }
        //Property Region
        #region Property
        public Order Order
        {
            get => _order; set
            {
                _order = value;
                this.OnPropertyChanged(PropertyChanged, nameof(Order));
            }
        }
        public IList<FlowerBouquet> FlowerBouquets
        {
            get => _flowerBouquets; set
            {
                _flowerBouquets = value;
                this.OnPropertyChanged(PropertyChanged, nameof(FlowerBouquets));
            }
        }
        public FlowerBouquet SelectedFlowerBouquet
        {
            get => _selectedFlowerBouquet; set
            {
                _selectedFlowerBouquet = value;
                this.OnPropertyChanged(PropertyChanged, nameof(SelectedFlowerBouquet));
            }
        }
        public ObservableCollection<OrderDetail> OrderDetails
        {
            get => _orderDetails; set
            {

                _orderDetails = value;
                this.OnPropertyChanged(PropertyChanged, nameof(OrderDetails));
            }
        }
        public OrderDetail SelectedOrderDetail
        {
            get => _selectedOrderDetail; set
            {
                _selectedOrderDetail = value;
                this.OnPropertyChanged(PropertyChanged, nameof(SelectedOrderDetail));
            }
        }


        #endregion
        public event PropertyChangedEventHandler? PropertyChanged;
        public CustomerPlaceOrderWindow(OrderDetail selectedOrderDetail)
        {
            SelectedOrderDetail = selectedOrderDetail;
        }
        private OrderDetail? GetOrderDetailObj()
        {
            try
            {
                return new OrderDetail
                {
                    OrderId = Order.OrderId,
                    FlowerBouquet = SelectedFlowerBouquet,
                    FlowerBouquetId = SelectedFlowerBouquet.FlowerBouquetId,
                    Discount = Convert.ToDouble(txtDiscount.Text),
                    Quantity = Convert.ToInt32(txtQuantity.Text),
                    UnitPrice = SelectedFlowerBouquet.UnitPrice
                };
            }
            catch { return null; }
        }


        private void btnAddOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderDetail orderDetail = GetOrderDetailObj();
                if (orderDetail == null) return;
                if(OrderDetails.FirstOrDefault(x=>x.FlowerBouquetId == orderDetail.FlowerBouquetId)!=null)
                {
                    MessageBox.Show("Cant add The Same Flower Boutquet Please Remove it So We can Add it again");
                    return;
                }

                OrderDetails.Add(orderDetail);
                SelectedOrderDetail = orderDetail;
                this.OnPropertyChanged(PropertyChanged, nameof(OrderDetails));

            }
            catch (Exception ex) { }
        }


        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            OrderDetails.Remove(SelectedOrderDetail);
        }

        // Quantity Modify
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrderDetail != null)
            {
                SelectedOrderDetail.Quantity += 1;
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedOrderDetail != null)
            {
                SelectedOrderDetail.Quantity -= 1;
            }
        }

        internal Window LoadWindowData()
        {
            return this;
        }

        private void btnPlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order.OrderDetails = OrderDetails;
                _orderRepository.InsertOrderWithOrderDetails(Order);
                MessageBox.Show("Added Success");
                this.Hide();
            }
            catch (Exception ex) { }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedOrderDetail = null;
            this.Hide();
        }
    }
}
