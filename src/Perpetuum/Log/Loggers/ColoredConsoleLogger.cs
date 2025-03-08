using Microsoft.Extensions.Logging;

namespace Perpetuum.Log.Loggers
{
    public class ColoredConsoleLogger(ILogEventFormatter<LogEvent, string> formatter) : ConsoleLogger<LogEvent>(formatter)
    {
        public override void Log(LogEvent logEvent)
        {
            var logger = GlobalServiceManager.LoggerFactory.CreateLogger(nameof(ColoredConsoleLogger));
            switch (logEvent.LogType)
            {
                case LogType.Warning:
                    logger.LogWarning("{Message}", logEvent.Message);
                    break;
                case LogType.Error:
                    logger.LogError(logEvent.ThrownException, "{Message}", logEvent.Message);
                    break;
                case LogType.None:
                    logger.LogDebug("{Message}", logEvent.Message);
                    break;
                default:
                    logger.LogInformation("{Message}", logEvent.Message);
                    break;
            }
        }
    }
}