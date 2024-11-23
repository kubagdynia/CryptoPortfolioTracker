using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Timeout;
using Polly.Extensions.Http;

namespace CryptoPortfolioTracker.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static AppSettings RegisterSettings(this IServiceCollection services, IConfiguration configuration,
        string appConfigSectionName = AppSettings.AppConfigSectionName)
    {
        var appConfig = configuration.GetSection(appConfigSectionName);

        if (!appConfig.Exists())
        {
            // Create default configuration
            var defaultConfig = DefaultAppConfig.GetDefaultConfig();
            
            services.Configure<AppSettings>(opt =>
            {
                opt.Portfolio = defaultConfig.Portfolio;
                opt.ApiKeys = defaultConfig.ApiKeys;
            });
            return defaultConfig;
        }

        services.Configure<AppSettings>(appConfig);
        return appConfig.Get<AppSettings>()!;
    }

    public static void RegisterCryptoPortfolioTracker(this IServiceCollection services, IConfiguration configuration,
        string appConfigSectionName = AppSettings.AppConfigSectionName)
    {
        var appConfig = configuration.GetSection(appConfigSectionName);

        if (!appConfig.Exists())
        {
            // Create default configuration
            var defaultConfig = DefaultAppConfig.GetDefaultConfig();
            
            services.Configure<AppSettings>(opt =>
            {
                opt.Portfolio = defaultConfig.Portfolio;
                opt.ApiKeys = defaultConfig.ApiKeys;
            });
        }
        else
        {
            services.Configure<AppSettings>(appConfig);
        }
        
        services.AddTransient<ICoinGeckoClient, CoinGeckoClient>();
        services.AddTransient<IPortfolioService, PortfolioService>();
        
        AddHttpClient(services);
    }
    
    public static void RegisterCryptoPortfolioTracker(this IServiceCollection services, AppSettings appSettings)
    {
        services.Configure<AppSettings>(opt =>
        {
            opt.Portfolio = appSettings.Portfolio;
            opt.ApiKeys = appSettings.ApiKeys.Count != 0 ? appSettings.ApiKeys : DefaultAppConfig.GetDefaultApiKeys();
        });

        services.AddTransient<ICoinGeckoClient, CoinGeckoClient>();
        services.AddTransient<IPortfolioService, PortfolioService>();
        
        AddHttpClient(services);
    }
    
    private static void AddHttpClient(IServiceCollection services)
    {
        services.AddHttpClient("ClientWithoutSSLValidation")
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    // Disable SSL certificate validation
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                };
            })
            .AddPolicyHandler(GetRetryPolicy(1))
            .AddPolicyHandler(GetTimeoutPolicy(1));
    
        services.AddHttpClient("HttpClient")
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(5));
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int seconds = 5)
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(1, _ => TimeSpan.FromSeconds(seconds));

    private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int seconds = 5)
        => Policy.TimeoutAsync<HttpResponseMessage>(seconds,
            TimeoutStrategy.Optimistic, onTimeoutAsync: (_, _, _, _) => Task.CompletedTask);
}