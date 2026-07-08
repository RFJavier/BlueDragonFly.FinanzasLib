namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Tipos de retención soportados.
/// </summary>
public enum RetentionType
{
    /// <summary>
    /// Retención porcentual sobre el monto base.
    /// </summary>
    Percentage,

    /// <summary>
    /// Retención con monto fijo.
    /// </summary>
    FixedAmount
}
