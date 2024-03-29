using System.Net;
using CryptoPortfolioTracker.Core.Clients;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPortfolioTracker.Tests;

public class Tests
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
        result[0].Id.Should().BeEquivalentTo("bitcoin");
        result[1].Id.Should().BeEquivalentTo("ethereum");
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