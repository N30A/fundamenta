namespace Fundamenta.Models;

public class Fund
{
    public int Id { get; set; }
    public string Isin { get; set; }
    public string Name { get; set; }
    public string Currency { get; set; }
    public string? HoldingsUrl { get; set; }
}
