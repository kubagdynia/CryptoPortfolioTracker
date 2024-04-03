using CryptoPortfolioTracker.Core.Clients.Models;

namespace CryptoPortfolioTracker.Core.Clients;

public interface ICoinGeckoClient
{
    /// <summary>
    /// Check API server status.
    /// </summary>
    Task<Ping?> GetPingAsync(CancellationToken ct = default);

    /// <summary>
    /// Get the current price of any cryptocurrencies in any other supported currencies that you need.
    /// </summary>
    Task<Price> GetSimplePrice(string id, string currency = "usd",
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get the current price of any cryptocurrencies in any other supported currencies that you need.
    /// </summary>
    Task<IList<Price>> GetSimplePrice(string[] ids, string[] currencies,
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get list of supported_vs_currencies.
    /// </summary>
    Task<SupportedCurrencies> GetSupportedVsCurrencies(CancellationToken ct = default);
}