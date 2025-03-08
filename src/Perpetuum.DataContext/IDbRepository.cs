using Microsoft.EntityFrameworkCore;
using Perpetuum.DataContext.Context;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace Perpetuum.DataContext
{
    public interface IDbRepository<T> where T : class
    {
        T? GetOne(Expression<Func<T, bool>> predicate, bool fromCache = false);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, bool fromCache = false);
        void Add(T entity);
        void Update(T entity, params Expression<Func<T, object>>[] updatedProperties);
        void Delete(T entity);
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

        // Get one entity matching the predicate
        public T? GetOne(Expression<Func<T, bool>> predicate, bool fromCache)
        {
            return fromCache
                ? _dbSet.DeferredFirstOrDefault(predicate).FromCache()
                : _dbSet.FirstOrDefault(predicate);
        }

        // Get many entities matching the predicate
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate, bool fromCache)
        {
            return fromCache
                ? _dbSet.Where(predicate).FromCache().ToList()
                : _dbSet.Where(predicate).ToList();
        }

        // Add a new entity to the database
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        // Update an entity. If no specific properties are provided, update all columns.
        public void Update(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            _dbSet.Attach(entity); // Attach the entity to the context (if not already tracked)

            if (updatedProperties == null || updatedProperties.Length == 0)
            {
                // If no specific properties are provided, mark the entire entity as modified
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                // If specific properties are provided, only update those fields
                foreach (var property in updatedProperties)
                {
                    _context.Entry(entity).Property(property).IsModified = true;
                }
            }
        }

        // Delete an entity
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Save changes to the database
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }

}
