using System.Collections.ObjectModel;

namespace CryptoPortfolioTracker.Core.Clients.Models;

public class PriceContract
{
    public required string Contract { get; set; }

    public IList<Currency> Currencies { get; set; } = new Collection<Currency>();

    public decimal? LastUpdatedAt { get; set; }
}