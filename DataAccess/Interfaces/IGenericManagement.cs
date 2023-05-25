using BusinessObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Interfaces
{
    public interface IGenericManagement<TEntity> where TEntity : BaseEntity
    {
        void AddNew(TEntity entity);
        void Remove(TEntity entity);
        IQueryable<TEntity> GetAll();
        TEntity? GetByID(int? id);
        void Update(TEntity? entity);
        void Detach(TEntity? customer);
        void AddNewTransaction(TEntity? entity);
    }
}