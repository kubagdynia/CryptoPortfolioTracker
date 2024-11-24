﻿using CryptoPortfolioTracker.Core.Configuration;
using CryptoPortfolioTracker.Core.Extensions;
using CryptoPortfolioTracker.Core.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// create settings
var settings = new AppSettings
{
    Portfolio = new Portfolio
    {
        Currencies = [ "usd", "pln" ],
        CryptoPortfolio =
        [
            new Crypto
            {
                CoinId = "bitcoin",
                Quantity = 1.21m
            },

            new Crypto
            {
                CoinId = "ethereum",
                Quantity = 2
            }
        ]
    }
};

// register services
services.RegisterCryptoPortfolioTracker(settings);

// build service provider
var serviceProvider = services.BuildServiceProvider();

// get portfolio service
var portfolioService = serviceProvider.GetRequiredService<IPortfolioService>();

// get portfolio by currencies
var portfolioByCurrencies = await portfolioService.GetPortfolioByCurrencies();

// print portfolio by currencies
foreach (var value in portfolioByCurrencies)
{
    Console.WriteLine($"{value.Key}: {value.Value}");
}

// output:
// usd: xxxxx
// pln: xxxxx

