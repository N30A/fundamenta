using Fundamenta.Models;

namespace Fundamenta.DataImport.Parsers;

public interface IInstrumentParser
{
    Task<IEnumerable<InstrumentHolding>> ParseAsync(string url);
}
