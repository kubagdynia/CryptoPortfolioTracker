namespace CryptoPortfolioTracker.Core.Configuration;

public class AppSettings
{
    public const string AppConfigSectionName = "App";

    public Portfolio Portfolio { get; set; } = new();
}

public class Portfolio
{
    public string[]? Currencies { get; set; }

    public List<Crypto>? CryptoPortfolio { get; set; }
}

public class Crypto
{
    public required string CoinId { get; set; }

    public required decimal Quantity { get; set; }
}