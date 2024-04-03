using System.Collections.ObjectModel;

namespace CryptoPortfolioTracker.Core.Clients.Models;

public class Price
{
    public required string Id { get; set; }

    public IList<Currency> Currencies { get; set; } = new Collection<Currency>();

    public decimal? LastUpdatedAt { get; set; }
}