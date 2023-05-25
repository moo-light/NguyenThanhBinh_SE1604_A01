using BusinessObject;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
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
    public class OrderRepository : IOrderRepository
    {
        private readonly IOrderManagement _orderManagement;
        private readonly IOrderDetailManagement _orderDetailManagement;

        public OrderRepository(IOrderManagement orderManagement, IOrderDetailManagement orderDetailManagement)
        {
            _orderManagement = orderManagement;
            this._orderDetailManagement = orderDetailManagement;
        }
        public IQueryable<Order> GetOrders
        {
            get
            {
                return OrderManagement.Instance.GetAll().Include(x => x.Customer).OrderByDescending(x => x.OrderDate);
            }
        }

        public Order? GetOrderByID(int? orderId) => OrderManagement.Instance.GetByID(orderId);
        public void InsertOrder(Order order) => OrderManagement.Instance.AddNew(order);

        public void DeleteOrder(Order order) => OrderManagement.Instance.Remove(order);

        public void UpdateOrder(Order order) => OrderManagement.Instance.Update(order);

        public ObservableCollectionListSource<Order> SearchOrderInfomation(string search)
        {
            if (string.IsNullOrEmpty(search)) return new ObservableCollectionListSource<Order>(GetOrders);

            IQueryable<Order> result = GetOrders.Where(x => x.Customer.CustomerName.Contains(search, StringComparison.OrdinalIgnoreCase));
            return new ObservableCollectionListSource<Order>(result);
        }

        public ObservableCollectionListSource<Order> GetCustomerOrderHistories(Customer customer)
        {
            return new ObservableCollectionListSource<Order>(OrderManagement.Instance.GetCustomerOrderHistories(customer));
        }

        public Order? GetOrderByOrderDate(DateTime orderDate)
        {
            return GetOrders.FirstOrDefault(x => x.OrderDate == orderDate);
        }

        public void InsertOrderWithOrderDetails(Order order)
        {
            foreach ( var orderDetail in  order.OrderDetails)
            OrderDetailManagement.Instance.AddNewTransaction(orderDetail);
            order.OrderDate = DateTime.UtcNow;
            OrderManagement.Instance.AddNewTransaction(order);
            OrderManagement.Instance.SaveChanges();
        }
    }
}
