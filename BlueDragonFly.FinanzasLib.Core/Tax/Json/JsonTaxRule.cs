using System.Text.Json.Serialization;

namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Representación JSON de una regla de impuesto.
/// </summary>
public sealed class JsonTaxRule
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = "percentage";

    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    [JsonPropertyName("included")]
    public bool Included { get; set; }

    [JsonPropertyName("rounding")]
    public string Rounding { get; set; } = "half_up";
}
