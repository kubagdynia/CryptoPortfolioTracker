using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPortfolioTracker.Tests.UnitTests;

[TestFixture]
[Category("UnitTests")]
public class PortfolioWithDirectConfigTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Portfolio_Should_Have_The_Correct_Value()
    {
        var settings = new AppSettings
        { 
            Portfolio = new Portfolio
            {
                Currencies = [ "usd", "pln" ],
                CryptoPortfolio =
                [
                    new Crypto
                    {
                        CoinId = "bitcoin",
                        Quantity = 1.21m
                    },

                    new Crypto
                    {
                        CoinId = "ethereum",
                        Quantity = 2
                    }
                ]
            },
            ApiKeys =
            [
                new Api
                {
                    Selected = true,
                    Name = "CoinGecko-Free",
                    Url = "https://api.coingecko.com/api/v3/"
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
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);
        
        var portfolioService = serviceProvider.GetRequiredService<IPortfolioService>();

        var result = await portfolioService.GetPortfolioByCurrencies();

        result.Should().HaveCount(2);
        result["usd"].Should().BeApproximately(91838.27m, 0);
        result["pln"].Should().BeApproximately(365779.66m, 0);
    }
    
    [Test]
    public async Task Portfolio_Should_Have_The_Correct_Value2()
    {
        var settings = new AppSettings
        { 
            Portfolio = new Portfolio
            {
                Currencies = [ "usd", "pln" ],
                CryptoPortfolio =
                [
                    new Crypto
                    {
                        CoinId = "bitcoin",
                        Quantity = 1.21m
                    },

                    new Crypto
                    {
                        CoinId = "ethereum",
                        Quantity = 2
                    }
                ]
            }
        };
        
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
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);
        
        var portfolioService = serviceProvider.GetRequiredService<IPortfolioService>();

        var result = await portfolioService.GetPortfolioByCurrencies();

        result.Should().HaveCount(2);
        result["usd"].Should().BeApproximately(91838.27m, 0);
        result["pln"].Should().BeApproximately(365779.66m, 0);
    }
    
    [Test]
    public async Task Portfolio_Should_Have_The_Correct_Grouped_Value()
    {
        var settings = new AppSettings
        { 
            Portfolio = new Portfolio
            {
                Currencies = [ "usd", "pln" ],
                CryptoPortfolio =
                [
                    new Crypto
                    {
                        CoinId = "bitcoin",
                        Quantity = 1.21m
                    },

                    new Crypto
                    {
                        CoinId = "ethereum",
                        Quantity = 2.01895m
                    }
                ]
            },
            ApiKeys =
            [
                new Api
                {
                    Selected = true,
                    Name = "CoinGecko-Free",
                    Url = "https://api.coingecko.com/api/v3/"
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
        
        var responseContentFromCoingecko =
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
        
        var httpClientFactory = TestHelper.CreateFakeHttpClientFactory(responseContentFromCoingecko);
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory, settings);
        
        var portfolioService = serviceProvider.GetRequiredService<IPortfolioService>();

        var result = await portfolioService.GetPortfolioByCoinId();

        result.FullPortfolio.Should().HaveCount(2);
        result.FullPortfolio.Should().ContainKey("bitcoin");
        result.FullPortfolio.Should().ContainKey("ethereum");

        result.FullPortfolio["bitcoin"].Should().NotBeNull();
        result.FullPortfolio["bitcoin"].Values.Should().HaveCount(2);
        result.FullPortfolio["bitcoin"].Values.Should().ContainKey("usd");
        result.FullPortfolio["bitcoin"].Values.Should().ContainKey("pln");
        result.FullPortfolio["bitcoin"].Quantity.Should().BeApproximately(1.21m, 0);
        result.FullPortfolio["bitcoin"].Values["usd"].Should().NotBeNull();
        result.FullPortfolio["bitcoin"].Values["usd"]!.Value.Should().BeApproximately(84759.29m, 0);
        result.FullPortfolio["bitcoin"].Values["pln"].Should().NotBeNull();
        result.FullPortfolio["bitcoin"].Values["pln"]!.Value.Should().BeApproximately(337585.16m, 0);
        
        result.FullPortfolio["ethereum"].Should().NotBeNull();
        result.FullPortfolio["ethereum"].Values.Should().HaveCount(2);
        result.FullPortfolio["ethereum"].Values.Should().ContainKey("usd");
        result.FullPortfolio["ethereum"].Values.Should().ContainKey("pln");
        result.FullPortfolio["ethereum"].Quantity.Should().BeApproximately(2.01895m, 0);
        result.FullPortfolio["ethereum"].Values["usd"].Should().NotBeNull();
        result.FullPortfolio["ethereum"].Values["usd"]!.Value.Should().BeApproximately(7146.0533355m, 0);
        result.FullPortfolio["ethereum"].Values["pln"].Should().NotBeNull();
        result.FullPortfolio["ethereum"].Values["pln"]!.Value.Should().BeApproximately(28461.6428875m, 0);

        var portfolioByCurrencies = result.GetPortfolioByCurrencies();
        portfolioByCurrencies.Should().HaveCount(2);
        portfolioByCurrencies["usd"].Should().BeApproximately(91905.3433355m, 0);
        portfolioByCurrencies["pln"].Should().BeApproximately(366046.8028875m, 0);
    }
}