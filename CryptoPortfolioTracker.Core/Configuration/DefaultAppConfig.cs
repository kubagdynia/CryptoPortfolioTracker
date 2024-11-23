namespace CryptoPortfolioTracker.Core.Configuration;

internal abstract class DefaultAppConfig
{
    public static AppSettings GetDefaultConfig()
    {
        var appConfig = new AppSettings
        {
            Portfolio = new Portfolio
            {
                Currencies = [ "usd" ],
                CryptoPortfolio = []
            },
            ApiKeys = GetDefaultApiKeys()
        };
        
        return appConfig;
    }
    
    public static List<Api> GetDefaultApiKeys()
    {
        var apiKeys = new List<Api>
        {
            new()
            {
                Selected = true,
                Name = "CoinGecko-Free",
                Url = "https://api.coingecko.com/api/v3/"
            },
            new()
            {
                Selected = false,
                Name = "CoinGecko-DemoApi",
                Url = "https://api.coingecko.com/api/v3/",
                Parameters =
                [
                    new Parameter
                    {
                        Name = "x_cg_demo_api_key",
                        Value = ""
                    }
                ]
            },
            new()
            {
                Selected = false,
                Name = "CoinGecko-ProApi",
                Url = "https://pro-api.coingecko.com/api/v3/",
                Parameters =
                [
                    new Parameter
                    {
                        Name = "x_cg_pro_api_key",
                        Value = ""
                    }
                ]
            }
        };
        return apiKeys;
    }
}