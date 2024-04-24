using CryptoPortfolioTracker.Core.Exceptions;

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
        
        throw new ConfigurationException("Configuration error. At least one ApiKey should be marked as selected - ApiKeys -> Selected: true ");
    }
}

public class Api
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