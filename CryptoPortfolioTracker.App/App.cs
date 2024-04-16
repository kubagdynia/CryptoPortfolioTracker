using CryptoPortfolioTracker.Core.Services;

namespace CryptoPortfolioTracker.App;

public class App(IPortfolioService portfolioService)
{
    public async Task Run(RunningOptions runningOptions)
    {
        if (runningOptions.GroupingPortfolioByCoins)
        {
            var portfolioByCoin = await portfolioService.GetPortfolioByCoinId();

            foreach (var fullPortfolio in portfolioByCoin.FullPortfolio)
            {
                Console.WriteLine($"** {fullPortfolio.Key}");
                foreach (var curr in fullPortfolio.Value)
                {
                    Console.WriteLine($"   {curr.Key}: {curr.Value}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine("summary: ");
            var portfolioByCurrencies = portfolioByCoin.GetPortfolioByCurrencies();
            foreach (var curr in portfolioByCurrencies)
            {
                Console.WriteLine($"{curr.Key}: {curr.Value}");
            }
        }
        else
        {
            var portfolioByCurrencies = await portfolioService.GetPortfolio();
            foreach (var curr in portfolioByCurrencies)
            {
                Console.WriteLine($"{curr.Key}: {curr.Value}");
            }
        }
    }
}