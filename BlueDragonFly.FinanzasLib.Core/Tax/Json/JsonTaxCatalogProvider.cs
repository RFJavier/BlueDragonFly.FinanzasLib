using System.Text.Json;

namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Proveedor de catálogos de impuestos basado en archivos JSON.
/// </summary>
public static class JsonTaxCatalogProvider
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Carga un catálogo de impuestos desde un archivo JSON.
    /// </summary>
    /// <param name="path">Ruta del archivo JSON.</param>
    /// <returns>Un catálogo de impuestos listo para usar.</returns>
    public static ITaxCatalog Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("No se encontró el archivo de catálogo de impuestos.", path);
        }

        string json = File.ReadAllText(path);
        return LoadFromJson(json);
    }

    /// <summary>
    /// Carga un catálogo de impuestos desde una cadena JSON.
    /// </summary>
    /// <param name="json">Contenido JSON.</param>
    /// <returns>Un catálogo de impuestos listo para usar.</returns>
    public static ITaxCatalog LoadFromJson(string json)
    {
        var catalog = JsonSerializer.Deserialize<JsonTaxCatalog>(json, Options)
            ?? throw new InvalidOperationException("El contenido JSON del catálogo no es válido.");

        var rules = catalog.Taxes.Select(ToTaxRule);
        return new InMemoryTaxCatalog(rules);
    }

    private static TaxRule ToTaxRule(JsonTaxRule json)
    {
        var type = ParseTaxType(json.Type);
        var rounding = ParseRoundingMode(json.Rounding);
        return new TaxRule(json.Code, json.Name, type, json.Rate, json.Included, rounding);
    }

    private static TaxType ParseTaxType(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "percentage" => TaxType.Percentage,
            "fixed" => TaxType.Fixed,
            _ => throw new NotSupportedException($"Tipo de impuesto no soportado: '{value}'.")
        };
    }

    private static RoundingMode ParseRoundingMode(string value)
    {
        return value.ToLowerInvariant() switch
        {
            "half_up" => RoundingMode.HalfUp,
            "to_even" or "bankers" => RoundingMode.ToEven,
            "down" or "truncate" => RoundingMode.Down,
            "up" => RoundingMode.Up,
            "away_from_zero" => RoundingMode.HalfUp,
            _ => throw new NotSupportedException($"Modo de redondeo no soportado: '{value}'.")
        };
    }
}
