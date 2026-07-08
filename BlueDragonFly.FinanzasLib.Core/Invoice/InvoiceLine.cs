namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Línea de factura sin calcular.
/// </summary>
public sealed class InvoiceLine
{
    /// <summary>
    /// Código del ítem.
    /// </summary>
    public string? ItemCode { get; }

    /// <summary>
    /// Descripción del ítem.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Cantidad de ítems.
    /// </summary>
    public long Quantity { get; }

    /// <summary>
    /// Precio unitario.
    /// </summary>
    public Money UnitPrice { get; }

    /// <summary>
    /// Descuentos aplicados a la línea.
    /// </summary>
    public IReadOnlyList<Discount> Discounts { get; }

    /// <summary>
    /// Códigos de impuestos a aplicar, según el catálogo.
    /// </summary>
    public IReadOnlyList<string> TaxCodes { get; }

    public InvoiceLine(
        string description,
        long quantity,
        Money unitPrice,
        IEnumerable<Discount>? discounts = null,
        IEnumerable<string>? taxCodes = null,
        string? itemCode = null)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("La descripción de la línea no puede estar vacía.", nameof(description));
        }

        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad no puede ser negativa.");
        }

        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discounts = (discounts ?? Enumerable.Empty<Discount>()).ToList().AsReadOnly();
        TaxCodes = (taxCodes ?? Enumerable.Empty<string>()).ToList().AsReadOnly();
        ItemCode = itemCode;
    }
}
