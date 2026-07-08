namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Modelo de entrada para calcular una factura.
/// </summary>
public sealed class InvoiceModel
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
    /// Líneas de la factura.
    /// </summary>
    public IReadOnlyList<InvoiceLine> Lines { get; }

    /// <summary>
    /// Descuentos globales aplicados al subtotal.
    /// </summary>
    public IReadOnlyList<Discount> GlobalDiscounts { get; }

    /// <summary>
    /// Retenciones aplicadas al total.
    /// </summary>
    public IReadOnlyList<Retention> Retentions { get; }

    public InvoiceModel(
        string number,
        DateTime date,
        IEnumerable<InvoiceLine> lines,
        IEnumerable<Discount>? globalDiscounts = null,
        IEnumerable<Retention>? retentions = null)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("El número de factura no puede estar vacío.", nameof(number));
        }

        Number = number;
        Date = date;
        Lines = (lines ?? Enumerable.Empty<InvoiceLine>()).ToList().AsReadOnly();
        GlobalDiscounts = (globalDiscounts ?? Enumerable.Empty<Discount>()).ToList().AsReadOnly();
        Retentions = (retentions ?? Enumerable.Empty<Retention>()).ToList().AsReadOnly();
    }
}
