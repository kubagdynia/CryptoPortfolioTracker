using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Exceptions;
using CryptoPortfolioTracker.Core.Services;
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
    public void FreeApi_Should_Be_Selected_Correctly()
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

        var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

        appSettings.ApiKeys.Should().HaveCount(1);

        var selectedApi = appSettings.GetSelectedApi();
        selectedApi.Name.Should().BeEquivalentTo("CoinGecko-Free");
        selectedApi.Url.Should().BeEquivalentTo("https://api.coingecko.com/api/v3/");
        selectedApi.GetParametersAsDictionary().Should().HaveCount(0);
        selectedApi.Parameters.Should().BeNull();
        selectedApi.Selected.Should().BeTrue();

        appSettings.Portfolio.Currencies.Should().NotBeNull();
    }
    
    [Test]
    public void DemoApi_Should_Be_Selected_Correctly()
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
                   "Selected": false,
                   "Name": "CoinGecko-Free",
                   "Url": "https://api.coingecko.com/api/v3/"
                 },
                 {
                   "Selected": true,
                   "Name": "CoinGecko-DemoApi",
                   "Url": "https://api.coingecko.com/api/v3/",
                   "Parameters": [
                     {
                       "Name": "x_cg_demo_api_key",
                       "Value": "demotestapikey"
                     }
                   ]
                 },
                 {
                   "Selected": false,
                   "Name": "CoinGecko-ProApi",
                   "Url": "https://pro-api.coingecko.com/api/v3/",
                   "Parameters": [
                     {
                       "Name": "x_cg_pro_api_key",
                       "Value": ""
                     }
                   ]
                 }
               ]
             }
         }
         """;
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, config);

        var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

        appSettings.ApiKeys.Should().HaveCount(3);

        var selectedApi = appSettings.GetSelectedApi();
        selectedApi.Name.Should().BeEquivalentTo("CoinGecko-DemoApi");
        selectedApi.Url.Should().BeEquivalentTo("https://api.coingecko.com/api/v3/");
        selectedApi.GetParametersAsDictionary().Should().HaveCount(1);
        selectedApi.Parameters.Should().NotBeEmpty();
        selectedApi.GetParametersAsDictionary().Should().NotBeEmpty();
        var parameters = selectedApi.GetParametersAsDictionary();
        parameters["x_cg_demo_api_key"].Should().BeEquivalentTo("demotestapikey");
        
        selectedApi.Selected.Should().BeTrue();

        appSettings.Portfolio.Currencies.Should().NotBeNull();
    }
    
    [Test]
    public void ProApi_Should_Be_Selected_Correctly()
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
                   "Selected": false,
                   "Name": "CoinGecko-Free",
                   "Url": "https://api.coingecko.com/api/v3/"
                 },
                 {
                   "Selected": false,
                   "Name": "CoinGecko-DemoApi",
                   "Url": "https://api.coingecko.com/api/v3/",
                   "Parameters": [
                     {
                       "Name": "x_cg_demo_api_key",
                       "Value": ""
                     }
                   ]
                 },
                 {
                   "Selected": true,
                   "Name": "CoinGecko-ProApi",
                   "Url": "https://pro-api.coingecko.com/api/v3/",
                   "Parameters": [
                     {
                       "Name": "x_cg_pro_api_key",
                       "Value": "protestapikey"
                     }
                   ]
                 }
               ]
             }
         }
         """;
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, config);

        var appSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;

        appSettings.ApiKeys.Should().HaveCount(3);

        var selectedApi = appSettings.GetSelectedApi();
        selectedApi.Name.Should().BeEquivalentTo("CoinGecko-ProApi");
        selectedApi.Url.Should().BeEquivalentTo("https://pro-api.coingecko.com/api/v3/");
        selectedApi.GetParametersAsDictionary().Should().HaveCount(1);
        selectedApi.Parameters.Should().NotBeEmpty();
        selectedApi.GetParametersAsDictionary().Should().NotBeEmpty();
        var parameters = selectedApi.GetParametersAsDictionary();
        parameters["x_cg_pro_api_key"].Should().BeEquivalentTo("protestapikey");
        
        selectedApi.Selected.Should().BeTrue();

        appSettings.Portfolio.Currencies.Should().NotBeNull();
    }
    
    [Test]
    public void Not_Selected_Api_Should_Throw_An_Exception()
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
                   "Selected": false,
                   "Name": "CoinGecko-Free",
                   "Url": "https://api.coingecko.com/api/v3/"
                 },
                 {
                   "Selected": false,
                   "Name": "CoinGecko-DemoApi",
                   "Url": "https://api.coingecko.com/api/v3/",
                   "Parameters": [
                     {
                       "Name": "x_cg_demo_api_key",
                       "Value": ""
                     }
                   ]
                 },
                 {
                   "Selected": false,
                   "Name": "CoinGecko-ProApi",
                   "Url": "https://pro-api.coingecko.com/api/v3/",
                   "Parameters": [
                     {
                       "Name": "x_cg_pro_api_key",
                       "Value": "protestapikey"
                     }
                   ]
                 }
               ]
             }
         }
         """;
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, config);

        Assert.Throws<ConfigurationException>(() => serviceProvider.GetRequiredService<IPortfolioService>());
    }
    
    [Test]
    public void No_Api_Should_Throw_An_Exception()
    {
      var config = """
                   {
                       "App": {
                         "Portfolio": {
                           "Currencies": ["usd"],
                           "CryptoPortfolio": [
                           
                           ]
                         }
                       }
                   }
                   """;
        
      var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
      var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, config);

      Assert.Throws<ConfigurationException>(() => serviceProvider.GetRequiredService<IPortfolioService>());
    }
}