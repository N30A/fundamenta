namespace Fundamenta.DataImport;

public interface IDownloader
{
    public Task<Stream> GetStreamAsync(string url);
}
