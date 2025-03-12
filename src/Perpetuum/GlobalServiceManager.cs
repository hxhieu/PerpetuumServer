using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Perpetuum.Configuration;
using Perpetuum.Data;
using Perpetuum.DataContext;
using Perpetuum.Log;
using Serilog;
using System;
using System.Diagnostics;
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
        /// Log the caller of this method
        /// </summary>
        /// <param name="maxDepth"></param>
        public static void LogCaller(int maxDepth = 5)
        {
            var stackTrace = new StackTrace(1); // Ignore itself
            var frames = stackTrace.GetFrames();
            var stackString = "";
            var depth = 0;
            foreach (var frame in frames)
            {
                var method = frame.GetMethod();

                if (method.DeclaringType?.FullName == typeof(DbQuery).FullName)
                    continue;

                depth++;

                if (method.DeclaringType?.Namespace.StartsWith("Perpetuum") ?? false)
                {
                    stackString += $"[{method.DeclaringType.FullName}] -> {method.Name} (";
                    method.GetParameters().ForEach(x =>
                    {
                        stackString += $"{x.ParameterType.Name} {x.Name}, ";
                    });
                    stackString = stackString.Trim(' ', ',');
                    stackString += "), ";
                }
                if (depth >= maxDepth)
                    break;
            }

            stackString = stackString.Trim(' ', ',');
            Logger.Error($"!>>> RAW SQL CALL: {stackString}");
        }

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
