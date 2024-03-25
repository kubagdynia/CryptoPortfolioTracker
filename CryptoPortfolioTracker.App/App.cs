using CryptoPortfolioTracker.Core.Configuration;
using Microsoft.Extensions.Options;

namespace CryptoPortfolioTracker.App;

public class App(IOptions<AppSettings> appSettings)
{
    private readonly AppSettings _appSettings = appSettings.Value;

    public async Task Run()
    {
        Console.WriteLine($"App name: {_appSettings.Name}");

        await Task.CompletedTask;
    }
}