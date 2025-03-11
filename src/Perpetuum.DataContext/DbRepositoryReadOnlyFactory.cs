using Microsoft.EntityFrameworkCore;
using Perpetuum.DataContext.Context;

namespace Perpetuum.DataContext
{
    /// <summary>
    /// To deal with legacy static code with no DI, use this sparingly
    /// </summary>
    public static class DbRepositoryReadOnlyFactory
    {
        private static DbContextOptions<PerpetuumDbContext> _options = new DbContextOptions<PerpetuumDbContext>();

        public static void UseOptions(DbContextOptions<PerpetuumDbContext> options)
        {
            _options = options;
        }

        /// <summary>
        /// New repository created each time to ensure thread safety
        /// </summary>
        /// <returns></returns>
        public static IDbRepositoryReadOnly<T> CreateReadOnlyRepository<T>() where T : class
        {
            var context = new PerpetuumDbContext(_options);
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return new DbRepositoryReadOnly<T>(context);
        }
    }
}
