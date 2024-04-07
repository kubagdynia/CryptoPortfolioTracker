using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;

namespace CryptoPortfolioTracker.Tests;

public static class TestHelper
{
    public static ServiceProvider CreateServiceProvider(IHttpClientFactory httpClientFactory)
    {
        var services = new ServiceCollection();
        
        // Create an empty configuration 
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();
        var appConfigSectionName = "App";
        var appConfig = configuration.GetSection(appConfigSectionName);

        if (!appConfig.Exists())
        {
            // Create a default configuration
            appConfig = DefaultAppConfig.GetDefaultAppConfig(appConfigSectionName);
        }
        
        services.Configure<AppSettings>(appConfig);
        
        var loggerMock = new Mock<ILogger<CoinGeckoClient>>();

        services.AddTransient<ICoinGeckoClient, CoinGeckoClient>(x =>
            new CoinGeckoClient(httpClientFactory, loggerMock.Object));
        
        return services.BuildServiceProvider();
    }

    public static ServiceProvider CreateNoMoqServiceProvider(string config)
    {
        var services = new ServiceCollection();
        
        using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config));
        
        // Create an empty configuration 
        var configuration = new ConfigurationBuilder().AddJsonStream(mem).Build();
        
        // defining Serilog configs
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        
        // configure logging
        services.AddLogging(builder =>
        { 
            builder.AddSerilog();
        });
        
        services.RegisterCore(configuration);
        
        return services.BuildServiceProvider();
    }

}