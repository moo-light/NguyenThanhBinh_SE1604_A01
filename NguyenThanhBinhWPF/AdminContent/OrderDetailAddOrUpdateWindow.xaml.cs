using BusinessObject;
using Microsoft.EntityFrameworkCore;
using NguyenThanhBinhWPF.Utils;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
    /// Interaction logic for OrderDetailAddOrUpdateWindow.xaml
    /// </summary>
    public partial class OrderDetailAddOrUpdateWindow : Window, INotifyPropertyChanged
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IFlowerBouquetRepository _flowerBouquetRepository;
        private IList<FlowerBouquet>? _flowerBouquets;
        private Order? _order;
        private OrderDetail? _orderDetail;

        public OrderDetailAddOrUpdateWindow(IOrderDetailRepository orderDetailRepository,
                                            IOrderRepository orderRepository,
                                            IFlowerBouquetRepository flowerBouquetRepository)
        {
            DataContext = this;
            InitializeComponent();
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _flowerBouquetRepository = flowerBouquetRepository;

            FlowerBouquets = _flowerBouquetRepository.GetFlowerBouquets.ToList();
        }
        public IList<FlowerBouquet>? FlowerBouquets
        {
            get => _flowerBouquets; set
            {
                _flowerBouquets = value;
                OnPropertyChanged(nameof(FlowerBouquets));
            }
        }
        public bool IsUpdate { get; internal set; }
        public Order? SelectedOrder
        {
            get => _order; set
            {
                _order = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }
        public OrderDetail? SelectedOrderDetail
        {
            get => _orderDetail; set
            {
                _orderDetail = value;
                OnPropertyChanged(nameof(SelectedOrderDetail));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        internal Window LoadOrderDetailWindow()
        {
            if(!IsUpdate) LoadOrderDetail();
            Title = $"Order {SelectedOrder.OrderId} Detail";
            btnAction.Content = IsUpdate ? "Update" : "Add";
            return this;
        }

        private void LoadOrderDetail()
        {
            if (!IsUpdate)
            {
                SelectedOrderDetail = new();
                SelectedOrderDetail.FlowerBouquet = FlowerBouquets.FirstOrDefault();
                SelectedOrderDetail.Quantity = 1;
                SelectedOrderDetail.OrderId = SelectedOrder.OrderId;
            }
            else
            {
                try
                {
                    _orderDetailRepository.UpdateOrderDetail(SelectedOrderDetail);
                }
                catch { }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e) => this.CancelWindowClosing(sender, e);

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            SelectedOrderDetail = GetOrderDetailObj();

            if (SelectedOrderDetail == null) return;

            try
            {
                if (IsUpdate)
                {
                    _orderDetailRepository.UpdateOrderDetail(SelectedOrderDetail);
                }
                else
                {
                    _orderDetailRepository.InsertOrderDetail(SelectedOrderDetail);
                }
                var details =_orderDetailRepository.GetOrderDetailsFromOrderId(SelectedOrder.OrderId);
                decimal sum = 0;
                foreach(var order in details)
                {
                    sum += order.ActualPrice; 
                }
                SelectedOrder.Total = sum;
                _orderRepository.UpdateOrder(SelectedOrder);
                MessageBox.Show($"{btnAction.Content} Success");
                this.Hide();
            }
            catch
            {
                MessageBox.Show($"{btnAction.Content} Unsuccessful");
            }
        }

        private OrderDetail? GetOrderDetailObj()
        {
            try
            {
                if (IsUpdate)
                {
                    SelectedOrderDetail.OrderId = SelectedOrder.OrderId;
                }
                else
                {

                }

                int quantity = int.Parse(txtQuantity.Text);
                SelectedOrderDetail.Quantity = quantity > 0 ? quantity : throw new Exception("Quantity must be > 0");
                SelectedOrderDetail.Discount = double.Parse(txtDiscount.Text);
                SelectedOrderDetail.UnitPrice = decimal.Parse(tbUnitPrice.Text);
                //SelectedOrderDetail.FlowerBouquet = cbFlowerBouquet.SelectedItem as FlowerBouquet ?? throw new Exception("Please Select Flower ");
                SelectedOrderDetail.FlowerBouquetId = Convert.ToInt32(cbFlowerBouquet.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LoadOrderDetail();
            }
            return SelectedOrderDetail;
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void cbFlowerBouquet_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateActPrice();
        private void txtUnitPrice_TextChanged(object sender, TextChangedEventArgs e) => UpdateActPrice();
        private void txtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (double.Parse(txtDiscount.Text) > 100) txtDiscount.Text = "100";
                if (double.Parse(txtDiscount.Text) < 0) txtDiscount.Text = "0";
            }
            catch { };
            UpdateActPrice();
        }

        private void UpdateActPrice()
        {
            try
            {
                double unitPrice = Convert.ToDouble(tbUnitPrice.Text);
                double percentage = (1 - Convert.ToDouble(txtDiscount.Text) / 100);
                double quantity = double.Parse(txtQuantity.Text);
                txtUnitPrice.Text = (unitPrice * percentage * quantity).ToString("N2");
            }
            catch { }
        }

    }
}
