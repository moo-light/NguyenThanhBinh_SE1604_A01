using BusinessObject;

namespace DataAccess.Interfaces
{
    public interface IOrderManagement : IGenericManagement<Order>
    {
        IEnumerable<Order> GetCustomerOrderHistories(Customer customer);
    }
}