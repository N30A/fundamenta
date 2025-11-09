namespace Fundamenta.DataAccess.Repositories.Queries;

public static class InstrumentQueries
{
    public const string Upsert = """
        INSERT INTO instruments(isin, name, currency)
        VALUES (@Isin, @Name, @Currency)
        ON CONFLICT (isin)
        DO UPDATE SET name = excluded.name;          
    """;
}
