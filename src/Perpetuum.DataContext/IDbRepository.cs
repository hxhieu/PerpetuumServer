using Microsoft.EntityFrameworkCore;
using Perpetuum.DataContext.Context;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace Perpetuum.DataContext
{
    public interface IDbRepository<T> : IDbRepositoryReadOnly<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        int UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory);
        void Delete(T entity);
        int DeleteBatch(Expression<Func<T, bool>> predicate);
        int ExecuteNonQuerySql(string sql, params object[] parameters);
        int SaveChanges();
    }

    public class DbRepository<T>(PerpetuumDbContext context) : DbRepositoryReadOnly<T>(context), IDbRepository<T> where T : class
    {
        // Save changes to the database
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public int ExecuteNonQuerySql(string sql, params object[] parameters)
        {
            return Context.Database.ExecuteSqlRaw(sql, parameters);
        }

        // Add a new entity to the database
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        // Update an entity
        public void Update(T entity)
        {
            DbSet.Update(entity);
        }

        // Delete an entity
        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public int UpdateBatch(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updateFactory)
        {
            return DbSet.Where(predicate).Update(updateFactory);
        }

        public int DeleteBatch(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).Delete();
        }
    }

}
