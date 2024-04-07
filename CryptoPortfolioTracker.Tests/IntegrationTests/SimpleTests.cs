using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Clients.Models;
using CryptoPortfolioTracker.Core.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPortfolioTracker.Tests.IntegrationTests;

[TestFixture]
[Category("IntegrationTests")]
public class SimpleTests
{
    private ServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void Init()
    {
        var config = """
             {
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
        
        // Create a real service provider, not a mocked one.
        _serviceProvider = TestHelper.CreateNoMoqServiceProvider(config);
    }
    
    [OneTimeTearDown]
    public void Cleanup()
    {
        _serviceProvider.Dispose();
    }

    [Test]
    public async Task GetSimplePrice_For_Bitcoin_And_Ethereum_Should_Return_Correct_Data()
    {
        ICoinGeckoClient client = _serviceProvider.GetRequiredService<ICoinGeckoClient>();
        IList<PriceId> result = await client.GetSimplePrice(["bitcoin", "ethereum"], ["usd", "pln"],
            true, true, true, true);

        result.Should().HaveCount(2);
        
        // bitcoin
        result[0].Id.Should().BeEquivalentTo("bitcoin");
        result[0].Currencies.Should().HaveCount(2);
        var currencyBtcUsd = result[0].Currencies.Currency("usd");
        currencyBtcUsd.Should().NotBeNull();
        currencyBtcUsd?.Name.Should().BeEquivalentTo("usd");
        var currencyBtcPln = result[0].Currencies.Currency("pln");
        currencyBtcPln.Should().NotBeNull();
        currencyBtcPln?.Name.Should().BeEquivalentTo("pln");
        
        // ethereum
        result[1].Id.Should().BeEquivalentTo("ethereum");
        result[1].Currencies.Should().HaveCount(2);
        var currencyEthUsd = result[1].Currencies.Currency("usd");
        currencyEthUsd.Should().NotBeNull();
        currencyEthUsd?.Name.Should().BeEquivalentTo("usd");
        var currencyEthPln = result[1].Currencies.Currency("pln");
        currencyEthPln.Should().NotBeNull();
        currencyEthPln?.Name.Should().BeEquivalentTo("pln");
    }

    [Test]
    public async Task GetSimplePrice_For_Bitcoin_Should_Return_Correct_Data()
    {
        ICoinGeckoClient client = _serviceProvider.GetRequiredService<ICoinGeckoClient>();
        PriceId result = await client.GetSimplePrice("bitcoin", "usd",
            true, true, true, true);

        result.Should().NotBeNull();
        result.Id.Should().BeEquivalentTo("bitcoin");
        result.Currencies.First().Name.Should().BeEquivalentTo("usd");
    }

    [Test]
    public async Task GetTokenPrice_For_Chainlink_And_Sandbox_On_Ethereum_Should_Return_Correct_Data()
    {
        ICoinGeckoClient client = _serviceProvider.GetRequiredService<ICoinGeckoClient>();
        IList<PriceContract> result = await client.GetTokenPrice("ethereum",
            ["0x514910771af9ca656af840dff83e8264ecf986ca"],
            ["usd", "pln"],
            true, true, true, true);
        
        result.Should().HaveCount(1);
         
        // Chainlink 
        result[0].Contract.Should().BeEquivalentTo("0x514910771af9ca656af840dff83e8264ecf986ca");
        result[0].Currencies.Should().HaveCount(2);
        var currencyBtcUsd = result[0].Currencies.Currency("usd");
        currencyBtcUsd.Should().NotBeNull();
        currencyBtcUsd?.Name.Should().BeEquivalentTo("usd");
        var currencyBtcPln = result[0].Currencies.Currency("pln");
        currencyBtcPln.Should().NotBeNull();
        currencyBtcPln?.Name.Should().BeEquivalentTo("pln");
    }
    
    [Test]
    public async Task GetSupportedVsCurrencies_Should_Return_Correct_Data()
    {
        var client = _serviceProvider.GetRequiredService<ICoinGeckoClient>();
        var supportedCurrencies = await client.GetSupportedVsCurrencies();

        supportedCurrencies.Should().NotBeNull();
        
        supportedCurrencies.Should().Contain("usd");
        supportedCurrencies.Should().Contain("btc");
        supportedCurrencies.Should().Contain("eth");
        supportedCurrencies.Should().Contain("eur");
    }

}