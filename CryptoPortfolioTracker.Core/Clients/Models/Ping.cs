using System.Text.Json.Serialization;

namespace CryptoPortfolioTracker.Core.Clients.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class Ping
{
    [JsonPropertyName("gecko_says")]
    public string? GeckoSays { get; set; }
}