using CryptoPortfolioTracker.App;
using CryptoPortfolioTracker.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var services = new ServiceCollection();

ConfigureServices();

// create service provider
var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<App>()!.Run();

return;

void ConfigureServices()
{
    // build config
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
    
    // defining Serilog configs
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    
    // configure logging
    services.AddLogging(builder =>
    { 
        builder.AddSerilog();
    });
    
    services.RegisterCore(configuration);
    services.AddTransient<App>();
}
