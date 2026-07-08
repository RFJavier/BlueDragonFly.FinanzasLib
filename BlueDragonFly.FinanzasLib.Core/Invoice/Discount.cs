namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Representa un descuento aplicable a una línea o a toda la factura.
/// </summary>
public sealed class Discount
{
    /// <summary>
    /// Código opcional del descuento.
    /// </summary>
    public string? Code { get; }

    /// <summary>
    /// Nombre descriptivo del descuento.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Tipo de descuento.
    /// </summary>
    public DiscountType Type { get; }

    /// <summary>
    /// Valor del descuento.
    /// Para <see cref="DiscountType.Percentage"/> es un valor entre 0 y 1.
    /// Para <see cref="DiscountType.FixedAmount"/> es un monto fijo.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Modo de redondeo aplicado al calcular el monto del descuento.
    /// </summary>
    public RoundingMode Rounding { get; }

    public Discount(string name, DiscountType type, decimal value, string? code = null, RoundingMode rounding = RoundingMode.HalfUp)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del descuento no puede estar vacío.", nameof(name));
        }

        if (type == DiscountType.Percentage && (value < 0m || value > 1m))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "El porcentaje de descuento debe estar entre 0 y 1.");
        }

        if (type == DiscountType.FixedAmount && value < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "El monto fijo de descuento no puede ser negativo.");
        }

        Name = name;
        Type = type;
        Value = value;
        Code = code;
        Rounding = rounding;
    }

    /// <summary>
    /// Calcula el monto del descuento sobre una cantidad base.
    /// </summary>
    public Money CalculateAmount(Money baseAmount)
    {
        return Type switch
        {
            DiscountType.Percentage => Money.Multiply(baseAmount, Value, Rounding),
            DiscountType.FixedAmount => Money.FromDecimal(Value, Rounding),
            _ => throw new NotSupportedException($"Tipo de descuento no soportado: {Type}.")
        };
    }
}
