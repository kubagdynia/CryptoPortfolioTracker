using CommandLine;

namespace CryptoPortfolioTracker.App;

public class RunningOptions
{
    [Option(shortName: 'f', longName: "GroupingPortfolioByCoins", Default  = false, Required = false, HelpText = "Grouping portfolio by coins")]
    public bool GroupingPortfolioByCoins { get; set; }
}