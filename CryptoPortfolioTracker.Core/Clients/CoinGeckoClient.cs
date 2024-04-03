using CryptoPortfolioTracker.Core.Clients.Models;
using Microsoft.Extensions.Logging;

namespace CryptoPortfolioTracker.Core.Clients;

public class CoinGeckoClient(IHttpClientFactory clientFactory, ILogger<CoinGeckoClient> logger)
    : BaseClient(clientFactory, logger), ICoinGeckoClient
{
    private static readonly Uri ApiEndpoint = new("https://api.coingecko.com/api/v3/");
    private static readonly Uri ProApiEndpoint = new("https://pro-api.coingecko.com/api/v3/");

    protected override Uri GetApiEndpoint => ApiEndpoint;
    protected override Uri GetProApiEndpoint => ProApiEndpoint;

    public async Task<Ping?> GetPingAsync(CancellationToken ct = default)
    {
        var requestUri = new Uri(ApiEndpoint, "ping");
        return await GetAsync<Ping>(requestUri, ct);
    }

    public async Task<Price> GetSimplePrice(string id, string currency = "usd",
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default)
    {
        return (await GetSimplePrice([id], [currency], includeMarketCap, include24HrVol,
            include24HrChange, includeLastUpdatedAt, ct)).First();
    }

    public async Task<IList<Price>> GetSimplePrice(string[] ids, string[] currencies,
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default)
    {
        var requestUri = CreateUrl("simple/price", new Dictionary<string, object>
        {
            { "ids", string.Join(",", ids) },
            { "vs_currencies", string.Join(",", currencies) },
            { "include_market_cap", includeMarketCap },
            { "include_24hr_vol", include24HrVol },
            { "include_24hr_change", include24HrChange },
            { "include_last_updated_at", includeLastUpdatedAt }
        });

        var data = await GetAsync<BaseDictionary>(requestUri, ct);

        if (data is null)
        {
            return new List<Price>();
        }

        var resultList = new List<Price>();

        foreach (var (id, dictionary) in data)
        {
            var price = new Price
            {
                Id = id,
                Currencies = Currency.GetCurrencies(dictionary),
                LastUpdatedAt = dictionary.GetValueOrDefault("last_updated_at", null)
            };

            resultList.Add(price);
        }

        return resultList;
    }

    public async Task<SupportedCurrencies> GetSupportedVsCurrencies(CancellationToken ct = default)
    {
        var requestUri = CreateUrl("simple/supported_vs_currencies");
        
        var supportedCurrencies = await GetAsync<SupportedCurrencies>(requestUri, ct);
        
        return supportedCurrencies ?? [];
    }
}