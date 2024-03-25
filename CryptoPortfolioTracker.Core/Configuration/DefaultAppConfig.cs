using Microsoft.Extensions.Configuration;

namespace CryptoPortfolioTracker.Core.Configuration;

public abstract class DefaultAppConfig
{
    public static IConfigurationSection GetDefaultAppConfig(string appConfigSectionName)
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {$"{appConfigSectionName}:Name", "Default config"}
        };

        var defaultConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();
        
        var configurationSection = defaultConfiguration.GetSection(appConfigSectionName);
        return configurationSection;
    }
}