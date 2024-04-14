namespace CryptoPortfolioTracker.Core.Services;

public interface IPortfolioService
{
    Task<Dictionary<string, decimal?>> GetPortfolio();

    Task<Dictionary<string, Dictionary<string, decimal?>>> GetPortfolioByCoinId();
}