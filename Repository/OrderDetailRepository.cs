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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly IOrderDetailManagement _orderDetailManagement;

        public OrderDetailRepository(IOrderDetailManagement orderDetailManagement)
        {
            _orderDetailManagement = orderDetailManagement;
        }
        public IQueryable<OrderDetail> GetOrderDetails => OrderDetailManagement.Instance.GetAll();
        public void InsertOrderDetail(OrderDetail orderDetail) => OrderDetailManagement.Instance.AddNew(orderDetail);

        public void DeleteOrderDetail(OrderDetail orderDetail) => OrderDetailManagement.Instance.Remove(orderDetail);

        public void UpdateOrderDetail(OrderDetail orderDetail) => OrderDetailManagement.Instance.Update(orderDetail);

        public IQueryable<OrderDetail> GetOrderDetailsFromOrderId(int? orderId) =>
            OrderDetailManagement.Instance.DbSet.Where(x => orderId == null || x.OrderId == orderId ).Include(x => x.FlowerBouquet);

        public IList<OrderDetail> SearchOrderDetailInfomation(string search, Order? selectedOrder)
        {
            if (string.IsNullOrEmpty(search)) return new ObservableCollectionListSource<OrderDetail>(GetOrderDetailsFromOrderId(selectedOrder?.OrderId));

            IQueryable<OrderDetail> result = GetOrderDetailsFromOrderId(selectedOrder?.OrderId)
                .Where(x => x.FlowerBouquet.FlowerBouquetName.Contains(search, StringComparison.OrdinalIgnoreCase));

            return new ObservableCollectionListSource<OrderDetail>(result);
        }

       
        
    }
}
