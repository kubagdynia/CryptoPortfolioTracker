using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Clients.Models;
using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Services.Models;
using Microsoft.Extensions.Options;

namespace CryptoPortfolioTracker.Core.Services;

public class PortfolioService(ICoinGeckoClient coinGeckoClient, IOptions<AppSettings> settings) : IPortfolioService
{
    private readonly AppSettings _appSettings = settings.Value;

    public async Task<Dictionary<string, decimal?>> GetPortfolioByCurrencies()
    {
        // If the portfolio is not set, return an empty dictionary
        if (_appSettings.Portfolio.CryptoPortfolio is null || _appSettings.Portfolio.Currencies is null)
            return await Task.FromResult(new Dictionary<string, decimal?>());
        
        var cryptoIds = _appSettings.Portfolio.CryptoPortfolio.Select(c => c.CoinId).ToArray();
            
        var prices = await coinGeckoClient.GetSimplePrice(cryptoIds, _appSettings.Portfolio.Currencies);

        var portfolio = _appSettings.Portfolio.Currencies
            .Select(c => new KeyValuePair<string, decimal?>(c, 0))
            .ToDictionary();
            
        foreach (var crypto in _appSettings.Portfolio.CryptoPortfolio)
        {
            var price = GetPriceByCoinId(crypto.CoinId, prices);
            if (price is null) continue;
            
            foreach (var currency in price.Currencies)
            {
                portfolio[currency.Name] += crypto.Quantity * currency.Price;
            }
        }

        return portfolio;

    }

    public async Task<PortfolioDto> GetPortfolioByCoinId()
    {
        if (_appSettings.Portfolio.CryptoPortfolio is null || _appSettings.Portfolio.Currencies is null)
            return await Task.FromResult(new PortfolioDto());
        
        var cryptoIds = _appSettings.Portfolio.CryptoPortfolio.Select(c => c.CoinId).ToArray();
            
        var prices = await coinGeckoClient.GetSimplePrice(cryptoIds, _appSettings.Portfolio.Currencies);

        var portfolioDto = new PortfolioDto
        {
            FullPortfolio = cryptoIds.Select(id =>
                new KeyValuePair<string, PortfolioItem>(id, new PortfolioItem
                {
                    ItemId = id,
                    Quantity = _appSettings.Portfolio.CryptoPortfolio
                        .Where(c => c.CoinId.Equals(id, StringComparison.OrdinalIgnoreCase))
                        .Sum(c => c.Quantity),
                    PriceByCurrencies = _appSettings.Portfolio.Currencies
                        .Select(c => new KeyValuePair<string, decimal?>(c, 0)).ToDictionary(),
                    Values = _appSettings.Portfolio.Currencies
                        .Select(c => new KeyValuePair<string, decimal?>(c, 0)).ToDictionary()
                })).ToDictionary()
        };
            
        foreach (var crypto in _appSettings.Portfolio.CryptoPortfolio)
        {
            var price = GetPriceByCoinId(crypto.CoinId, prices);
            if (price is null) continue;
            
            foreach (var currency in price.Currencies)
            {
                portfolioDto.FullPortfolio[crypto.CoinId].PriceByCurrencies[currency.Name] = currency.Price;
                portfolioDto.FullPortfolio[crypto.CoinId].Values[currency.Name] += crypto.Quantity * currency.Price;
            }
        }

        return portfolioDto;

    }
    
    private PriceId? GetPriceByCoinId(string coinId, IList<PriceId> prices)
        => prices.SingleOrDefault(p => p.Id.Equals(coinId, StringComparison.OrdinalIgnoreCase));
}