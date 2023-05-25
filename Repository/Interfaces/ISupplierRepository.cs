using BusinessObject;

namespace Repository.Interfaces
{
    public interface ISupplierRepository
    {
        List<Supplier> GetSuppliers { get; }

        void DeleteSupplier(Supplier supplier);
        Supplier? GetSupplierByID(int? supplierId);
        void InsertSupplier(Supplier supplier);
        void UpdateSupplier(Supplier supplier);
    }
}