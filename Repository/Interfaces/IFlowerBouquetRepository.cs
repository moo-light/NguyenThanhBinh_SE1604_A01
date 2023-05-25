using BusinessObject;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;

namespace Repository.Interfaces
{
    public interface IFlowerBouquetRepository
    {
        IQueryable<FlowerBouquet> GetFlowerBouquets { get; }

        void DeleteFlowerBouquet(FlowerBouquet flowerBouquet);
        FlowerBouquet? GetFlowerBouquetByID(int? flowerBouquetId);
        void InsertFlowerBouquet(FlowerBouquet flowerBouquet);
        ObservableCollectionListSource<FlowerBouquet> SearchFlowerBouquetInfomation(string text);
        void UpdateFlowerBouquet(FlowerBouquet flowerBouquet);
    }
}