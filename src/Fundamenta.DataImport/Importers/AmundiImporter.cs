// using System.Text;
// using System.Text.Json;
// using Fundamenta.DataImport.Interfaces;
// using Fundamenta.Models;
//
// namespace Fundamenta.DataImport.Importers;
//
// using System.Text.Json.Serialization;
//
// public record AmundiCompositionResponse(
//     [property: JsonPropertyName("products")]
//     AmundiProduct[] Products
// );
//
// public record AmundiProduct(
//     [property: JsonPropertyName("productId")]
//     string ProductId,
//
//     [property: JsonPropertyName("composition")]
//     AmundiComposition Composition
// );
//
// public record AmundiComposition(
//     [property: JsonPropertyName("compositionData")]
//     AmundiCompositionData[] CompositionData
// );
//
// public record AmundiCompositionData(
//     [property: JsonPropertyName("compositionCharacteristics")]
//     AmundiInstrument? Instrument
// );
//
// public record AmundiInstrument(
//     [property: JsonPropertyName("isin")]
//     string? ISIN,
//
//     [property: JsonPropertyName("name")]
//     string? Name,
//
//     [property: JsonPropertyName("weight")]
//     decimal Weight,
//
//     [property: JsonPropertyName("quantity")]
//     decimal? Quantity,
//
//     [property: JsonPropertyName("currency")]
//     string? Currency
// );
//
//
// public class AmundiImporter : IDataImporter
// {
//     private readonly HttpClient _httpClient;
//     private const string Endpoint = "https://www.amundietf.se/mapi/ProductAPI/getProductsData";
//     
//     public AmundiImporter(HttpClient httpClient)
//     {
//         _httpClient = httpClient;
//     }
//
//     public async Task ImportAsync(string url)
//     {
//         string productId = url[(url.LastIndexOf('/') + 1)..].ToUpper();
//         var requestBody = new
//         {
//             productIds = new[] { productId },
//             composition = new
//             {
//                 compositionFields = new[] { "isin", "name", "weight", "quantity", "currency" }
//             }
//         };
//
//         var response = await _httpClient.PostAsync(Endpoint, new StringContent(
//             JsonSerializer.Serialize(requestBody),
//             Encoding.UTF8,
//             "application/json"
//         ));
//         response.EnsureSuccessStatusCode();
//
//         var json = await JsonSerializer.DeserializeAsync<AmundiCompositionResponse>(
//             await response.Content.ReadAsStreamAsync());
//         var data = json!.Products.SingleOrDefault()!.Composition.CompositionData
//             .Where(x => !string.IsNullOrWhiteSpace(x.Instrument?.ISIN));
//         
//         List<InstrumentHolding> holdings = [];
//         foreach (var holding in data)
//         {   
//             holdings.Add(new InstrumentHolding
//             {
//                 Instrument = new Instrument
//                 {
//                     Isin = holding.Instrument!.ISIN!.ToUpper(),
//                     Name = holding.Instrument.Name!.ToUpper(),
//                     Currency = holding.Instrument.Currency!.ToUpper(),
//                 },
//                 Weight = holding.Instrument.Weight
//             });
//         }
//         
//         return holdings;
//     }
// }
