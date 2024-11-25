using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Exceptions;
using CryptoPortfolioTracker.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CryptoPortfolioTracker.Tests.UnitTests;

[TestFixture]
[Category("UnitTests")]
public class DirectConfigurationTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void FreeApi_Should_Be_Selected_Correctly()
    {
        var settings = new AppSettings
        {
            Portfolio = new Portfolio
            {
                Currencies = [ "usd" ],
                CryptoPortfolio = []
            },
            ApiKeys =
            [
                new Api
                {
                    Selected = true,
                    Name = "CoinGecko-Free",
                    Url = "https://api.coingecko.com/api/v3/"
                }
            ]
        };
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);

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
        var settings = new AppSettings
        {
            Portfolio = new Portfolio
            {
                Currencies = [ "usd" ],
                CryptoPortfolio = []
            },
            ApiKeys =
            [
                new Api
                {
                    Selected = false,
                    Name = "CoinGecko-Free",
                    Url = "https://api.coingecko.com/api/v3/"
                },
                new Api
                {
                    Selected = true,
                    Name = "CoinGecko-DemoApi",
                    Url = "https://api.coingecko.com/api/v3/",
                    Parameters =
                    [
                        new Parameter
                        {
                            Name = "x_cg_demo_api_key",
                            Value = "demotestapikey"
                        }
                    ]
                },
                new Api
                {
                    Selected = false,
                    Name = "CoinGecko-ProApi",
                    Url = "https://pro-api.coingecko.com/api/v3/",
                    Parameters =
                    [
                        new Parameter
                        {
                            Name = "x_cg_pro_api_key",
                            Value = ""
                        }
                    ]
                }
            ]
        };
            
            
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);

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
        var settings = new AppSettings
        {
            Portfolio = new Portfolio
            {
                Currencies = [ "usd" ],
                CryptoPortfolio = []
            },
            ApiKeys =
            [
                new Api
                {
                    Selected = false,
                    Name = "CoinGecko-Free",
                    Url = "https://api.coingecko.com/api/v3/"
                },
                new Api
                {
                    Selected = false,
                    Name = "CoinGecko-DemoApi",
                    Url = "https://api.coingecko.com/api/v3/",
                    Parameters =
                    [
                        new Parameter
                        {
                            Name = "x_cg_demo_api_key",
                            Value = ""
                        }
                    ]
                },
                new Api
                {
                    Selected = true,
                    Name = "CoinGecko-ProApi",
                    Url = "https://pro-api.coingecko.com/api/v3/",
                    Parameters =
                    [
                        new Parameter
                        {
                            Name = "x_cg_pro_api_key",
                            Value = "protestapikey"
                        }
                    ]
                }
            ]
        };
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);

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
        var settings = new AppSettings
        {
            Portfolio = new Portfolio
            {
                Currencies = [ "usd" ],
                CryptoPortfolio = []
            },
            ApiKeys =
            [
                new Api
                {
                    Selected = false,
                    Name = "CoinGecko-Free",
                    Url = "https://api.coingecko.com/api/v3/"
                },
                new Api
                {
                    Selected = false,
                    Name = "CoinGecko-DemoApi",
                    Url = "https://api.coingecko.com/api/v3/",
                    Parameters =
                    [
                        new Parameter
                        {
                            Name = "x_cg_demo_api_key",
                            Value = ""
                        }
                    ]
                },
                new Api
                {
                    Selected = false,
                    Name = "CoinGecko-ProApi",
                    Url = "https://pro-api.coingecko.com/api/v3/",
                    Parameters =
                    [
                        new Parameter
                        {
                            Name = "x_cg_pro_api_key",
                            Value = "protestapikey"
                        }
                    ]
                }
            ]
        };
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);

        Assert.Throws<ConfigurationException>(() => serviceProvider.GetRequiredService<IPortfolioService>());
    }
    
    [Test]
    public void No_Api_Should_Not_Throw_An_Exception()
    {
        var settings = new AppSettings
        { 
            Portfolio = new Portfolio
            {
                Currencies = [ "usd" ],
                CryptoPortfolio = []
            }
        };
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContent: "{}");
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);
        
        // Acr & Assert
        Assert.DoesNotThrow(() => serviceProvider.GetRequiredService<IPortfolioService>(),
            "Service should be resolved without throwing an exception.");
    }
}