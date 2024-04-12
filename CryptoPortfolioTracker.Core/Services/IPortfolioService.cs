namespace CryptoPortfolioTracker.Core.Services;

public interface IPortfolioService
{
    Task<Dictionary<string, decimal?>> GetPortfolio();
}