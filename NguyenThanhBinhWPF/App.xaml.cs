using BusinessObject;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NguyenThanhBinhWPF.AdminContent;
using NguyenThanhBinhWPF.CustomerContent;
using NLog.Extensions.Logging;
using Repository;
using Repository.Interfaces;
using System;
using System.Windows;

namespace NguyenThanhBinhWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IServiceProvider serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);
         
            serviceProvider= services.BuildServiceProvider();

        }

        public static IServiceProvider ServiceProvider { get => serviceProvider; set => serviceProvider = value; }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AuthenticationWindow>();//Login Window

            // Add Admin Window
            services.AddSingleton<AdminWindow>();
            services.AddSingleton<CustomerManagementWindow>();
            services.AddSingleton<FlowerBouquetManagementWindow>();
            services.AddSingleton<OrderManagementWindow>();
           
            // Add Customer Window
            services.AddSingleton<CustomerWindow>();
            services.AddScoped<OrderDetailWindow>();
            services.AddScoped<CustomerProfileWindow>();
            services.AddScoped<CustomerPlaceOrderWindow>();

            // Add Modify WIndow
            services.AddSingleton<CustomerAddOrUpdateWindow>();
            services.AddSingleton<OrderAddOrUpdateWindow>();
            services.AddSingleton<OrderDetailAddOrUpdateWindow>();
            services.AddSingleton<FlowerBouquetAddOrUpdateWindow>();

            NLog.LogManager.LoadConfiguration("nlog.config");
            
            services.AddDbContext<FuFlowerBouquetManagementContext>();

            services.AddSingleton<ICustomerManagement,CustomerManagement>();
            services.AddSingleton<ICustomerRepository,CustomerRepository>();

            services.AddSingleton<IOrderManagement,OrderManagement>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            
            services.AddSingleton<IOrderDetailManagement,OrderDetailManagement>();
            services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
            
            services.AddSingleton<IFlowerBouquetRepository, FlowerBouquetRepository>();
            services.AddSingleton<IFlowerBouquetManagement, FlowerBouquetManagement>();

            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<ICategoryManagement, CategoryManagement>();
            
            services.AddSingleton<ISupplierRepository, SupplierRepository>();
            services.AddSingleton<ISupplierManagement, SupplierManagement>();

        }


        private void OnStartup(object sender, StartupEventArgs e)
        {
            var frmLogin = serviceProvider.GetRequiredService<AuthenticationWindow>();
            frmLogin.ShowDialog();
        }
    }
}
