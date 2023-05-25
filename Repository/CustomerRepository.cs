using BusinessObject;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        ICustomerManagement _management;
        public CustomerRepository(ICustomerManagement management)
        {
            _management = management;
        }
        public Customer? GetCustomerByID(int? customerId) => CustomerManagement.Instance.GetByID(customerId);
        public ObservableCollectionListSource<Customer> GetCustomers => new ObservableCollectionListSource<Customer>(CustomerManagement.Instance.GetAll());

        public void InsertCustomer(Customer customer)
        {
            if (CustomerManagement.Instance.GetAll().FirstOrDefault(x => x.Email.Equals(customer.Email)) != null) throw new InvalidDataException("Email Exist");

            CustomerManagement.Instance.AddNew(customer);
        }

        public void DeleteCustomer(Customer customer) => CustomerManagement.Instance.Remove(customer);

        public void UpdateCustomer(Customer customer) => CustomerManagement.Instance.Update(customer);

        public void DetachCustomer(Customer customer) => CustomerManagement.Instance.Detach(customer);
        public Customer? SignIn(string email, string password)
        {
            var customerFind = CustomerManagement.Instance.SignIn(email,password);
            return customerFind;
        }

        public ObservableCollectionListSource<Customer> SearchCustomerInfomation(string search)
        {
            if (string.IsNullOrEmpty(search)) return new ObservableCollectionListSource<Customer>(CustomerManagement.Instance.GetAll());

            var customers = CustomerManagement.Instance.GetAll().Where(x => x.City.Contains(search)
                                                                            || x.Country.Contains(search)
                                                                            || x.CustomerName.Contains(search)
                                                                            || x.Email.Contains(search));
            return new ObservableCollectionListSource<Customer>(customers);
        }
    }
}
