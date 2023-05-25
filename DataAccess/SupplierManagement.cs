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
    public class SupplierManagement : GenericManagement<Supplier>, ISupplierManagement
    {
        private static readonly object locker = new object();
        private static SupplierManagement _instance = null;

        public SupplierManagement(FuFlowerBouquetManagementContext context) : base(context)
        {
            _instance = this;
        }

        public static SupplierManagement Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                        _instance = new SupplierManagement(_context);
                }
                return _instance;
            }
        }
        public override Supplier? GetByID(int? id)
        {

            if (id == null) return null;

            return _dbSet.FirstOrDefault(x => x.SupplierId == id);
        }
        public override void AddNew(Supplier? entity)
        {
            if (GetByID(entity?.SupplierId) == null)
                base.AddNew(entity);
        }

        public override void Update(Supplier? entity)
        {
            if (GetByID(entity?.SupplierId) != null)
                base.Update(entity);
        }

        public override void Remove(Supplier? entity)
        {
            if (GetByID(entity?.SupplierId) != null)
                base.Remove(entity);
        }

     
    }
}
