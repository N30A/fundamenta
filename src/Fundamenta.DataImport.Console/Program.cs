using Fundamenta.DataAccess.Repositories;
using Fundamenta.DataImport.Importers;
using Npgsql;

namespace Fundamenta.DataImport.Console;

public class Program
{
    private const string ConnectionString = "Host=localhost;Database=postgres;Username=postgres;Password=mysecretpassword";
    private const string UserAgent = "Fundamenta API";

    public static async Task Main()
    {   
        HttpClientHandler handler = new()
        {
            AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
            AllowAutoRedirect = true
        };
        using HttpClient client = new(handler);
        client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        
        InstrumentRepository instrumentRepository = new(connection);
        FundRepository fundRepository = new(connection);
        
        var importer = new SPDRImporter(
            connection,
            client,
            instrumentRepository,
            fundRepository
        );

        await importer.ImportAsync("IE00B3YLTY66");

        // List<InstrumentHolding> newInstruments = (await FetchNewInstruments(Url)).ToList();
        //
        // await using var connection = new NpgsqlConnection(ConnectionString);
        // await connection.OpenAsync();
        // await using var transaction = await connection.BeginTransactionAsync();
        //
        // int fundId = await GetInstrumentIdFromIsin(FundIsin, connection, transaction);
        // await AddMultipleInstruments(newInstruments, connection, transaction);
        // await DeleteInstrumentHoldings(fundId, connection, transaction);
        // await AddMultipleHoldingsToInstrument(fundId, newInstruments, connection, transaction);
        // await transaction.CommitAsync();
    }
    
    // private static async Task AddMultipleHoldingsToInstrument(
    //     int instrumentId,
    //     List<InstrumentHolding> newInstruments, 
    //     IDbConnection connection,
    //     IDbTransaction? transaction = null
    // )
    // {
    //     const string query = """
    //         INSERT INTO instruments_holdings(instrument_id, holding_id, weight)
    //         VALUES (@instrumentId, @holdingId, @weight)
    //         ON CONFLICT (instrument_id, holding_id)
    //         DO UPDATE SET weight = excluded.weight;
    //     """;
    //
    //     string[] isins = newInstruments.Select(i => i.Instrument.Isin).ToArray();
    //
    //     Dictionary<string, int> idMap = (await connection.QueryAsync<(string isin, int id)>(
    //         "SELECT isin, id FROM instruments WHERE isin = ANY(@isins)",
    //         new { isins },
    //         transaction
    //     )).ToDictionary(x => x.isin, x => x.id);
    //
    //     var holdingsParams = newInstruments.Select(h => new
    //     {
    //         instrumentId,
    //         holdingId = idMap[h.Instrument.Isin],
    //         weight = h.Weight
    //     });
    //
    //     await connection.ExecuteAsync(query, holdingsParams, transaction);
    // }
    //
    // private static async Task AddMultipleInstruments(
    //     IEnumerable<InstrumentHolding> instruments,
    //     IDbConnection connection,
    //     IDbTransaction? transaction = null
    // )
    // {
    //     const string query = """
    //         INSERT INTO instruments(isin, name, base_currency)
    //         VALUES (@isin, @name, @baseCurrency)
    //         ON CONFLICT (isin) DO NOTHING;
    //     """;
    //
    //     foreach (var instrument in instruments)
    //     {
    //         await connection.ExecuteAsync(query, new
    //         {
    //             isin = instrument.Instrument.Isin,
    //             name = instrument.Instrument.Name,
    //             baseCurrency = instrument.Instrument.Currency,
    //         }, transaction);
    //     }
    // }
    //
    // private static async Task DeleteInstrumentHoldings(int instrumentId, IDbConnection connection, IDbTransaction? transaction = null)
    // {   
    //     const string query = """
    //         DELETE FROM instruments_holdings
    //         WHERE instrument_id = @instrumentId;
    //     """;
    //     
    //     await connection.ExecuteAsync(query, new { instrumentId }, transaction);
    // }
    //
    // private static async Task<int> GetInstrumentIdFromIsin(string isin, IDbConnection connection, IDbTransaction? transaction = null)
    // {   
    //     const string query = """
    //         SELECT id FROM instruments
    //         WHERE isin = @isin;
    //     """;
    //     
    //     return await connection.QuerySingleOrDefaultAsync<int>(query, new { isin }, transaction);
    // }
    //
    // private static async Task<IEnumerable<InstrumentHolding>> FetchNewInstruments(string isin)
    // {
    //     HttpClientHandler handler = new()
    //     {
    //         AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
    //         AllowAutoRedirect = true
    //     };
    //     HttpClient client = new(handler);
    //     client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
    //     
    //     SPDRImporter importer = new();
    //     return await importer.ImportAsync(isin);
    // }
}
