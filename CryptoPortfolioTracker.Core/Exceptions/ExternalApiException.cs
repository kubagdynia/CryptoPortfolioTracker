using System.Net;

namespace CryptoPortfolioTracker.Core.Exceptions;

public class ExternalApiException(string message, HttpStatusCode? status = null) : Exception(message)
{
    public HttpStatusCode? Status { get; set; } = status;
}