using Scalar.AspNetCore;

namespace Fundamenta.Api.Extensions;

public static class OpenApiExtensions
{
    public static WebApplication AddScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "Fundamenta API";
        });
        
        return app;
    }
}
