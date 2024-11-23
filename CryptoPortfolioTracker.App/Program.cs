using CommandLine;
using CryptoPortfolioTracker.App;
using CryptoPortfolioTracker.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var services = new ServiceCollection();

ConfigureServices();

var runningOptions = CheckCommandLineOptions(args); 

// create service provider
var serviceProvider = services.BuildServiceProvider();

await serviceProvider.GetService<App>()!.Run(runningOptions);

return;

void ConfigureServices()
{
    // build config
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("baseappsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("cryptoportfolio.json", optional: true, reloadOnChange: true)
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
    
    services.RegisterCryptoPortfolioTracker(configuration);
    services.AddTransient<App>();
}

static RunningOptions CheckCommandLineOptions(string[] args)
{
    var options = new RunningOptions();
    Parser.Default.ParseArguments<RunningOptions>(args)
        .WithParsed(opt =>
        {
            options = opt;
        })
        .WithNotParsed(_ =>
        {
            // in case of parameter parsing errors or using help option close the application
            Environment.Exit(0);
        });
    return options;
}
