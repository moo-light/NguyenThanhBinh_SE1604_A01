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
    public class CategoryManagement : GenericManagement<Category>, ICategoryManagement
    {
        private static readonly object locker = new object();
        private static CategoryManagement _instance = null;

        public CategoryManagement(FuFlowerBouquetManagementContext context) : base(context)
        {
            _instance = this;
        }

        public static CategoryManagement Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                        _instance = new CategoryManagement(_context);
                }
                return _instance;
            }
        }
        public override Category? GetByID(int? id)
        {

            if (id == null) return null;

            return _dbSet.FirstOrDefault(x => x.CategoryId == id);
        }
        public override void AddNew(Category? entity)
        {
            if (GetByID(entity?.CategoryId) == null)
                base.AddNew(entity);
        }

        public override void Update(Category? entity)
        {
            if (GetByID(entity?.CategoryId) != null)
                base.Update(entity);
        }

        public override void Remove(Category? entity)
        {
            if (GetByID(entity?.CategoryId) != null)
                base.Remove(entity);
        }

     
    }
}
