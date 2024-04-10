using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Clients.Models;
using CryptoPortfolioTracker.Core.Configuration;
using Microsoft.Extensions.Options;

namespace CryptoPortfolioTracker.App;

public class App(ICoinGeckoClient coinGeckoClient, IOptions<AppSettings> settings)
{
    private readonly AppSettings _appSettings = settings.Value;
    
    public async Task Run()
    {
        if (_appSettings.CryptoPortfolio is not null)
        {
            var cryptoIds = _appSettings.CryptoPortfolio.Select(c => c.CoinId).ToArray();
            
            var prices = await coinGeckoClient.GetSimplePrice(cryptoIds, _appSettings.Currencies);

            var portfolio =
                _appSettings.Currencies.Select(c => new KeyValuePair<string, decimal?>(c, 0)).ToDictionary();
            
            foreach (var crypto in _appSettings.CryptoPortfolio)
            {
                var price = GetPriceByCoinId(crypto.CoinId, prices);
                if (price is not null)
                {
                    foreach (var currency in price.Currencies)
                    {
                        portfolio[currency.Name] += currency.Price;
                    }
                }
            }

            foreach (var curr in portfolio)
            {
                Console.WriteLine($"{curr.Key}: {curr.Value}");
            }
        }
        
        await Task.CompletedTask;
    }

    private PriceId? GetPriceByCoinId(string coinId, IList<PriceId> prices)
        => prices.SingleOrDefault(p => p.Id.Equals(coinId, StringComparison.OrdinalIgnoreCase));
}