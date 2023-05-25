using BusinessObject;

namespace Repository.Interfaces
{
    public interface IOrderDetailRepository
    {
        IQueryable<OrderDetail> GetOrderDetails { get; }

        void DeleteOrderDetail(OrderDetail orderDetail);
        IQueryable<OrderDetail> GetOrderDetailsFromOrderId(int? orderId);
        void InsertOrderDetail(OrderDetail orderDetail);
        IList<OrderDetail> SearchOrderDetailInfomation(string search, Order? selectedOrder);
        void UpdateOrderDetail(OrderDetail orderDetail);
    }
}