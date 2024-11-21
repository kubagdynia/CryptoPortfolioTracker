namespace CryptoPortfolioTracker.Core.Services.Models;

public record PortfolioItem
{
    public required string ItemId { get; set; }
    
    public required decimal Quantity { get; set; }

    public required Dictionary<string, decimal?> PriceByCurrencies { get; set; } = new();
    
    public Dictionary<string, decimal?> Values { get; set; } = new();
}