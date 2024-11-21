# CryptoPortfolioTracker
App for tracking crypto portfolio

## App Configuration

`baseappsettings.json` is a configuration file for the app.
Its structure is as follows:
```json
{
  "App": {
    "Portfolio": {
      "Currencies": ["usd", "pln"],
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
        "Name": "CoinGecko-DemoApi",
        "Url": "https://api.coingecko.com/api/v3/",
        "Parameters": [
          {
            "Name": "x_cg_demo_api_key",
            "Value": ""
          }
        ]
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
  },
  "Serilog" : {
    
  }
}
```
The `Currencies` array should contain the currencies in which you want to display the value of your portfolio.

The `CryptoPortfolio` array should contain the coins you own and the quantity of each coin. `NOTE:` You can leave it empty and fill it later in the `cryptoportfolio.json` file.
This is the suggested solution because you will not accidentally put the portfolio data into the repository.

The `ApiKeys` array should contain the API keys for the CoinGecko API. You can get the API keys from the [CoinGecko](https://www.coingecko.com/en) website.
You can use the free version of the API without the need for an API key. In this case, you should set the `Selected` property to `true` for the `CoinGecko-Free` object.
`NOTE:` The free version of the API has a limit of 5 to 15 calls per minute, depending on usage conditions worldwide.
To get a stable rate limit of 30 calls per minute, please register a Demo account here: [pricing](https://www.coingecko.com/en/api/pricing).
`NOTE 2:` You can leave it empty and fill it later in the `cryptoportfolio.json` file.

## Portfolio Configuration
To track your portfolio you need to create a file named `cryptoportfolio.json` in the root directory of the project.
The file should have the following structure (example):
```json
{
  "App": {
    "Portfolio": {
      "Currencies": ["usd", "pln"],
      "CryptoPortfolio": [
        {
          "coinId": "bitcoin",
          "quantity": 0.51
        },
        {
          "coinId": "ethereum",
          "quantity": 2.5
        },
        {
          "coinId": "usd-coin",
          "quantity": 500
        },
        {
          "coinId": "tether",
          "quantity": 1000
        }
      ]
    }
  }
}
```
The `Currencies` array should contain the currencies in which you want to display the value of your portfolio.

The `CryptoPortfolio` array should contain the coins you own and the quantity of each coin.

You can find the `coinId` of a coin on the [CoinGecko](https://www.coingecko.com/en) website. For example, the `coinId` of Bitcoin is `bitcoin`.

