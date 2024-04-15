namespace CryptoPortfolioTracker.Core.Services.Models;

public class PortfolioDto
{
    public Dictionary<string, Dictionary<string, decimal?>> FullPortfolio { get; set; } = new();

    public Dictionary<string, decimal?> GetPortfolioByCurrencies()
    {
        if (FullPortfolio.Values.Count == 0)
        {
            return new Dictionary<string, decimal?>();
        }
        
        Dictionary<string, decimal?> pricesByCurrencies =
            FullPortfolio.Values.SelectMany(c => c.Keys).Select(v => v).Distinct()
                .Select(c => new KeyValuePair<string, decimal?>(c, 0)).ToDictionary();

        foreach (var v1 in FullPortfolio.Values)
        {
            foreach (var v2 in v1)
            {
                pricesByCurrencies[v2.Key] += v2.Value;
            }
        }

        return pricesByCurrencies;
    }
}