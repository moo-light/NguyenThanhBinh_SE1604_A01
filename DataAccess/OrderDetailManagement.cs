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
    public class OrderDetailManagement : GenericManagement<OrderDetail>, IOrderDetailManagement
    {
        private static readonly object locker = new object();
        private static OrderDetailManagement _instance = null;

        public OrderDetailManagement(FuFlowerBouquetManagementContext context) : base(context)
        {
            _instance = this;
        }

        public static OrderDetailManagement Instance
        {
            get
            {
                lock (locker)
                {
                    if (_instance == null)
                        _instance = new OrderDetailManagement(_context);
                }
                return _instance;
            }
        }

        
        public override OrderDetail? GetByID(int? id)
        {
            if (id == null) return null;
            return _dbSet.FirstOrDefault(x => x.OrderId == id);
        }
       
    }
}
