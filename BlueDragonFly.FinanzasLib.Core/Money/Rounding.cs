namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Estrategias de redondeo para conversiones entre <see cref="decimal"/> y centavos.
/// </summary>
public enum RoundingMode
{
    /// <summary>
    /// Redondeo al par más cercano (redondeo bancario, predeterminado en .NET).
    /// </summary>
    ToEven,

    /// <summary>
    /// Redondeo medio hacia arriba (0.5 siempre hacia arriba). Común en finanzas.
    /// </summary>
    HalfUp,

    /// <summary>
    /// Redondeo hacia abajo (truncamiento).
    /// </summary>
    Down,

    /// <summary>
    /// Redondeo hacia arriba.
    /// </summary>
    Up
}

/// <summary>
/// Utilidades de redondeo para operaciones monetarias.
/// </summary>
public static class Rounding
{
    /// <summary>
    /// Redondea un valor decimal a la cantidad de decimales indicada usando el modo especificado.
    /// </summary>
    public static decimal Round(decimal value, int decimals, RoundingMode mode)
    {
        return mode switch
        {
            RoundingMode.ToEven => decimal.Round(value, decimals, MidpointRounding.ToEven),
            RoundingMode.HalfUp => decimal.Round(value, decimals, MidpointRounding.AwayFromZero),
            RoundingMode.Down => Math.Truncate(value * (decimal)Math.Pow(10, decimals)) / (decimal)Math.Pow(10, decimals),
            RoundingMode.Up => Math.Ceiling(value * (decimal)Math.Pow(10, decimals)) / (decimal)Math.Pow(10, decimals),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    /// <summary>
    /// Redondea un valor decimal a 2 decimales usando el modo especificado.
    /// </summary>
    public static decimal RoundToCents(decimal value, RoundingMode mode)
    {
        return Round(value, 2, mode);
    }
}
