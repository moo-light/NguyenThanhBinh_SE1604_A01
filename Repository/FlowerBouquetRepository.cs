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
    public class FlowerBouquetRepository : IFlowerBouquetRepository
    {
        private readonly IFlowerBouquetManagement _flowerBouquetManagement;

        public FlowerBouquetRepository(IFlowerBouquetManagement flowerBouquetManagement)
        {
            _flowerBouquetManagement = flowerBouquetManagement;
        }
        public IQueryable<FlowerBouquet> GetFlowerBouquets
        {
            get
            {
                return FlowerBouquetManagement.Instance.GetAll().Where(x => x.FlowerBouquetStatus == 1).Include(fb => fb.Supplier).Include(fb => fb.Category).OrderBy(x => x.FlowerBouquetId);
            }
        }

        public FlowerBouquet? GetFlowerBouquetByID(int? flowerBouquetId) => FlowerBouquetManagement.Instance.GetByID(flowerBouquetId);
        public void InsertFlowerBouquet(FlowerBouquet flowerBouquet) => FlowerBouquetManagement.Instance.AddNew(flowerBouquet);

        public void DeleteFlowerBouquet(FlowerBouquet flowerBouquet) => FlowerBouquetManagement.Instance.Remove(flowerBouquet);

        public void UpdateFlowerBouquet(FlowerBouquet flowerBouquet) => FlowerBouquetManagement.Instance.Update(flowerBouquet);

        public ObservableCollectionListSource<FlowerBouquet> SearchFlowerBouquetInfomation(string search)
        {
            IQueryable<FlowerBouquet> result = FlowerBouquetManagement.Instance.GetAll().Where(x => x.FlowerBouquetStatus == 1).Include(fb => fb.Supplier).Include(fb => fb.Category).OrderBy(x => x.FlowerBouquetId);
            if (string.IsNullOrEmpty(search))
            {
                return new ObservableCollectionListSource<FlowerBouquet>(result);
            }

            result = result.Where(x => x.FlowerBouquetName.Contains(search)
                                                          || x.Category.CategoryName.Contains(search)
                                                          || x.Supplier.SupplierName.Contains(search));
            return new ObservableCollectionListSource<FlowerBouquet>(result);
        }
    }
}
