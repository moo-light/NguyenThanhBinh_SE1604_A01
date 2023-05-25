using BusinessObject;
using NguyenThanhBinhWPF.CustomerContent;
using Repository.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace NguyenThanhBinhWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        private readonly AdminWindow _adminWindow;
        private readonly CustomerWindow _customerWindow;
        private readonly ICustomerRepository _customerRepository;
        public AuthenticationWindow(AdminWindow adminWindow, CustomerWindow customerWindow, ICustomerRepository customerRepository)
        {
            InitializeComponent();
            this._adminWindow = adminWindow;
            this._customerWindow = customerWindow;
            _customerRepository = customerRepository;


        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = txtPassword.Password;

            var customer = _customerRepository.SignIn(email, password);

            if (customer == null)
            {
                MessageBox.Show("Email or Password are Wrong", $"{email} Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Window window;

            
            if (customer.IsAdmin)
            {
                window = _adminWindow;
                _adminWindow.SignedCustomer = customer;
            }
            else
            {
                window = _customerWindow;
                _customerWindow.SignedCustomer = customer;
            }
            // If login Successful open Main Screen
            this.Hide();
            txtEmail.Text = "";
            txtPassword.Password = "";
            
            window.Show();
            window.Owner = this;
            window.Title = this.Title;// Easy Title name Changing
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (Debugger.IsAttached)
            //{  
            //    // quick login for debugging purpurses
            //    txtEmail.Text = "DavidCopperfield@fuflowerbouquetsystem.com";
            //    txtPassword.Password = "1";
            //    btnLogin_Click(sender, e);
            //}
        }


        private void Window_Close(object sender, EventArgs e)
        {
            Environment.Exit(0);

        }
    }
}
