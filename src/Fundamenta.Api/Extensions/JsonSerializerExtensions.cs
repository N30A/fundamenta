using System.Text.Json;

namespace Fundamenta.Api.Extensions;

public static class JsonSerializerExtensions
{   
    public static IServiceCollection ConfigureJsonSerializer(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });
        
        return services;
    }
}
