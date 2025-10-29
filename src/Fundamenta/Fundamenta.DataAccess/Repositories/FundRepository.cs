using System.Data;
using Fundamenta.DataAccess.Interfaces;
using Fundamenta.Models;
using Dapper;
using Fundamenta.DataAccess.Repositories.Queries;


namespace Fundamenta.DataAccess.Repositories;

public class FundRepository : IFundRepository
{
    private readonly IDbConnection _connection;

    public FundRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<Fund?> GetByIsinAsync(string isin, IDbTransaction? transaction = null)
    {
        return await _connection.QuerySingleOrDefaultAsync<Fund>(FundQueries.GetByIsin, new { Isin = isin }, transaction);
    }

    public async Task AddHoldingsAsync(int fundId, IEnumerable<InstrumentHolding> holdings, IDbTransaction? transaction = null)
    {   
        holdings = holdings.ToList();
        
        bool localTransaction = transaction == null;
        transaction ??= _connection.BeginTransaction();
        
        try
        {
            await _connection.ExecuteAsync("""
                DELETE FROM instruments_holdings
                WHERE instrument_id = @FundId;
            """, new { FundId = fundId }, transaction);
            
            string[] isins = holdings.Select(holding => holding.Instrument.Isin).ToArray();
            
            Dictionary<string, int> idMap = (await _connection.QueryAsync<(string isin, int id)>(
                "SELECT isin, id FROM instruments WHERE isin = ANY(@isins)",
                new { isins },
                transaction
            )).ToDictionary(x => x.isin, x => x.id);
            
            var holdingsParams = holdings.Select(holding => new
            {
                FundId = fundId,
                HoldingId = idMap[holding.Instrument.Isin],
                Weight = holding.Weight,
            });
            
            await _connection.ExecuteAsync("""
                INSERT INTO instruments_holdings(instrument_id, holding_id, weight)
                VALUES (@FundId, @HoldingId, @Weight)
                ON CONFLICT (instrument_id, holding_id)
                DO UPDATE SET weight = excluded.weight;
            """, holdingsParams, transaction);
           
            if (localTransaction)
            {
                transaction.Commit();
            }
        }
        catch   
        {
            if (localTransaction)
            {
                transaction.Rollback();
            }
            throw;
        }
    }
}
