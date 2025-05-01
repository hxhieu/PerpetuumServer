using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Perpetuum.DataContext.Context;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace Perpetuum.DataContext
{
    public interface IDbRepositoryReadOnly<T> where T : class
    {
        T? GetOne(Expression<Func<T, bool>> predicate, TimeSpan? cacheTime = null);
        List<T> GetMany(Expression<Func<T, bool>>? predicate = null, TimeSpan? cacheTime = null);
        IQueryable<T> GetManyQuery(Expression<Func<T, bool>>? predicate = null);
    }

    public class DbRepositoryReadOnly<T> : IDbRepositoryReadOnly<T> where T : class
    {
        protected readonly PerpetuumDbContext Context;
        protected readonly DbSet<T> DbSet;

        public DbRepositoryReadOnly(PerpetuumDbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        // Get one entity matching the predicate
        public T? GetOne(Expression<Func<T, bool>> predicate, TimeSpan? cacheTime)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = cacheTime?? TimeSpan.FromSeconds(10)
            };
            return DbSet.AsNoTracking().DeferredFirstOrDefault(predicate).FromCache(cacheOptions);
        }

        // Get many entities matching the predicate
        public List<T> GetMany(Expression<Func<T, bool>>? predicate = null, TimeSpan? cacheTime = null)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = cacheTime?? TimeSpan.FromSeconds(10)
            };
            return DbSet.AsNoTracking().Where(predicate ?? (_ => true)).FromCache(cacheOptions).ToList();
        }

        public IQueryable<T> GetManyQuery(Expression<Func<T, bool>>? predicate = null)
        {
            return DbSet.AsNoTracking().Where(predicate ?? (_ => true));
        }
    }

}
