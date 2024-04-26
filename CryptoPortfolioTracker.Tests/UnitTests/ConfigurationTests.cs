using CryptoPortfolioTracker.Core.Configuration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CryptoPortfolioTracker.Tests.UnitTests;

[TestFixture]
[Category("UnitTests")]
public class ConfigurationTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void FreeApi_Should_Be_Selected_Correclty()
    {
        var config = """
         {
             "App": {
               "Portfolio": {
                 "Currencies": ["usd"],
                 "CryptoPortfolio": [
                 
                 ]
               },
               "ApiKeys": [
                 {
                   "Selected": true,
                   "Name": "CoinGecko-Free",
                   "Url": "https://api.coingecko.com/api/v3/"
                 }
               ]
             }
         }
         """;
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, config);

        AppSettings appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

        appSettings.ApiKeys.Should().HaveCount(1);

        Api selectedApi = appSettings.GetSelectedApi();
        selectedApi.Name.Should().BeEquivalentTo("CoinGecko-Free");
        selectedApi.Url.Should().BeEquivalentTo("https://api.coingecko.com/api/v3/");
        selectedApi.GetParametersAsDictionary().Should().HaveCount(0);
        selectedApi.Parameters.Should().BeNull();
        selectedApi.Selected.Should().BeTrue();

        appSettings.Portfolio.Currencies.Should().NotBeNull();
    }
}