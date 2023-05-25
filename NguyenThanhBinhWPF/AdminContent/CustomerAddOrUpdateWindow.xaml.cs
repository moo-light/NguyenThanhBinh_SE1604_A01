using BusinessObject;
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

namespace NguyenThanhBinhWPF.AdminContent;

/// <summary>
/// Interaction logic for CustomerAddOrUpdateWindow.xaml
/// </summary>
public partial class CustomerAddOrUpdateWindow : Window, INotifyPropertyChanged
{
    private Customer? _customer;
    private readonly ICustomerRepository _customerRepository;

    public CustomerAddOrUpdateWindow(ICustomerRepository customerRepository)
    {
        this.DataContext = this;
        InitializeComponent();
        _customerRepository = customerRepository;

    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public Customer? Customer
    {
        get => _customer; set
        {
            _customer = value;
            this.OnPropertyChanged(PropertyChanged, nameof(Customer));
        }
    }
    public bool IsUpdate { get; internal set; }
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        Customer = null;
        txtPassword.Password = "";
        this.CancelWindowClosing(sender, e);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        Customer = null;
        txtPassword.Password = "";
        this.Hide();
    }

    private void btnAction_Click(object sender, RoutedEventArgs e)
    {
        try
        {

            if (IsUpdate == false && txtPassword.Password == "") throw new InvalidOperationException("Password is Empty Please Insert Password");
            if (IsUpdate)
            {
                if(string.IsNullOrEmpty(txtPassword.Password))
                Customer.Password = txtPassword.Password;

                _customerRepository.UpdateCustomer(Customer);
            }
            else
            {
                Customer.Password = txtPassword.Password;
                _customerRepository.InsertCustomer(Customer);
            }

            btnCancel_Click(sender, e);
            MessageBox.Show($"{btnAction.Content} Successful!");
        }
        catch
        {

        }
    }

    internal Window PrepareWindow()
    {

        btnAction.Content = IsUpdate ? "Update" : "Add";

        Title = $"{btnAction.Content} Customer {Customer?.CustomerId}";
        return this;
    }
}
