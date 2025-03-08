using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Perpetuum.DataContext.Context;
using Z.EntityFramework.Plus;

namespace Perpetuum.DataContext
{
    public class DbContextModule : Module
    {
        public static PerpetuumDbContext CreateDbContext(string connectionString, ILoggerFactory loggerFactory)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PerpetuumDbContext>();
            optionsBuilder
                .UseSqlServer(connectionString)
                .UseLoggerFactory(loggerFactory);

            return new PerpetuumDbContext(optionsBuilder.Options);
        }

        protected override void Load(ContainerBuilder builder)
        {
            // EF+ caching policy
            var options = new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(60)};
            QueryCacheManager.DefaultMemoryCacheEntryOptions = options;

            // Generic repository
            builder.RegisterGeneric(typeof(DbRepository<>))
                .As(typeof(IDbRepository<>))
                .InstancePerLifetimeScope();
        }
    }
}
