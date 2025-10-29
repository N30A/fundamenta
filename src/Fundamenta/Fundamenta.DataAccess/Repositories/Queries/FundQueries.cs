namespace Fundamenta.DataAccess.Repositories.Queries;

public static class FundQueries
{
    public const string GetByIsin = """
        SELECT
            i.id, i.isin, i.name, i.currency,
            f.holdings_url AS HoldingsUrl
        FROM funds f
        JOIN instruments i ON f.instrument_id = i.id
        WHERE i.isin = @Isin;
    """;
}
