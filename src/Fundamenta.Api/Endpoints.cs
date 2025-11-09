using Fundamenta.Api.Common;
using Fundamenta.Api.Features.Funds.Endpoints;

namespace Fundamenta.Api;

public static class Endpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup(string.Empty)
            .WithOpenApi();
        
        endpoints.MapFundEndpoints();
    }

    private static void MapFundEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("/funds")
            .WithTags("Funds");

        endpoints.MapEndpoint<GetFundHoldings>();
        endpoints.MapEndpoint<GetFunds>();
        endpoints.MapEndpoint<GetFund>();
    }
    
    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
