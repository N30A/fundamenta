namespace Fundamenta.Models;

public class InstrumentHolding
{
    public Instrument Instrument { get; set; }
    public decimal Weight { get; set; }
    public long? Shares { get; set; }
}
