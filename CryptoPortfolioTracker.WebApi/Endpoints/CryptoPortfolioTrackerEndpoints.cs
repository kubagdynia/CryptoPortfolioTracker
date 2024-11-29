using CryptoPortfolioTracker.Core.Services;

namespace CryptoPortfolioTracker.WebApi.Endpoints;

public static class CryptoPortfolioTrackerEndpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder route = app.MapGroup("api/v1");
        
        route.MapGet("/portfolio", async (IPortfolioService portfolioService) =>
        {
            return await portfolioService.GetPortfolioByCurrencies();
        });

        route.MapGet("/portfolio/{coinId}", async (IPortfolioService portfolioService, string coinId) =>
        {
            return await portfolioService.GetPortfolioByCoinId();
        });

        return app;
    }
}