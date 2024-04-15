using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Clients.Models;
using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Services.Models;
using Microsoft.Extensions.Options;

namespace CryptoPortfolioTracker.Core.Services;

public class PortfolioService(ICoinGeckoClient coinGeckoClient, IOptions<AppSettings> settings) : IPortfolioService
{
    private readonly AppSettings _appSettings = settings.Value;

    public async Task<Dictionary<string, decimal?>> GetPortfolio()
    {
        if (_appSettings.Portfolio.CryptoPortfolio is not null && _appSettings.Portfolio.Currencies is not null)
        {
            var cryptoIds = _appSettings.Portfolio.CryptoPortfolio.Select(c => c.CoinId).ToArray();
            
            var prices = await coinGeckoClient.GetSimplePrice(cryptoIds, _appSettings.Portfolio.Currencies);
            
            var portfolio =
                _appSettings.Portfolio.Currencies.Select(c => new KeyValuePair<string, decimal?>(c, 0)).ToDictionary();
            
            foreach (var crypto in _appSettings.Portfolio.CryptoPortfolio)
            {
                var price = GetPriceByCoinId(crypto.CoinId, prices);
                if (price is not null)
                {
                    foreach (var currency in price.Currencies)
                    {
                        portfolio[currency.Name] += crypto.Quantity * currency.Price;
                    }
                }
            }

            return portfolio;
        }
        
        return await Task.FromResult(new Dictionary<string, decimal?>());
    }

    public async Task<PortfolioDto> GetPortfolioByCoinId()
    {
        if (_appSettings.Portfolio.CryptoPortfolio is not null && _appSettings.Portfolio.Currencies is not null)
        {
            var cryptoIds = _appSettings.Portfolio.CryptoPortfolio.Select(c => c.CoinId).ToArray();
            
            var prices = await coinGeckoClient.GetSimplePrice(cryptoIds, _appSettings.Portfolio.Currencies);

            var portfolioDto = new PortfolioDto
            {
                FullPortfolio = cryptoIds.Select(id =>
                        new KeyValuePair<string, Dictionary<string, decimal?>>(id,
                            _appSettings.Portfolio.Currencies.Select(c => new KeyValuePair<string, decimal?>(c, 0))
                                .ToDictionary()))
                    .ToDictionary()
            };
            
            foreach (var crypto in _appSettings.Portfolio.CryptoPortfolio)
            {
                var price = GetPriceByCoinId(crypto.CoinId, prices);
                if (price is not null)
                {
                    foreach (var currency in price.Currencies)
                    {
                        portfolioDto.FullPortfolio[crypto.CoinId][currency.Name] += crypto.Quantity * currency.Price;
                    }
                }
            }

            return portfolioDto;
        }

        return await Task.FromResult(new PortfolioDto());
    }
    
    private PriceId? GetPriceByCoinId(string coinId, IList<PriceId> prices)
        => prices.SingleOrDefault(p => p.Id.Equals(coinId, StringComparison.OrdinalIgnoreCase));
}