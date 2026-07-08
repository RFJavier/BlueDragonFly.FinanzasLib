using System.Text.Json.Serialization;

namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Modelo de intercambio para la carga de catálogos de impuestos desde JSON.
/// </summary>
public sealed class JsonTaxCatalog
{
    /// <summary>
    /// Lista de impuestos definidos en el catálogo.
    /// </summary>
    [JsonPropertyName("taxes")]
    public List<JsonTaxRule> Taxes { get; set; } = new();
}
