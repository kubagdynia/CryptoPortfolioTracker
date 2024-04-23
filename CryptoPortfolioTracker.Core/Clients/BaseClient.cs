using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CryptoPortfolioTracker.Core.Exceptions;
using CryptoPortfolioTracker.Core.Extensions;
using Microsoft.Extensions.Logging;
using Polly.Timeout;

namespace CryptoPortfolioTracker.Core.Clients;

public abstract class BaseClient(IHttpClientFactory clientFactory, ILogger<BaseClient> logger)
{
    protected abstract Uri GetApiEndpoint();

    protected abstract Dictionary<string, object> GetAdditionalParameters();

    protected Uri CreateUrl(string path)
        => CreateUrl(path, new Dictionary<string, object>());
    
    protected Uri CreateUrl(string path, Dictionary<string, object> parameters)
    {
        var urlParameters = new List<string?>();
        
        parameters.AddRange(GetAdditionalParameters());
        
        foreach (var parameter in parameters)
        {
            var param = string.IsNullOrWhiteSpace(parameter.Value.ToString())
                ? null
                : $"{parameter.Key}={parameter.Value.ToString()?.ToLower(CultureInfo.InvariantCulture)}";
            
            if (param is not null)
            {
                urlParameters.Add(param);
            }
        }
        
        var encodedParams = urlParameters
            .Select(WebUtility.HtmlEncode)
            .Select((x, i) => i > 0 ? $"&{x}" : $"?{x}")
            .ToArray();
        
        var url = encodedParams.Length > 0 ? $"{path}{string.Join(string.Empty, encodedParams)}" : path;
        
        return new Uri(GetApiEndpoint(), url);
    }
    
    protected async Task<T?> GetAsync<T>(Uri requestUri, CancellationToken ct = default)
    {
        var client = clientFactory.CreateClient("ClientWithoutSSLValidation");

        try
        {
            using var response = await client.GetAsync(requestUri, ct);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct);
                    return result;

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
            
            var message = await response.Content.ReadAsStringAsync(cancellationToken: ct);
            throw new ExternalApiException(message, response.StatusCode);
        }
        catch (TimeoutRejectedException ex)
        {
            logger.LogError(ex, $"Timeout has occurred.");
            throw;
        }
        catch (ExternalApiException ex)
        {
            logger.LogError(ex, $"There was an error in the response from the external API: {ex.Status}");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred.");
            throw;
        }
    }
}