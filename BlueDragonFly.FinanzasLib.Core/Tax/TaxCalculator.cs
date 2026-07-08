namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Calcula impuestos aplicando reglas de un catálogo.
/// </summary>
public static class TaxCalculator
{
    /// <summary>
    /// Calcula un único impuesto sobre un monto base.
    /// </summary>
    public static TaxResult Calculate(Money baseAmount, TaxRule rule)
    {
        Money taxAmount = rule.Type switch
        {
            TaxType.Percentage => Money.Multiply(baseAmount, rule.Rate, rule.Rounding),
            TaxType.Fixed => Money.FromDecimal(rule.Rate, rule.Rounding),
            _ => throw new NotSupportedException($"Tipo de impuesto no soportado: {rule.Type}.")
        };

        return new TaxResult(rule.Code, rule.Name, baseAmount, taxAmount, rule.Included);
    }

    /// <summary>
    /// Calcula múltiples impuestos sobre un monto base.
    /// </summary>
    public static TaxCalculation Calculate(Money baseAmount, IEnumerable<TaxRule> rules)
    {
        var details = rules.Select(r => Calculate(baseAmount, r)).ToList();
        return new TaxCalculation(baseAmount, details);
    }

    /// <summary>
    /// Calcula impuestos buscando las reglas por código en el catálogo.
    /// </summary>
    public static TaxCalculation Calculate(Money baseAmount, ITaxCatalog catalog, params string[] codes)
    {
        var rules = codes.Select(catalog.Get).ToList();
        return Calculate(baseAmount, rules);
    }
}
