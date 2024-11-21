using CryptoPortfolioTracker.Core.Exceptions;

namespace CryptoPortfolioTracker.Core.Configuration;

public record AppSettings
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
        
        throw new ConfigurationException("Configuration error. At least one ApiKey should be marked as selected - ApiKeys -> Selected: true ");
    }
}

public record Api
{
    public string Name { get; set; }

    public string Url { get; set; }
    
    public Parameter[]? Parameters { get; set; }

    public bool Selected { get; set; }
    
    public Dictionary<string, object> GetParametersAsDictionary()
    {
        if (Parameters is null)
        {
            return new Dictionary<string, object>();
        }

        return Parameters.ToDictionary(key => key.Name, value => value.Value);
    }
}

public record Parameter
{
    public string Name { get; set; }
    
    public object Value { get; set; }
}

public record Portfolio
{
    public string[]? Currencies { get; set; }

    public List<Crypto>? CryptoPortfolio { get; set; }
}

public record Crypto
{
    public required string CoinId { get; set; }

    public required decimal Quantity { get; set; }
}