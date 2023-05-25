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

namespace Repository;

public class SupplierRepository : ISupplierRepository
{
    private readonly ISupplierManagement _supplierManagement;

    public SupplierRepository(ISupplierManagement supplierManagement)
    {
        _supplierManagement = supplierManagement;
    }
    public Supplier? GetSupplierByID(int? supplierId) => SupplierManagement.Instance.GetByID(supplierId);
    public List<Supplier> GetSuppliers => SupplierManagement.Instance.GetAll().ToList();
    public void InsertSupplier(Supplier supplier) => SupplierManagement.Instance.AddNew(supplier);

    public void DeleteSupplier(Supplier supplier) => SupplierManagement.Instance.Remove(supplier);

    public void UpdateSupplier(Supplier supplier) => SupplierManagement.Instance.Update(supplier);


}
