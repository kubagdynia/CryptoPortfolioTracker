namespace CryptoPortfolioTracker.Core.Configuration;

public class AppSettings
{
    public const string AppConfigSectionName = "App";

    public Portfolio Portfolio { get; set; } = new();

    public List<Api> ApiKeys { get; set; } = [];

    public bool IsApiKeyExistsAndEnabled() 
        => ApiKeys.Count != 0 && ApiKeys.Any(c => c.Enabled && !string.IsNullOrWhiteSpace(c.ApiKey));
    
    public bool IsProApiKeyExistsAndEnabled(out Api apiKey)
    {
        if (ApiKeys.Count != 0 && ApiKeys.Any(c => c.Enabled && !string.IsNullOrWhiteSpace(c.ApiKey)))
        {
            apiKey = ApiKeys.First(c => c.Enabled && !string.IsNullOrWhiteSpace(c.ApiKey));
            return true;
        }

        apiKey = new Api();
        return false;
    }
}

public class Api
{
    public string Name { get; set; }

    public string Url { get; set; }
    
    public string ApiKey { get; set; }

    public bool Enabled { get; set; }
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