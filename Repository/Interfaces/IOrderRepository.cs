using BusinessObject;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;

namespace Repository.Interfaces
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetOrders { get; }

        void DeleteOrder(Order order);
        Order? GetOrderByID(int? orderId);
        void UpdateOrder(Order order);
        void InsertOrder(Order order);
        ObservableCollectionListSource<Order> SearchOrderInfomation(string search);
        ObservableCollectionListSource<Order> GetCustomerOrderHistories(Customer customer);
        Order? GetOrderByOrderDate(DateTime orderDate);
        void InsertOrderWithOrderDetails(Order order);
    }
}