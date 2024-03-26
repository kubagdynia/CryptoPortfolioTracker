namespace CryptoPortfolioTracker.Core.Clients;

public interface ICoinGeckoClient
{
    /// <summary>
    /// Check API server status
    /// </summary>
    Task<Ping?> GetPingAsync(CancellationToken ct = default);
}