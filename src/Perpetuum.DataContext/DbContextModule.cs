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
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .UseLoggerFactory(loggerFactory)
                .Options;

            DbRepositoryReadOnlyFactory.UseOptions(options);
            return new PerpetuumDbContext(options);
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Generic repository
            builder.RegisterGeneric(typeof(DbRepository<>))
                .As(typeof(IDbRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DbRepositoryReadOnly<>))
                .As(typeof(IDbRepositoryReadOnly<>))
                .InstancePerLifetimeScope();
        }
    }
}
