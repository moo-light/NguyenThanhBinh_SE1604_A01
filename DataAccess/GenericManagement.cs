using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DataAccess
{
    public abstract class GenericManagement<TEntity> : IGenericManagement<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> _dbSet;
        protected static FuFlowerBouquetManagementContext _context;

        public DbSet<TEntity> DbSet { get => _dbSet; set => _dbSet = value; }

        public GenericManagement(FuFlowerBouquetManagementContext context)
        {
            _dbSet = context.Set<TEntity>();
            _context = context;
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual void AddNew(TEntity? entity)
        {
            if (entity == null) return;

            _dbSet.Entry(entity).State = EntityState.Added;
            _context.SaveChanges();
        }
        public void AddNewTransaction(TEntity? entity)
        {
            if (entity == null) return;

            _dbSet.Entry(entity).State = EntityState.Added;
        }

        public virtual void Update(TEntity? entity)
        {
            if (entity == null) return;

            _dbSet.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public virtual void Remove(TEntity? entity)
        {
            if (entity == null) return;

            //_dbSet.Entry(entity).State = EntityState.Deleted;
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public abstract TEntity? GetByID(int? id);

        public virtual void Detach(TEntity? entity)
        {
            _dbSet.Entry(entity).State = EntityState.Detached;
        }
    }
}