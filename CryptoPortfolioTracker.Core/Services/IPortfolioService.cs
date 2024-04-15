using CryptoPortfolioTracker.Core.Services.Models;

namespace CryptoPortfolioTracker.Core.Services;

public interface IPortfolioService
{
    Task<Dictionary<string, decimal?>> GetPortfolio();

    Task<PortfolioDto> GetPortfolioByCoinId();
}