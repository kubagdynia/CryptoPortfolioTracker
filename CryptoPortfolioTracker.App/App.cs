using CryptoPortfolioTracker.Core.Services;

namespace CryptoPortfolioTracker.App;

public class App(IPortfolioService portfolioService)
{
    public async Task Run()
    {
        var portfolioByCoin = await portfolioService.GetPortfolioByCoinId();

        var portfolioByCurrencies = portfolioByCoin.GetPortfolioByCurrencies();
        foreach (var curr in portfolioByCurrencies)
        {
            Console.WriteLine($"{curr.Key}: {curr.Value}");
        }
    }
}