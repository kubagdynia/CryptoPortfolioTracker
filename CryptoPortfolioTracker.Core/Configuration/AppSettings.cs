namespace CryptoPortfolioTracker.Core.Configuration;

public class AppSettings
{
    public string[] Currencies { get; set; }
    
    public List<Crypto>? CryptoPortfolio { get; set; }
}

public class Crypto
{
    public required string CoinId { get; set; }

    public required decimal Quantity { get; set; }
}