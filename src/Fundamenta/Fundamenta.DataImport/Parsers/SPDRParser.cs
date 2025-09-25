using System.Globalization;
using ClosedXML.Excel;
using Fundamenta.Models;

namespace Fundamenta.DataImport.Parsers;

public class SPDRParser : IInstrumentParser
{
    private readonly IDownloader _downloader;
    private const int RowSkipCount = 5;
    private const string NoIsinText = "Unassigned";
    
    public SPDRParser(IDownloader downloader)
    {
        _downloader = downloader;
    }
    
    public async Task<IEnumerable<InstrumentHolding>> ParseAsync(string url)
    {
        Stream stream;
        try
        {
            stream = await _downloader.GetStreamAsync(url);
        }
        catch (HttpRequestException)
        {
            return [];
        }
        
        
        using var workBook = new XLWorkbook(stream);
        var worksheet = workBook.Worksheets.FirstOrDefault();
        if (worksheet == null)
        {
            return [];
        }
        
        var rows = worksheet.RowsUsed().Skip(RowSkipCount).ToList();
        if (rows.Count > 0)
        {
            rows.RemoveAt(rows.Count - 1);
        }
        
        List<InstrumentHolding> holdings = [];
        foreach (var row in rows)
        {   
            string isin = row.Cell(1).GetValue<string>().Trim();
            if (string.IsNullOrEmpty(isin) || isin == NoIsinText)
            {
                continue;
            }

            string weightText = row.Cell(6).GetValue<string>().Trim();
            if (!decimal.TryParse(weightText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal weight))
            {
                continue;
            }
            
            holdings.Add(new InstrumentHolding
            {
                Instrument = new Instrument
                {
                    ISIN = isin,
                    Name = row.Cell(3).GetValue<string>().Trim(),
                    BaseCurrency = row.Cell(4).GetValue<string>().Trim(),
                },
                Weight = weight
            });
        }
        
        return holdings;
    }
}
