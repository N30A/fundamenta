namespace Fundamenta.DataImport;

public class HttpDownloader : IDownloader
{
    private readonly HttpClient _httpClient;
    public HttpDownloader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<Stream> GetStreamAsync(string url)
    {   
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var stream = new MemoryStream();
        await response.Content.CopyToAsync(stream);
        stream.Position = 0;
        return stream;
    }
}
