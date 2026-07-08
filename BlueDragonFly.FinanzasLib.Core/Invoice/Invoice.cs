namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Factura calculada con todos sus totales.
/// </summary>
public sealed class Invoice
{
    /// <summary>
    /// Número de factura.
    /// </summary>
    public string Number { get; }

    /// <summary>
    /// Fecha de emisión.
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Líneas calculadas.
    /// </summary>
    public IReadOnlyList<InvoiceLineResult> Lines { get; }

    /// <summary>
    /// Suma de montos brutos de todas las líneas.
    /// </summary>
    public Money SubTotal { get; }

    /// <summary>
    /// Suma de descuentos de línea.
    /// </summary>
    public Money LineDiscounts { get; }

    /// <summary>
    /// Subtotal menos descuentos de línea.
    /// </summary>
    public Money SubTotalNet { get; }

    /// <summary>
    /// Suma de descuentos globales.
    /// </summary>
    public Money GlobalDiscounts { get; }

    /// <summary>
    /// Monto gravable: subtotal neto menos descuentos globales.
    /// </summary>
    public Money TaxableAmount { get; }

    /// <summary>
    /// Suma de todos los impuestos no incluidos.
    /// </summary>
    public Money TotalTaxes { get; }

    /// <summary>
    /// Suma de todas las retenciones.
    /// </summary>
    public Money TotalRetentions { get; }

    /// <summary>
    /// Total a pagar: gravable + impuestos - retenciones.
    /// </summary>
    public Money Total { get; }

    public Invoice(
        string number,
        DateTime date,
        IEnumerable<InvoiceLineResult> lines,
        Money subTotal,
        Money lineDiscounts,
        Money subTotalNet,
        Money globalDiscounts,
        Money taxableAmount,
        Money totalTaxes,
        Money totalRetentions,
        Money total)
    {
        Number = number;
        Date = date;
        Lines = lines.ToList().AsReadOnly();
        SubTotal = subTotal;
        LineDiscounts = lineDiscounts;
        SubTotalNet = subTotalNet;
        GlobalDiscounts = globalDiscounts;
        TaxableAmount = taxableAmount;
        TotalTaxes = totalTaxes;
        TotalRetentions = totalRetentions;
        Total = total;
    }
}
