// See https://aka.ms/new-console-template for more information
using BusinessObject;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Interfaces;
using System.Threading.Channels;

IServiceCollection services = Configuring();
IServiceProvider serviceProvider = services.BuildServiceProvider();
var context = serviceProvider.GetService<FuFlowerBouquetManagementContext>();
Console.WriteLine(context.ContextId);
Console.WriteLine();
context.GetType().GetProperties().ToList().ForEach(x => Console.WriteLine($"{x.Name}"));
Console.WriteLine(context.Database.GetDbConnection().ConnectionString);
var repo = serviceProvider.GetService<ICustomerRepository>();

var customer =  repo.GetCustomerByID(1);
customer.City = "ABC";
repo.UpdateCustomer(customer);


static IServiceCollection Configuring()
{
    IServiceCollection services = new ServiceCollection();
    services.AddDbContext<FuFlowerBouquetManagementContext>();
    services.AddSingleton<ICustomerRepository, CustomerRepository>();
    services.AddSingleton<ICustomerManagement, CustomerManagement>();

    return services;
}