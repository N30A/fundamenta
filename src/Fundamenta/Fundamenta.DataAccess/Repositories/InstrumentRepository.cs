using System.Data;
using Dapper;
using Fundamenta.DataAccess.Interfaces;
using Fundamenta.DataAccess.Repositories.Queries;
using Fundamenta.Models;

namespace Fundamenta.DataAccess.Repositories;

public class InstrumentRepository : IInstrumentRepository
{   
    private readonly IDbConnection _connection;

    public InstrumentRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task UpsertMultipleAsync(IEnumerable<Instrument> instruments, IDbTransaction? transaction = null)
    {
        foreach (var instrument in instruments)
        {   
            await _connection.ExecuteAsync(InstrumentQueries.Upsert, new
            {
                instrument.Isin,
                instrument.Name,
                instrument.Currency,
            }, transaction);
        }
    }
}
