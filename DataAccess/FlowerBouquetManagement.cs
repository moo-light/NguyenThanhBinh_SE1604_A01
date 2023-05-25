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
    public class FlowerBouquetManagement : GenericManagement<FlowerBouquet>, IFlowerBouquetManagement
    {
        private static readonly object locker = new object();
        private static FlowerBouquetManagement _instance = null;

        public FlowerBouquetManagement(FuFlowerBouquetManagementContext context) : base(context)
        {
            _instance = this;
        }

        public static FlowerBouquetManagement Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                        _instance = new FlowerBouquetManagement(_context);
                }
                return _instance;
            }
        }
        public override FlowerBouquet? GetByID(int? id)
        {

            if (id == null) return null;

            return _dbSet.FirstOrDefault(x => x.FlowerBouquetId == id);
        }
        public override void AddNew(FlowerBouquet? entity)
        {
            base.AddNew(entity);
        }

        public override void Update(FlowerBouquet? entity)
        {
            base.Update(entity);
        }

        public override void Remove(FlowerBouquet? entity)
        {
            if(entity == null) return;

            if (_context.OrderDetails.FirstOrDefault(x => x.FlowerBouquetId == entity.FlowerBouquetId) != null)
            { 
                entity.FlowerBouquetStatus = 0;
                base.Update(entity);
            }
            else
            {
                base.Remove(entity);
            }
        }
    }
}
