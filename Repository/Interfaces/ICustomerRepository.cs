using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repository.Interfaces
{
    public interface ICustomerRepository
    {
        ObservableCollectionListSource<Customer> GetCustomers { get; }

        void DeleteCustomer(Customer? customer);
        void DetachCustomer(Customer? customer);
        Customer? GetCustomerByID(int? customerId);
        void InsertCustomer(Customer? customer);
        ObservableCollectionListSource<Customer> SearchCustomerInfomation(string search);
        Customer? SignIn(string email, string password);
        void UpdateCustomer(Customer? customer);
    }
}