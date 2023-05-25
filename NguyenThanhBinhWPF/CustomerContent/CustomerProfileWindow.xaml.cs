using BusinessObject;
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
using System.Xml.Linq;

namespace NguyenThanhBinhWPF.CustomerContent;

/// <summary>
/// Interaction logic for CustomerProfileWindow.xaml
/// </summary>
public partial class CustomerProfileWindow : Window
{
    private bool _isEditModeEnabled;
    private bool _dataChange = false;

    private readonly ICustomerRepository _customerRepository;

    public Customer? Profile { get; set; }
    public Customer? ProfileDecoy { get; set; }

    public CustomerProfileWindow(ICustomerRepository customerRepository)
    {
        InitializeComponent();
        this._customerRepository = customerRepository;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ProfileDecoy = Profile.Clone() ;
        DataContext = Profile;
        _dataChange = false;
        txtPassword.Password = Profile.Password;
    }

    private void btnEdit_Click(object sender, RoutedEventArgs e)
    {
        _isEditModeEnabled = true;
        ToggleEditControlsVisibility();
    }

    private void CheckDataChanged()
    {
        Profile.City = blCity.Text;
        if (ProfileDecoy.City != Profile.City) _dataChange = true;

        Profile.CustomerName = blCustomerName.Text;
        if (ProfileDecoy.CustomerName != Profile.CustomerName) _dataChange = true;

        Profile.Email = txtEmail.Text;
        if (ProfileDecoy.Email != Profile.Email) _dataChange = true;

        Profile.Country = blCountry.Text;
        if (ProfileDecoy.Country != Profile.Country) _dataChange = true;

        Profile.Birthday = DateTime.Parse(blBirthday.Text);
        if (ProfileDecoy.Birthday != Profile.Birthday) _dataChange = true;

        Profile.Password = txtPassword.Password;
        if (ProfileDecoy.Password != Profile.Password) _dataChange = true;

    }

    private void btnSave_Click(object sender, RoutedEventArgs e)
    {
        _isEditModeEnabled = false;
        ToggleEditControlsVisibility();
        CheckDataChanged();

    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        _isEditModeEnabled = false;
        ToggleEditControlsVisibility();
        Profile = ProfileDecoy;
        DataContext = Profile;
        blCustomerName.Text = Profile.CustomerName;
        blEmail.Text = Profile.Email;
        blCity.Text = Profile.City;
        blCountry.Text = Profile.Country;
        blBirthday.Text = Profile.Birthday.ToString();
        txtPassword.Password = ProfileDecoy.Password;
    }
    private void ToggleEditControlsVisibility()
    {
        if (_isEditModeEnabled)
        {
            // Hide text blocks and show editable text boxes
            spCustomerView.Children.OfType<TextBlock>().ToList().ForEach(tb => tb.Visibility = Visibility.Collapsed);
            spCustomerView.Children.OfType<TextBox>().ToList().ForEach(tb => tb.Visibility = Visibility.Visible);

            // Show save and cancel buttons, hide edit button
            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            txtPassword.IsEnabled = true;
        }
        else
        {
            // Hide editable text boxes and show text blocks
            spCustomerView.Children.OfType<TextBox>().ToList().ForEach(tb => tb.Visibility = Visibility.Collapsed);
            spCustomerView.Children.OfType<TextBlock>().ToList().ForEach(tb => tb.Visibility = Visibility.Visible);

            // Show edit button, hide save and cancel buttons
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            txtPassword.IsEnabled = false;

        }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        this.DialogResult = _dataChange;
        this.DataContext = null;
        ProfileDecoy = null;
        _customerRepository.DetachCustomer(Profile);
    }

}
