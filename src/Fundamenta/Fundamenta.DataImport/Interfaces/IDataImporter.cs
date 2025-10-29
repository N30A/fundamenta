namespace Fundamenta.DataImport.Interfaces;

public interface IDataImporter
{
    Task ImportAsync(string isin);
    protected static string Normalize(string text) => text.Trim().ToUpper();
}
