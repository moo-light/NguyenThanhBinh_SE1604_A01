using BusinessObject;
using NguyenThanhBinhWPF.AdminContent;
using NguyenThanhBinhWPF.Utils;
using Repository.Interfaces;
using System;
using System.ComponentModel;
using System.Windows;

namespace NguyenThanhBinhWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class AdminWindow : Window
{
    public Customer SignedCustomer { get; internal set; } = null!;

    public FlowerBouquetManagementWindow FlowerBouquetManagementWindow { get; }

    public OrderManagementWindow OrderManagementWindow { get; }

    public CustomerManagementWindow CustomerManagementWindow { get; }

    public AdminWindow(CustomerManagementWindow customerManagementWindow,
                       FlowerBouquetManagementWindow flowerBouquetManagementWindow,
                       OrderManagementWindow orderManagementWindow)
    {
        CustomerManagementWindow = customerManagementWindow;
        FlowerBouquetManagementWindow = flowerBouquetManagementWindow;
        OrderManagementWindow = orderManagementWindow;
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void btnViewCustomerInfomation_Click(object sender, RoutedEventArgs e)
    {
        this.ShowAndSetOwnerWindow(CustomerManagementWindow);
        CustomerManagementWindow.LoadCustomerList();
    }

    private void btnViewFlowerBouquetInfomation_Click(object sender, RoutedEventArgs e)
    {
        this.ShowAndSetOwnerWindow(FlowerBouquetManagementWindow);
        
    }

    private void btnViewOrderInfomation_Click(object sender, RoutedEventArgs e)
    {
        this.ShowAndSetOwnerWindow(OrderManagementWindow);
    }

    private void Window_Closing(object sender, CancelEventArgs e) => this.CancelWindowClosing(sender, e);
}
