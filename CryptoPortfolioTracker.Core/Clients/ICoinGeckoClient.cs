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
    Task<PriceId> GetSimplePrice(string id, string currency = "usd",
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default);
    
    /// <summary>
    /// Get the current price of any cryptocurrencies in any other supported currencies that you need.
    /// </summary>
    Task<IList<PriceId>> GetSimplePrice(string[] ids, string[] currencies,
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get current price of tokens (using contract addresses) for a given platform in any other currency that you need.
    /// </summary>
    /// <param name="id">The id of the platform issuing tokens (See asset_platforms endpoint for list of options)</param>
    /// <param name="contractAddress">The contract address of token</param>
    /// <param name="currency">vs_currency of coin, default: usd</param>
    /// <param name="includeMarketCap">true/false to include market_cap, default: false</param>
    /// <param name="include24HrVol">true/false to include 24hr_vol, default: false</param>
    /// <param name="include24HrChange">true/false to include 24hr_change, default: false</param>
    /// <param name="includeLastUpdatedAt">true/false to include last_updated_at of price, default: false</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>price(s) of cryptocurrency</returns>
    Task<PriceContract> GetTokenPrice(string id, string contractAddress, string currency = "usd",
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get current price of tokens (using contract addresses) for a given platform in any other currency that you need.
    /// </summary>
    /// <param name="id">The id of the platform issuing tokens (See asset_platforms endpoint for list of options)</param>
    /// <param name="contractAddresses">The contract address of tokens</param>
    /// <param name="currencies">vs_currency of coins</param>
    /// <param name="includeMarketCap">true/false to include market_cap, default: false</param>
    /// <param name="include24HrVol">true/false to include 24hr_vol, default: false</param>
    /// <param name="include24HrChange">true/false to include 24hr_change, default: false</param>
    /// <param name="includeLastUpdatedAt">true/false to include last_updated_at of price, default: false</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>price(s) of cryptocurrency</returns>
    Task<IList<PriceContract>> GetTokenPrice(string id, string[] contractAddresses, string[] currencies,
        bool includeMarketCap = false, bool include24HrVol = false, bool include24HrChange = false,
        bool includeLastUpdatedAt = false,
        CancellationToken ct = default);

    /// <summary>
    /// Get list of supported_vs_currencies.
    /// </summary>
    Task<SupportedCurrencies> GetSupportedVsCurrencies(CancellationToken ct = default);
}