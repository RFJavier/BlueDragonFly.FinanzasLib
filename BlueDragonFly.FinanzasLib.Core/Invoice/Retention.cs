namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Representa una retención aplicada sobre el total de la factura.
/// </summary>
public sealed class Retention
{
    /// <summary>
    /// Código opcional de la retención.
    /// </summary>
    public string? Code { get; }

    /// <summary>
    /// Nombre descriptivo de la retención.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Tipo de retención.
    /// </summary>
    public RetentionType Type { get; }

    /// <summary>
    /// Valor de la retención.
    /// Para <see cref="RetentionType.Percentage"/> es un valor entre 0 y 1.
    /// Para <see cref="RetentionType.FixedAmount"/> es un monto fijo.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Modo de redondeo aplicado al calcular el monto de la retención.
    /// </summary>
    public RoundingMode Rounding { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Retention"/>.
    /// </summary>
    /// <param name="name">Nombre descriptivo de la retención.</param>
    /// <param name="type">Tipo de retención.</param>
    /// <param name="value">Valor de la retención.</param>
    /// <param name="code">Código opcional de la retención.</param>
    /// <param name="rounding">Modo de redondeo. Por defecto <see cref="RoundingMode.HalfUp"/>.</param>
    public Retention(string name, RetentionType type, decimal value, string? code = null, RoundingMode rounding = RoundingMode.HalfUp)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la retención no puede estar vacío.", nameof(name));
        }

        if (type == RetentionType.Percentage && (value < 0m || value > 1m))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "El porcentaje de retención debe estar entre 0 y 1.");
        }

        if (type == RetentionType.FixedAmount && value < 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "El monto fijo de retención no puede ser negativo.");
        }

        Name = name;
        Type = type;
        Value = value;
        Code = code;
        Rounding = rounding;
    }

    /// <summary>
    /// Calcula el monto de la retención sobre una cantidad base.
    /// </summary>
    public Money CalculateAmount(Money baseAmount)
    {
        return Type switch
        {
            RetentionType.Percentage => Money.Multiply(baseAmount, Value, Rounding),
            RetentionType.FixedAmount => Money.FromDecimal(Value, Rounding),
            _ => throw new NotSupportedException($"Tipo de retención no soportado: {Type}.")
        };
    }
}
