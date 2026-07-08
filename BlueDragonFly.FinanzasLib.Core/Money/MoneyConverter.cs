namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Convierte entre representaciones decimal y centavos de forma segura y exacta.
/// </summary>
public static class MoneyConverter
{
    private const decimal CentsPerUnit = 100m;

    /// <summary>
    /// Convierte un valor decimal a centavos usando el modo de redondeo indicado.
    /// </summary>
    /// <param name="value">Valor decimal a convertir.</param>
    /// <param name="mode">Modo de redondeo. Por defecto <see cref="RoundingMode.HalfUp"/>.</param>
    /// <returns>Cantidad de centavos como <see cref="long"/>.</returns>
    public static long DecimalToCents(decimal value, RoundingMode mode = RoundingMode.HalfUp)
    {
        decimal rounded = Rounding.RoundToCents(value, mode);
        return (long)(rounded * CentsPerUnit);
    }

    /// <summary>
    /// Convierte una cantidad de centavos a su representación decimal.
    /// </summary>
    /// <param name="cents">Cantidad de centavos.</param>
    /// <returns>Valor decimal con dos decimales.</returns>
    public static decimal CentsToDecimal(long cents)
    {
        return cents / CentsPerUnit;
    }
}
