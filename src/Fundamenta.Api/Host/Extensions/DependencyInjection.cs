using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Npgsql;
using Scalar.AspNetCore;

namespace Fundamenta.Api.Host.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = ConnectionStringBuilder.Build(config);
        services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        
        return services;
    }
    
    public static WebApplication AddScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "Fundamenta API";
        });
        
        return app;
    }
    
    public static IServiceCollection ConfigureJsonSerializer(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
        });
        
        return services;
    }
}
