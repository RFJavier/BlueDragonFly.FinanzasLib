namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Resultado completo del cálculo de impuestos sobre un monto base.
/// </summary>
public sealed class TaxCalculation
{
    /// <summary>
    /// Monto base original.
    /// </summary>
    public Money BaseAmount { get; }

    /// <summary>
    /// Desglose de cada impuesto aplicado.
    /// </summary>
    public IReadOnlyList<TaxResult> Details { get; }

    /// <summary>
    /// Suma de todos los impuestos calculados.
    /// </summary>
    public Money TotalTax => Details.Aggregate(Money.Zero, (acc, d) => acc + d.TaxAmount);

    /// <summary>
    /// Total general: base + impuestos no incluidos.
    /// </summary>
    public Money GrandTotal => Details.Aggregate(BaseAmount, (acc, d) => d.Included ? acc : acc + d.TaxAmount);

    public TaxCalculation(Money baseAmount, IEnumerable<TaxResult> details)
    {
        BaseAmount = baseAmount;
        Details = details.ToList().AsReadOnly();
    }
}
