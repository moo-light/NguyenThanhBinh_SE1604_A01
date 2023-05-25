using BusinessObject;

namespace DataAccess.Interfaces
{
    public interface ICustomerManagement : IGenericManagement<Customer>
    {
        Customer? SignIn(string email, string password);
    }
}