using Serilog;
using System;
using Serilogger = Serilog.ILogger;

namespace Perpetuum.Log.Loggers
{
    public class ColoredConsoleLogger : ConsoleLogger<LogEvent>
    {
        private static readonly Lazy<Serilogger> _logger = new(() => new LoggerConfiguration()
            .ReadFrom.Configuration(ConfigurationManager.Load())
            .CreateLogger());

        public ColoredConsoleLogger(ILogEventFormatter<LogEvent, string> formatter) : base(formatter) { }

        public override void Log(LogEvent logEvent)
        {
            switch (logEvent.LogType)
            {
                case LogType.Warning:
                    _logger.Value.Warning(logEvent.Message);
                    break;
                case LogType.Error:
                    _logger.Value.Error(logEvent.ThrownException, logEvent.Message);
                    break;
                case LogType.None:
                    _logger.Value.Debug(logEvent.Message);
                    break;
                default:
                    _logger.Value.Information(logEvent.Message);
                    break;
            }
        }
    }
}