using Microsoft.Extensions.Configuration;

namespace CryptoPortfolioTracker.Core.Configuration;

public abstract class DefaultAppConfig
{
    public static IConfigurationSection GetDefaultAppConfig(string appConfigSectionName)
    {
        var config = """
         {
             "App": {
               "Portfolio": {
                 "Currencies": ["usd"],
                 "CryptoPortfolio": [
                   
                 ]
               },
               "ApiKeys": [
                 {
                   "Selected": true,
                   "Name": "CoinGecko-Free",
                   "Url": "https://api.coingecko.com/api/v3/"
                 },
                 {
                   "Selected": false,
                   "Name": "CoinGecko-ProApi",
                   "Url": "https://pro-api.coingecko.com/api/v3/",
                   "Parameters": [
                     {
                       "Name": "x_cg_pro_api_key",
                       "Value": ""
                     }
                   ]
                 }
               ]
             }
         }
         """;
        
        using var mem = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config));
        
        var configuration = new ConfigurationBuilder().AddJsonStream(mem).Build();
        return configuration.GetSection(appConfigSectionName);
    }
}