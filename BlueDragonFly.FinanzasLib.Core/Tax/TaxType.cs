namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Tipos de impuesto soportados por el calculador.
/// </summary>
public enum TaxType
{
    /// <summary>
    /// Impuesto calculado como porcentaje del monto base.
    /// </summary>
    Percentage,

    /// <summary>
    /// Impuesto con monto fijo.
    /// </summary>
    Fixed
}
