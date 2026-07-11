using System.Text.Json.Serialization;

namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Representación JSON de una regla de impuesto.
/// </summary>
public sealed class JsonTaxRule
{
    /// <summary>
    /// Código del impuesto.
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del impuesto.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de impuesto (por ejemplo, "percentage" o "fixed").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "percentage";

    /// <summary>
    /// Tasa o monto del impuesto.
    /// </summary>
    [JsonPropertyName("rate")]
    public decimal Rate { get; set; }

    /// <summary>
    /// Indica si el impuesto está incluido en el monto base.
    /// </summary>
    [JsonPropertyName("included")]
    public bool Included { get; set; }

    /// <summary>
    /// Modo de redondeo aplicado al impuesto.
    /// </summary>
    [JsonPropertyName("rounding")]
    public string Rounding { get; set; } = "half_up";
}
