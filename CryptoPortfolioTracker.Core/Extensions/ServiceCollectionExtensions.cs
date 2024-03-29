using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Timeout;
using Polly.Extensions.Http;

namespace CryptoPortfolioTracker.Core.Extensions;

public static class ServiceCollectionExtensions
{
    private const string AppConfigSectionName = "App";

    public static void RegisterCore(this IServiceCollection services, IConfiguration configuration, string appConfigSectionName = AppConfigSectionName)
    {
        IConfigurationSection appConfig = configuration.GetSection(appConfigSectionName);

        if (!appConfig.Exists())
        {
            // Create default configuration
            appConfig = DefaultAppConfig.GetDefaultAppConfig(appConfigSectionName);
        }

        services.Configure<AppSettings>(appConfig);

        services.AddTransient<ICoinGeckoClient, CoinGeckoClient>();
        
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