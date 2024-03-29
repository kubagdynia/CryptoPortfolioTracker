using CryptoPortfolioTracker.Core.Clients;
using CryptoPortfolioTracker.Core.Extensions;

namespace CryptoPortfolioTracker.App;

public class App(ICoinGeckoClient coinGeckoClient)
{
    public async Task Run()
    {
        var price = await coinGeckoClient.GetSimplePrice(["bitcoin", "ethereum"], ["usd", "pln"]);

        foreach (var item in price)
        {
            Console.WriteLine(item.Id);
            Console.WriteLine(item.LastUpdatedAt);
            var currency = item.Currencies.Currency("usd");
            Console.WriteLine(currency?.Name);
            Console.WriteLine(currency?.Price);
        }
        
        await Task.CompletedTask;
    }
}