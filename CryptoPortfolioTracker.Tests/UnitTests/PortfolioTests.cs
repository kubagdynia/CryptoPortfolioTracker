using CryptoPortfolioTracker.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPortfolioTracker.Tests.UnitTests;

[TestFixture]
[Category("UnitTests")]
public class PortfolioTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Portfolio_Should_Have_The_Correct_Value()
    {
        var config = """
         {
             "App": {
               "Portfolio": {
                 "Currencies": ["usd", "pln"],
                 "CryptoPortfolio": [
                   {
                     "coinId": "bitcoin",
                     "quantity": 1.21
                   },
                   {
                     "coinId": "ethereum",
                     "quantity": 2
                   }
                 ]
               }
             },
             "Serilog" : {
               "MinimalLevel": {
                 "Default": "Debug",
                 "Override": {
                   "Microsoft": "Information",
                   "System": "Information"
                 }
               },
               "Using": [ "Serilog.Sinks.File" ],
               "WriteTo": [
                 {
                   "Name": "File",
                   "Args": {
                     "path": "app.txt",
                     "rollingInterval": "Day",
                     "retainedFileCountLimit": 10,
                     "fileSizeLimitBytes": 52428800,
                     "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                   }
                 }
               ]
             }
         }
         """;
        
        var responseContent =
            """
            {
              "bitcoin": {
                "usd": 70049,
                "usd_market_cap": 1378339867472.376,
                "usd_24h_vol": 30300245047.661156,
                "usd_24h_change": -0.646955985425542,
                "pln": 278996,
                "pln_market_cap": 5491516918009.672,
                "pln_24h_vol": 120681330988.07709,
                "pln_24h_change": -1.0419830418344131,
                "last_updated_at": 1711716206
              },
              "ethereum": {
                "usd": 3539.49,
                "usd_market_cap": 425313818419.12524,
                "usd_24h_vol": 14023439988.61355,
                "usd_24h_change": -0.8935328135440894,
                "pln": 14097.25,
                "pln_market_cap": 1694515325596.0132,
                "pln_24h_vol": 55853257958.649414,
                "pln_24h_change": -1.2875794820891027,
                "last_updated_at": 1711716223
              }
            }
            """;
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent);
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, config);
        
        var portfolioService = serviceProvider.GetRequiredService<IPortfolioService>();

        var result = await portfolioService.GetPortfolio();

        result.Should().HaveCount(2);
        result["usd"].Should().BeApproximately(91838.27m, 0);
        result["pln"].Should().BeApproximately(365779.66m, 0);
    }

}