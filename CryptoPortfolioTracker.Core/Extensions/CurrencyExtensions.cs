using CryptoPortfolioTracker.Core.Clients;

namespace CryptoPortfolioTracker.Core.Extensions;

public static class CurrencyExtensions
{
    public static Currency? Currency(this IList<Currency>? currencies)
    {
        return currencies?.FirstOrDefault();
    }
    
    public static Currency? Currency(this IList<Currency>? currencies, string name)
    {
        return currencies?.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}