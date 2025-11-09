namespace Fundamenta.Api.Host;

public static class ConnectionStringBuilder
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
    
    public static string Build(IConfiguration config)
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
}