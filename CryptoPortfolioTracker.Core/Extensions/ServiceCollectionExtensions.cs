using CryptoPortfolioTracker.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPortfolioTracker.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterCore(this IServiceCollection services, IConfiguration configuration, string appConfigSectionName = "App")
    {
        IConfigurationSection appConfig = configuration.GetSection(appConfigSectionName);

        if (!appConfig.Exists())
        {
            // Create default configuration
            appConfig = DefaultAppConfig.GetDefaultAppConfig(appConfigSectionName);
        }

        services.Configure<AppSettings>(appConfig);
    }
}