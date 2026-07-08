namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Representa el desglose de un impuesto aplicado sobre un monto base.
/// </summary>
public sealed class TaxResult
{
    /// <summary>
    /// Código del impuesto aplicado.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Nombre del impuesto aplicado.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Monto base sobre el que se calculó el impuesto.
    /// </summary>
    public Money BaseAmount { get; }

    /// <summary>
    /// Monto del impuesto calculado.
    /// </summary>
    public Money TaxAmount { get; }

    /// <summary>
    /// Indica si el impuesto estaba incluido en el monto base.
    /// </summary>
    public bool Included { get; }

    public TaxResult(string code, string name, Money baseAmount, Money taxAmount, bool included)
    {
        Code = code;
        Name = name;
        BaseAmount = baseAmount;
        TaxAmount = taxAmount;
        Included = included;
    }

    /// <summary>
    /// Total resultante: base + impuesto si no está incluido; base si está incluido.
    /// </summary>
    public Money Total => Included ? BaseAmount : BaseAmount + TaxAmount;
}
