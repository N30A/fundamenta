using System.Data;
using System.Globalization;
using ClosedXML.Excel;
using Fundamenta.DataAccess.Interfaces;
using Fundamenta.DataImport.Interfaces;
using Fundamenta.Models;

namespace Fundamenta.DataImport.Importers;

internal record ImportHolding(string Isin, string Name, string Currency, decimal Weight);

public class SPDRImporter : IDataImporter
{   
    private readonly IDbConnection _connection;
    private readonly HttpClient _httpClient;
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IFundRepository _fundRepository;
    
    private const int RowSkipCount = 5;
    private const string NoIsinText = "Unassigned";
    
    public SPDRImporter(
        IDbConnection connection,
        HttpClient httpClient,
        IInstrumentRepository instrumentRepository,
        IFundRepository fundRepository
    )
    {   
        _connection = connection;
        _httpClient = httpClient;
        _instrumentRepository = instrumentRepository;
        _fundRepository = fundRepository;
    }
    
    public async Task ImportAsync(string isin)
    {   
        var transaction = _connection.BeginTransaction();

        try
        {
            var fund = await _fundRepository.GetByIsinAsync(isin, transaction);
            if (fund == null)
            {   
                Console.WriteLine($"Could not find fund for isin {isin}");
                return;
            }

            if (fund.HoldingsUrl == null)
            {   
                Console.WriteLine($"Could not find holdings url for isin {isin}");
                return;
            }

            List<ImportHolding> result = ParseXml(await GetStreamAsync(fund.HoldingsUrl));
            List<InstrumentHolding> holdings = result.Select(holding => new InstrumentHolding
            {   
                Instrument = new Instrument
                {
                    Isin = holding.Isin,
                    Name = holding.Name,
                    Currency = holding.Currency
                },
                Weight = holding.Weight
            }).ToList();
            
            await _instrumentRepository.UpsertMultipleAsync(holdings.Select(holding => holding.Instrument), transaction);

            await _fundRepository.AddHoldingsAsync(fund.Id, holdings, transaction);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }
    }
    
    private static List<ImportHolding> ParseXml(Stream stream)
    {   
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
        
        List<ImportHolding> holdings = [];
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

            holdings.Add(new ImportHolding(
                IDataImporter.Normalize(isin), 
                IDataImporter.Normalize(row.Cell(3).GetValue<string>()),
                IDataImporter.Normalize(row.Cell(4).GetValue<string>()),
                weight
            ));
        }
        
        return holdings;
    }
    
    private async Task<Stream> GetStreamAsync(string url)
    {   
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var stream = new MemoryStream();
        await response.Content.CopyToAsync(stream);
        stream.Position = 0;
        return stream;
    }
}
