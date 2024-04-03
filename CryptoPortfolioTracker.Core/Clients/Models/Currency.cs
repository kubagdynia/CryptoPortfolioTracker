namespace CryptoPortfolioTracker.Core.Clients.Models;

public class Currency
{
    public required string Name { get; set; }

    public decimal? Price { get; set; }
    
    public decimal? MarketCap { get; set; }
    
    public decimal? Vol24H { get; set; }
    
    public decimal? Change24H { get; set; }

    public static List<Currency> GetCurrencies(Dictionary<string, decimal?> dictionary)
    {
        if (dictionary.Count == 0)
        {
            return [];
        }

        var currencyNames = dictionary
            .Where(c => !c.Key.Contains('_'))
            .Select(c => c.Key).ToList();

        var result = new List<Currency>();
        
        foreach (var currencyName in currencyNames)
        {
            var currency = new Currency
            {
                Name = currencyName,
                Price = dictionary.GetValueOrDefault(currencyName, null),
                MarketCap = dictionary.GetValueOrDefault($"{currencyName}_market_cap", null),
                Vol24H = dictionary.GetValueOrDefault($"{currencyName}_24h_vol", null),
                Change24H = dictionary.GetValueOrDefault($"{currencyName}_24h_change", null)
            };
            result.Add(currency);
        }

        return result;
    }
}