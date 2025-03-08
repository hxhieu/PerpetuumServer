using Microsoft.Extensions.Configuration;

namespace Perpetuum
{
    public static class ConfigurationManager
    {
        public static IConfiguration Load()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", false, true)
                .AddJsonFile("appSettings.Development.json", true, true)
                .AddEnvironmentVariables("PERPETUUM_SERVER_")
            ;
            return builder.Build();
        }
    }
}
