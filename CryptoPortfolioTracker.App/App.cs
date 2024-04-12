using CryptoPortfolioTracker.Core.Services;

namespace CryptoPortfolioTracker.App;

public class App(IPortfolioService portfolioService)
{
    public async Task Run()
    {
        var portfolio = await portfolioService.GetPortfolio();
        
        foreach (var curr in portfolio)
        {
            Console.WriteLine($"{curr.Key}: {curr.Value}");
        }
    }
}