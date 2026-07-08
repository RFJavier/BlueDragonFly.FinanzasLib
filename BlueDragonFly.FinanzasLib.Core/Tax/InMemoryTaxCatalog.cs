namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Catálogo de impuestos en memoria.
/// </summary>
public sealed class InMemoryTaxCatalog : ITaxCatalog
{
    private readonly Dictionary<string, TaxRule> _rules;

    public InMemoryTaxCatalog(IEnumerable<TaxRule> rules)
    {
        _rules = rules.ToDictionary(r => r.Code, StringComparer.OrdinalIgnoreCase);
    }

    public TaxRule Get(string code)
    {
        if (_rules.TryGetValue(code, out var rule))
        {
            return rule;
        }

        throw new KeyNotFoundException($"No se encontró el impuesto con código '{code}' en el catálogo.");
    }

    public IEnumerable<TaxRule> GetAll()
    {
        return _rules.Values;
    }
}
