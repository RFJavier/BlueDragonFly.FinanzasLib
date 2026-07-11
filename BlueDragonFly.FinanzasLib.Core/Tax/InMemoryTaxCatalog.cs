namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Catálogo de impuestos en memoria.
/// </summary>
public sealed class InMemoryTaxCatalog : ITaxCatalog
{
    private readonly Dictionary<string, TaxRule> _rules;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InMemoryTaxCatalog"/>.
    /// </summary>
    /// <param name="rules">Reglas de impuesto que conforman el catálogo.</param>
    public InMemoryTaxCatalog(IEnumerable<TaxRule> rules)
    {
        _rules = rules.ToDictionary(r => r.Code, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Obtiene una regla de impuesto por su código.
    /// </summary>
    /// <param name="code">Código del impuesto.</param>
    /// <returns>La regla de impuesto correspondiente.</returns>
    /// <exception cref="KeyNotFoundException">Se produce cuando el código no existe en el catálogo.</exception>
    public TaxRule Get(string code)
    {
        if (_rules.TryGetValue(code, out var rule))
        {
            return rule;
        }

        throw new KeyNotFoundException($"No se encontró el impuesto con código '{code}' en el catálogo.");
    }

    /// <summary>
    /// Obtiene todas las reglas del catálogo.
    /// </summary>
    /// <returns>Colección de reglas de impuesto.</returns>
    public IEnumerable<TaxRule> GetAll()
    {
        return _rules.Values;
    }
}
