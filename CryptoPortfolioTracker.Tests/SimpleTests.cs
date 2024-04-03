using System.Net;
using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Extensions;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPortfolioTracker.Tests;

public class SimpleTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetSimplePrice_For_Bitcoin_And_Ethereum_Should_Return_Correct_Data()
    {
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

        var httpClientFactory = CreateFakeHttpClientFactory(responseContent);
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory);

        var client = serviceProvider.GetRequiredService<ICoinGeckoClient>();
        var result = await client.GetSimplePrice(["bitcoin", "ethereum"], ["usd", "pln"],
            true, true, true, true);

        result.Should().HaveCount(2);
        
        // bitcoin
        result[0].Id.Should().BeEquivalentTo("bitcoin");
        result[0].LastUpdatedAt.Should().BeApproximately(1711716206m, 0);
        result[0].Currencies.Should().HaveCount(2);
        var currencyBtcUsd = result[0].Currencies.Currency("usd");
        currencyBtcUsd.Should().NotBeNull();
        currencyBtcUsd?.Name.Should().BeEquivalentTo("usd");
        currencyBtcUsd?.Price.Should().BeApproximately(70049m, 0);
        currencyBtcUsd?.MarketCap.Should().BeApproximately(1378339867472.376m, 0);
        currencyBtcUsd?.Vol24H.Should().BeApproximately(30300245047.661156m, 0);
        currencyBtcUsd?.Change24H.Should().BeApproximately(-0.646955985425542m, 0);
        var currencyBtcPln = result[0].Currencies.Currency("pln");
        currencyBtcPln.Should().NotBeNull();
        currencyBtcPln?.Name.Should().BeEquivalentTo("pln");
        currencyBtcPln?.Price.Should().BeApproximately(278996m, 0);
        currencyBtcPln?.MarketCap.Should().BeApproximately(5491516918009.672m, 0);
        currencyBtcPln?.Vol24H.Should().BeApproximately(120681330988.07709m, 0);
        currencyBtcPln?.Change24H.Should().BeApproximately(-1.0419830418344131m, 0);
        
        
        // ethereum
        result[1].Id.Should().BeEquivalentTo("ethereum");
        result[1].LastUpdatedAt.Should().BeApproximately(1711716223m, 0);
        result[1].Currencies.Should().HaveCount(2);
        var currencyEthUsd = result[1].Currencies.Currency("usd");
        currencyEthUsd.Should().NotBeNull();
        currencyEthUsd?.Name.Should().BeEquivalentTo("usd");
        currencyEthUsd?.Price.Should().BeApproximately(3539.49m, 0);
        currencyEthUsd?.MarketCap.Should().BeApproximately(425313818419.12524m, 0);
        currencyEthUsd?.Vol24H.Should().BeApproximately(14023439988.61355m, 0);
        currencyEthUsd?.Change24H.Should().BeApproximately(-0.8935328135440894m, 0);
        var currencyEthPln = result[1].Currencies.Currency("pln");
        currencyEthPln.Should().NotBeNull();
        currencyEthPln?.Name.Should().BeEquivalentTo("pln");
        currencyEthPln?.Price.Should().BeApproximately(14097.25m, 0);
        currencyEthPln?.MarketCap.Should().BeApproximately(1694515325596.0132m, 0);
        currencyEthPln?.Vol24H.Should().BeApproximately(55853257958.649414m, 0);
        currencyEthPln?.Change24H.Should().BeApproximately(-1.2875794820891027m, 0);
    }
    
    [Test]
    public async Task GetSimplePrice_For_Bitcoin_Should_Return_Correct_Data()
    {
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
              }
            }
            """;

        var httpClientFactory = CreateFakeHttpClientFactory(responseContent);
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory);

        var client = serviceProvider.GetRequiredService<ICoinGeckoClient>();
        var result = await client.GetSimplePrice("bitcoin", "usd",
            true, true, true, true);

        result.Should().NotBeNull();
        result.Id.Should().BeEquivalentTo("bitcoin");
        result.LastUpdatedAt.Should().BeApproximately(1711716206m, 0);
        result.Currencies.First().Name.Should().BeEquivalentTo("usd");
        result.Currencies.First().Price.Should().BeApproximately(70049m, 0);
        result.Currencies.First().MarketCap.Should().BeApproximately(1378339867472.376m, 0);
        result.Currencies.First().Vol24H.Should().BeApproximately(30300245047.661156m, 0);
        result.Currencies.First().Change24H.Should().BeApproximately(-0.646955985425542m, 0);
    }
    
    [Test]
    public async Task GetSupportedVsCurrencies_Should_Return_Correct_Data()
    {
        var responseContent =
            """
            [
              "btc",
              "eth",
              "ltc",
              "bch",
              "bnb",
              "eos",
              "xrp",
              "xlm",
              "link",
              "dot",
              "yfi",
              "usd",
              "aed",
              "ars",
              "aud",
              "bdt",
              "bhd",
              "bmd",
              "brl",
              "cad",
              "chf",
              "clp",
              "cny",
              "czk",
              "dkk",
              "eur",
              "gbp",
              "gel",
              "hkd",
              "huf",
              "idr",
              "ils",
              "inr",
              "jpy",
              "krw",
              "kwd",
              "lkr",
              "mmk",
              "mxn",
              "myr",
              "ngn",
              "nok",
              "nzd",
              "php",
              "pkr",
              "pln",
              "rub",
              "sar",
              "sek",
              "sgd",
              "thb",
              "try",
              "twd",
              "uah",
              "vef",
              "vnd",
              "zar",
              "xdr",
              "xag",
              "xau",
              "bits",
              "sats"
            ]
            """;

        var httpClientFactory = CreateFakeHttpClientFactory(responseContent);
        var serviceProvider = TestHelper.CreateServiceProvider(httpClientFactory);

        var client = serviceProvider.GetRequiredService<ICoinGeckoClient>();
        var supportedCurrencies = await client.GetSupportedVsCurrencies();

        supportedCurrencies.Should().NotBeNull();
        supportedCurrencies.Should().HaveCount(62);
    }

    private IHttpClientFactory CreateFakeHttpClientFactory(string content)
    {
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        var mockHttpResponse = new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(content)
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