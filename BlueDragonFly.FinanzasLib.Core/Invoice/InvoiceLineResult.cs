namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Línea de factura calculada.
/// </summary>
public sealed class InvoiceLineResult
{
    /// <summary>
    /// Línea de entrada.
    /// </summary>
    public InvoiceLine Input { get; }

    /// <summary>
    /// Monto bruto: cantidad × precio unitario.
    /// </summary>
    public Money GrossAmount { get; }

    /// <summary>
    /// Total de descuentos aplicados a la línea.
    /// </summary>
    public Money DiscountAmount { get; }

    /// <summary>
    /// Monto neto después de descuentos.
    /// </summary>
    public Money NetAmount { get; }

    /// <summary>
    /// Impuestos calculados sobre el monto neto.
    /// </summary>
    public TaxCalculation Taxes { get; }

    /// <summary>
    /// Total de la línea: neto + impuestos no incluidos.
    /// </summary>
    public Money Total { get; }

    public InvoiceLineResult(
        InvoiceLine input,
        Money grossAmount,
        Money discountAmount,
        Money netAmount,
        TaxCalculation taxes,
        Money total)
    {
        Input = input;
        GrossAmount = grossAmount;
        DiscountAmount = discountAmount;
        NetAmount = netAmount;
        Taxes = taxes;
        Total = total;
    }
}
