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

namespace NguyenThanhBinhWPF.AdminContent;

/// <summary>
/// Interaction logic for OrderManagement.xaml
/// </summary>
public partial class OrderManagementWindow : Window, INotifyPropertyChanged
{
    private readonly IOrderDetailRepository _orderDetailRepository;
    private readonly IOrderRepository _orderRepository;
    private IList<Order> _orders;
    private IList<OrderDetail> _orderDetails = null;

    public event PropertyChangedEventHandler? PropertyChanged;

    public OrderAddOrUpdateWindow OrderAddOrUpdateWindow { get; set; }
    public OrderDetailAddOrUpdateWindow OrderDetailAddOrUpdateWindow { get; set; }

    public IList<Order> Orders
    {
        get => _orders; set
        {
            _orders = value;
            OnPropertyChanged(nameof(Orders));
        }
    }
    public Order? SelectedOrder { get; set; }
    public IList<OrderDetail> OrderDetails
    {
        get => _orderDetails; set
        {
            _orderDetails = value;
            OnPropertyChanged(nameof(OrderDetails));
        }
    }
    public OrderDetail? SelectedOrderDetail { get; set; }


    public OrderManagementWindow(IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository,
                                OrderAddOrUpdateWindow orderAddOrUpdateWindow, OrderDetailAddOrUpdateWindow orderDetailAddOrUpdateWindow)
    {
        InitializeComponent();

        DataContext = this;

        _orderDetailRepository = orderDetailRepository;
        _orderRepository = orderRepository;

        OrderAddOrUpdateWindow = orderAddOrUpdateWindow;
        OrderDetailAddOrUpdateWindow = orderDetailAddOrUpdateWindow;

        LoadOrderList();
    }


    private void Window_Closing(object sender, CancelEventArgs e) => this.CancelWindowClosing(sender, e);
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
        LoadOrderList(lvOrderList.SelectedItem);
        LoadOrderDetailList(lvOrderDetailList.SelectedItem);
    }
    private void LoadOrderList(object? item = null)
    {
        if (cbOrder.IsChecked == false)
        {
            Orders = _orderRepository.SearchOrderInfomation("");
            return;
        }

        if (lvOrderList.SelectedIndex < 0) Orders = _orderRepository.SearchOrderInfomation(txtSearchCriteria.Text);
        lvOrderList.SelectedItem = item;
    }
    private void LoadOrderDetailList(object? item = null)
    {
        if (lvOrderList.SelectedIndex < 0) { OrderDetails = null; return; }

        if (cbOrderDetail.IsChecked == false)
        {
            OrderDetails = _orderDetailRepository.GetOrderDetailsFromOrderId(SelectedOrder?.OrderId).ToList();
            return;
        }
        OrderDetails = _orderDetailRepository.SearchOrderDetailInfomation(txtSearchCriteria.Text, SelectedOrder);

        lvOrderDetailList.SelectedItem = item;
    }



    //Add
    private void btnAddOrderDetail_Click(object sender, RoutedEventArgs e)
    {
        OrderDetailAddOrUpdateWindow.IsUpdate = false;

        OrderDetails = null;
        if(SelectedOrder == null)
        {
            MessageBox.Show("Please Select an Order");
            return;
        }
        OrderDetailAddOrUpdateWindow.SelectedOrder = SelectedOrder;
        OrderDetailAddOrUpdateWindow.LoadOrderDetailWindow().ShowDialog();

        LoadOrderDetailList();
    }



    //Update
    private void lvOrderList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OrderAddOrUpdateWindow.IsUpdate = true;

        if (SelectedOrder == null) return;
        
        OrderAddOrUpdateWindow.SelectedOrder = SelectedOrder;
        Orders = null;
        OrderAddOrUpdateWindow.LoadOrderData().ShowDialog();

        LoadOrderList(SelectedOrder);
    }

    private void lvOrderDetailList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OrderDetailAddOrUpdateWindow.IsUpdate = true;

        if (SelectedOrderDetail == null) return;

        OrderDetailAddOrUpdateWindow.SelectedOrderDetail = SelectedOrderDetail;
        OrderDetailAddOrUpdateWindow.SelectedOrder = SelectedOrder;
        _orderDetails = null;
        OrderDetailAddOrUpdateWindow.LoadOrderDetailWindow().ShowDialog();
        LoadOrderList(SelectedOrder);
        LoadOrderDetailList(SelectedOrderDetail);
    }
    //Remove
    private void btnRemoveOrder_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            OrderAddOrUpdateWindow.SelectedOrder = this.SelectedOrder;
            SelectedOrder = OrderAddOrUpdateWindow.GetOrderObj();
            OrderAddOrUpdateWindow.SelectedOrder = null;

            _orderRepository.DeleteOrder(SelectedOrder);
            LoadOrderList();
            MessageBox.Show($"Removed Order Success");

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void btnRemoveOrderDetail_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _orderDetailRepository.DeleteOrderDetail(SelectedOrderDetail);
            LoadOrderDetailList();
            MessageBox.Show($"Removed Detail Success");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    //INotifyChanging Binding


    private void lvOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedOrder = lvOrderList.SelectedItem as Order;
        LoadOrderDetailList();
    }

    private void lvOrderDetailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedOrderDetail = lvOrderDetailList.SelectedItem as OrderDetail;
    }
    private void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
