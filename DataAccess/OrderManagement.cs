using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderManagement : GenericManagement<Order>, IOrderManagement
    {
        private static readonly object locker = new object();
        private static OrderManagement _instance = null;

        public OrderManagement(FuFlowerBouquetManagementContext context) : base(context)
        {
            _instance = this;
        }

        public static OrderManagement Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                        _instance = new OrderManagement(_context);
                }
                return _instance;
            }
        }
        public override Order? GetByID(int? id)
        {

            if (id == null) return null;

            return _dbSet.AsNoTracking().Include(x=>x.Customer).FirstOrDefault(x => x.OrderId == id);
        }
        public override void AddNew(Order? entity)
        {
            base.AddNew(entity);
        }

        public override void Update(Order? entity)
        {
            if (entity.ShippedDate < entity.OrderDate) throw new InvalidDataException("ShippedDate can't be smaller than OrderDate");
            
            base.Update(entity);
        }

        public override void Remove(Order? entity)
        {
            base.Remove(entity);
        }
        public IEnumerable<Order> GetCustomerOrderHistories(Customer customer)
        {
            return GetAll().Where(x => x.CustomerId == customer.CustomerId);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
