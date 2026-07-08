# Changelog

Todas las versiones notables de `BlueDragonFly.FinanzasLib` se documentan en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto adhiere a [Semantic Versioning](https://semver.org/lang/es/).

## [1.0.0] - 2026-07-08

### Added

- Tipo `Money` para representar cantidades monetarias con exactitud de centavos.
- Conversión segura entre `decimal` y centavos mediante `MoneyConverter`.
- Estrategias de redondeo: `ToEven`, `HalfUp` (predeterminado), `Down` y `Up`.
- Sistema de impuestos extensible basado en `TaxRule`, `TaxCalculator` y `TaxCalculation`.
- Soporte para impuestos porcentuales y de monto fijo, incluidos o no en la base.
- Abstracción `ITaxCatalog` con implementaciones en memoria (`InMemoryTaxCatalog`) y desde JSON (`JsonTaxCatalogProvider`).
- Modelo de facturación con líneas, descuentos de línea, descuentos globales, impuestos y retenciones.
- `InvoiceCalculator` para calcular facturas completas siguiendo un flujo determinado.
- Proyecto de demostración (`BlueDragonFly.FinanzasLib.Demo`) con ejemplos de uso.
- Proyecto de pruebas (`BlueDragonFly.FinanzasLib.Tests`) con escenarios de validación.
- Documentación de la API en comentarios XML habilitada para IntelliSense.
- Archivos `LICENSE` (MIT) y `README.md`.
