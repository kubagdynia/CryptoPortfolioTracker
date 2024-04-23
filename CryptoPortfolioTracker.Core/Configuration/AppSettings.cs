namespace CryptoPortfolioTracker.Core.Configuration;

public class AppSettings
{
    public const string AppConfigSectionName = "App";

    public Portfolio Portfolio { get; set; } = new();

    public List<Api> ApiKeys { get; set; } = [];

    public Api GetSelectedApi()
    {
        if (ApiKeys.Count != 0 && ApiKeys.Any(c => c.Selected))
        {
            return ApiKeys.First(c => c.Selected);
        }
        
        return new Api();
    }
}

public class Api
{
    public string Name { get; set; }

    public string Url { get; set; }
    
    public Parameter[]? Parameters { get; set; }

    public bool Selected { get; set; }
}

public class Parameter
{
    public string Name { get; set; }
    
    public object Value { get; set; }
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