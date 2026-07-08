namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Tipos de descuento soportados.
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// Descuento porcentual sobre el monto base.
    /// </summary>
    Percentage,

    /// <summary>
    /// Descuento con monto fijo.
    /// </summary>
    FixedAmount
}
