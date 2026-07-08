namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Abstracción de un catálogo de impuestos.
/// Permite agregar nuevos impuestos sin modificar el calculador.
/// </summary>
public interface ITaxCatalog
{
    /// <summary>
    /// Obtiene una regla de impuesto por su código.
    /// </summary>
    /// <param name="code">Código del impuesto.</param>
    /// <returns>La regla correspondiente.</returns>
    /// <exception cref="KeyNotFoundException">Si el código no existe en el catálogo.</exception>
    TaxRule Get(string code);

    /// <summary>
    /// Obtiene todas las reglas del catálogo.
    /// </summary>
    IEnumerable<TaxRule> GetAll();
}
