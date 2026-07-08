namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Define una regla de impuesto individual.
/// </summary>
public sealed class TaxRule
{
    /// <summary>
    /// Código único del impuesto (por ejemplo, "IVA13").
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Nombre descriptivo del impuesto.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Tipo de impuesto.
    /// </summary>
    public TaxType Type { get; }

    /// <summary>
    /// Tasa o monto del impuesto.
    /// Para <see cref="TaxType.Percentage"/> es un valor entre 0 y 1.
    /// Para <see cref="TaxType.Fixed"/> es un monto fijo en la moneda base.
    /// </summary>
    public decimal Rate { get; }

    /// <summary>
    /// Indica si el impuesto está incluido en el monto base.
    /// </summary>
    public bool Included { get; }

    /// <summary>
    /// Modo de redondeo aplicado al resultado del impuesto.
    /// </summary>
    public RoundingMode Rounding { get; }

    public TaxRule(string code, string name, TaxType type, decimal rate, bool included = false, RoundingMode rounding = RoundingMode.HalfUp)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("El código del impuesto no puede estar vacío.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del impuesto no puede estar vacío.", nameof(name));
        }

        if (type == TaxType.Percentage && (rate < 0m || rate > 1m))
        {
            throw new ArgumentOutOfRangeException(nameof(rate), "La tasa porcentual debe estar entre 0 y 1.");
        }

        if (type == TaxType.Fixed && rate < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(rate), "El monto fijo no puede ser negativo.");
        }

        Code = code;
        Name = name;
        Type = type;
        Rate = rate;
        Included = included;
        Rounding = rounding;
    }

    /// <summary>
    /// Crea una copia de la regla con el modo de redondeo especificado.
    /// </summary>
    public TaxRule WithRounding(RoundingMode rounding)
    {
        return new TaxRule(Code, Name, Type, Rate, Included, rounding);
    }
}
