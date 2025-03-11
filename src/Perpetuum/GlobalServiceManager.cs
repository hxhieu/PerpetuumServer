using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Perpetuum.Configuration;
using Perpetuum.DataContext;
using Serilog;
using System;
using System.Threading;

namespace Perpetuum
{
    public static class GlobalServiceManager
    {
        private static readonly Lazy<IConfiguration> _configuration = new(() =>
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", false, true)
                .AddJsonFile("appSettings.Development.json", true, true)
                .AddEnvironmentVariables("PERPETUUM_SERVER_")
            ;
            return builder.Build();
        });

        private static readonly Lazy<ILoggerFactory> _loggerFactory = new(() =>
        {
            // Serilog logger instance from configuration
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            return Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(logger);
            });
        });

        private static readonly Lazy<DebugSettings> _debugSettings = new (()=>
        {
            var settings = new DebugSettings();
            Configuration.GetSection("Debugging").Bind(settings);
            return settings;
        });

        public static bool ZonesLoaded { get; set; }

        public static IConfiguration Configuration => _configuration.Value;

        public static ILoggerFactory LoggerFactory => _loggerFactory.Value;

        public static DebugSettings DebugSettings => _debugSettings.Value;

        public static IDbRepositoryReadOnly<T> CreateReadOnlyRepository<T>() where T : class => DbRepositoryReadOnlyFactory.CreateReadOnlyRepository<T>();


        /// <summary>
        /// Invoke the provided action, only after all zones loaded
        /// </summary>
        /// <param name="invoke"></param>
        /// <param name="waitMs"></param>
        public static void PostZonesLoadedAction(Action invoke, int waitMs = 500)
        {
            // Create an off thread, to check in background
            var offThread = new Thread(() =>
            {
                while (!ZonesLoaded)
                {
                    Thread.Sleep(waitMs);
                }
                invoke();
            });
            offThread.Start();
        }
    }
}
