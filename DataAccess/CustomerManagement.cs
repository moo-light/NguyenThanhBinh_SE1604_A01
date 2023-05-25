using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CustomerManagement : GenericManagement<Customer>, ICustomerManagement
    {
        private static readonly object locker = new object();
        private static CustomerManagement _instance = null;

        public CustomerManagement(FuFlowerBouquetManagementContext context) : base(context)
        {
            _instance = this;
        }

        public static CustomerManagement Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                        _instance = new CustomerManagement(_context);
                }
                return _instance;
            }
        }

        public override IQueryable<Customer> GetAll()
        {
            return base.GetAll().Select(c => new Customer
            {
                CustomerId = c.CustomerId,
                Email = c.Email,
                CustomerName = c.CustomerName,
                City = c.City,
                Country = c.Country,
                Birthday = c.Birthday,
                IsAdmin = c.IsAdmin
            });
        }

        public override Customer? GetByID(int? id)
        {

            if (id == null) return null;

            return _dbSet.FirstOrDefault(x => x.CustomerId == id);
        }
        public override void AddNew(Customer? entity)
        {
            base.AddNew(entity);
        }

        public override void Update(Customer? entity)
        {
            Customer? customer = GetByID(entity.CustomerId);
            if (customer == null) return;
            
            if (string.IsNullOrEmpty(entity?.Password))
                entity.Password = customer.Password;
            
            Detach(customer); //prevent dataTracking
            base.Update(entity);
        }


        public override void Remove(Customer? entity)
        {
            base.Remove(entity);
        }
        public override void Detach(Customer? customer)
        {
            base.Detach(customer);
        }
        public static bool CheckAdminAccount(string email,string password)
        {
            var config = FuFlowerBouquetManagementContext.GetConfiguration();

            return config.GetSection("admin")
                         .GetChildren()
                         .FirstOrDefault(cf => cf["email"] == email && cf["password"] == password) != null;

        }

        public Customer? SignIn(string email, string password)
        {
            if (CheckAdminAccount(email, password)) return new Customer
            {
                Email = email,
                Password = password,
                IsAdmin = true
            };

            return DbSet.FirstOrDefault(x => x.Email == email && x.Password == password);
        }
    }
}
