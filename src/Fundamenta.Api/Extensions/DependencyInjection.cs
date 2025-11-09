using System.Data;
using Npgsql;

namespace Fundamenta.Api.Extensions;

public static class DependencyInjection
{
    private static string GetEnvironmentVariableOrThrow(IConfiguration config, string key)
    {
        string? value = config.GetValue<string>(key);
        
        if (string.IsNullOrWhiteSpace(value))
        {   
            throw new InvalidOperationException($"Environment variable '{key}' is required but was either empty or not found.");
        }
        
        return value;
    }
    
    private static string BuildConnectionString(IConfiguration config)
    {   
        string? connectionString = config.GetValue<string>("DB:CONNECTION_STRING");

        if (!string.IsNullOrWhiteSpace(connectionString))
        {   
            return connectionString;
        }
        
        string host = GetEnvironmentVariableOrThrow(config, "DB:HOSTNAME");
        string database = GetEnvironmentVariableOrThrow(config, "DB:DATABASE");
        string username = GetEnvironmentVariableOrThrow(config, "DB:USERNAME");
        string password = GetEnvironmentVariableOrThrow(config, "DB:PASSWORD");
        string? options = config.GetValue<string>("DB:OPTIONS");
        
        connectionString = $"Host={host};Database={database};Username={username};Password={password}";
        
        if (!string.IsNullOrWhiteSpace(options))
        {
            connectionString += $";{options.Trim()}";
        }
        
        return connectionString;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = BuildConnectionString(config);
        
        try
        {   
            using (var connection = new NpgsqlConnection(connectionString))
            {   
                connection.Open();
            }
            
            services.AddTransient<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Could not connect to the database.", ex);
        }
        
        return services;
    }
}
