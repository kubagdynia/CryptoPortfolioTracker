using CryptoPortfolioTracker.Core.Services.Models;

namespace CryptoPortfolioTracker.Core.Services;

public interface IPortfolioService
{
    // Get the portfolio by currencies
    Task<Dictionary<string, decimal?>> GetPortfolioByCurrencies();

    // Get the portfolio by coin id
    Task<PortfolioDto> GetPortfolioByCoinId();
}