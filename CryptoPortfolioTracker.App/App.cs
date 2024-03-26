using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Clients;
using Microsoft.Extensions.Options;

namespace CryptoPortfolioTracker.App;

public class App(IOptions<AppSettings> appSettings, ICoinGeckoClient coinGeckoClient)
{
    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task Run()
    {
        Console.WriteLine($"App name: {_appSettings.Name}");
        var ping = await coinGeckoClient.GetPingAsync();
        Console.WriteLine(ping?.GeckoSays);
        
        await Task.CompletedTask;
    }
}