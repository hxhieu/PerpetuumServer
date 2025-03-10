using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Perpetuum.DataContext.Context;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace Perpetuum.DataContext
{
    public interface IDbRepository<T> where T : class
    {
        T? GetOne(Expression<Func<T, bool>> predicate, TimeSpan? cacheTime = null);
        List<T> GetMany(Expression<Func<T, bool>> predicate, TimeSpan? cacheTime = null);
        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity, params Expression<Func<T, object>>[] updatedProperties);
        int UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory);
        void Delete(T entity);
        int DeleteBatch(Expression<Func<T, bool>> predicate);
        int ExecuteNonQuerySql(string sql, params object[] parameters);
        int SaveChanges();
    }

    public class DbRepository<T> : IDbRepository<T> where T : class
    {
        protected readonly PerpetuumDbContext _context;
        private readonly DbSet<T> _dbSet;

        public DbRepository(PerpetuumDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // Save changes to the database
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public int ExecuteNonQuerySql(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlRaw(sql, parameters);
        }

        // Get one entity matching the predicate
        public T? GetOne(Expression<Func<T, bool>> predicate, TimeSpan? cacheTime)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = cacheTime?? TimeSpan.FromSeconds(10)
            };
            return _dbSet.AsNoTracking().DeferredFirstOrDefault(predicate).FromCache(cacheOptions);
        }

        // Get many entities matching the predicate
        public List<T> GetMany(Expression<Func<T, bool>> predicate, TimeSpan? cacheTime)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = cacheTime?? TimeSpan.FromSeconds(10)
            };
            return _dbSet.AsNoTracking().Where(predicate).FromCache(cacheOptions).ToList();
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate);
        }

        // Add a new entity to the database
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        // Update an entity. If no specific properties are provided, update all columns.
        public void Update(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            var entry = _context.Entry(entity);

            if (updatedProperties == null || updatedProperties.Length == 0)
            {
                // If no specific properties are provided, mark the entire entity as modified
                entry.State = EntityState.Modified;
            }
            else
            {
                // If specific properties are provided, only update those fields
                foreach (var property in updatedProperties)
                {
                    entry.Property(property).IsModified = true;
                }
            }
        }

        // Delete an entity
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public int UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory)
        {
            return _dbSet.Where(predicate).Update(updateFactory);
        }

        public int DeleteBatch(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Delete();
        }
    }

}
