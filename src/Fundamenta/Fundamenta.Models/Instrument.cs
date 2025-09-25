namespace Fundamenta.Models;

public class Instrument
{
    public int Id { get; set; }
    public string ISIN { get; set; }
    public string Name { get; set; }
    public string BaseCurrency { get; set; }
    public string? Provider { get; set; }
}
