using System.Text.Json.Serialization;

namespace CryptoPortfolioTracker.Core.Clients;

public class Ping
{
    [JsonPropertyName("gecko_says")]
    public string? GeckoSays { get; set; }
}