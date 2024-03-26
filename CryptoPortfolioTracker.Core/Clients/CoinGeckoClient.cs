using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Polly.Timeout;

namespace CryptoPortfolioTracker.Core.Clients;

public class CoinGeckoClient(IHttpClientFactory clientFactory, ILogger<CoinGeckoClient> logger) : ICoinGeckoClient
{
    private static readonly Uri ApiEndpoint = new("https://api.coingecko.com/api/v3/");

    public async Task<Ping?> GetPingAsync(CancellationToken ct = default)
    {
        var requestUri = new Uri(ApiEndpoint, "ping");
        return await GetAsync<Ping>(requestUri, ct);
    }
    
    private async Task<T?> GetAsync<T>(Uri requestUri, CancellationToken ct = default)
    {
        var client = clientFactory.CreateClient("ClientWithoutSSLValidation");

        try
        {
            using var response = await client.GetAsync(requestUri, ct);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var ping = await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
                    return ping;

                }
                catch (NotSupportedException ex) // When content type is not valid
                {
                    logger.LogError(ex, $"The content type is not supported.");
                    throw;
                }
                catch (JsonException ex) // Invalid JSON
                {
                    logger.LogError(ex, $"Invalid JSON.");
                    throw;
                }
            }
        }
        catch (TimeoutRejectedException ex)
        {
            logger.LogError(ex, $"Timeout has occurred.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred.");
            throw;
        }

        return await Task.FromResult<T>(default!);
    }
}