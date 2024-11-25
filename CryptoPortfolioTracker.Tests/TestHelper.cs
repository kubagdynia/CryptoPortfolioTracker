using System.Net;
using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Extensions;
using CryptoPortfolioTracker.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Serilog;

namespace CryptoPortfolioTracker.Tests;

public static class TestHelper
{
    public static ServiceProvider CreateServiceProvider(IHttpClientFactory httpClientFactory, string? config = null)
    {
        var services = new ServiceCollection();

        IConfigurationRoot? configuration;
        if (config is null)
        {
            // Create an empty configuration 
            configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();
        }
        else
        {
            using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config));
            configuration = new ConfigurationBuilder().AddJsonStream(mem).Build();
        }

        var appConfig = services.RegisterSettings(configuration);
        var loggerMock = new Mock<ILogger<ICoinGeckoClient>>();
        services.RegisterCoinGeckoClient(httpClientFactory, loggerMock.Object, appConfig);
        
        services.AddTransient<IPortfolioService, PortfolioService>();
        
        return services.BuildServiceProvider();
    }
    
    public static ServiceProvider CreateServiceProvider(IHttpClientFactory httpClientFactory, AppSettings settings)
    {
        var services = new ServiceCollection();

        var appConfig = services.RegisterSettings(settings);
        var loggerMock = new Mock<ILogger<ICoinGeckoClient>>();
        services.RegisterCoinGeckoClient(httpClientFactory, loggerMock.Object, appConfig);
        
        services.AddTransient<IPortfolioService, PortfolioService>();
        
        return services.BuildServiceProvider();
    }

    public static ServiceProvider CreateNoMoqServiceProvider(string config)
    {
        var services = new ServiceCollection();
        
        using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config));
        
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
        
        services.RegisterCryptoPortfolioTracker(configuration);
        
        return services.BuildServiceProvider();
    }
    
    public static IHttpClientFactory CreateFakeHttpClientFactory(string responseContent)
    {
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        var mockHttpResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseContent)
        };
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockHttpResponse);
        
        var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
        mockHttpClientFactory.Setup(x => x.CreateClient("ClientWithoutSSLValidation")).Returns(mockHttpClient);
        
        return mockHttpClientFactory.Object;
    }
}