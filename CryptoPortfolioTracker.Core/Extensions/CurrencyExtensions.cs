using CryptoPortfolioTracker.Core.Clients.Models;

namespace CryptoPortfolioTracker.Core.Extensions;

public static class CurrencyExtensions
{
    public static Currency? Currency(this IList<Currency> currencies)
    {
        ArgumentNullException.ThrowIfNull(currencies);
        
        return currencies?.FirstOrDefault();
    }
    
    public static Currency? Currency(this IList<Currency> currencies, string name)
    {
        ArgumentNullException.ThrowIfNull(currencies);
        
        return currencies?.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}