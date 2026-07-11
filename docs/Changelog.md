# Changelog

Todas las versiones notables de `BlueDragonFly.FinanzasLib` se documentan en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto adhiere a [Semantic Versioning](https://semver.org/lang/es/).

## [1.0.2] - 2026-07-10

### Added

- Primera publicación oficial del paquete en NuGet.org.
- Carpeta `docs` incluida dentro del paquete NuGet.
- Archivo `README.md` incluido dentro del paquete NuGet.
- Licencia MIT mediante `PackageLicenseExpression`.
- Metadatos completos del paquete:
  - `PackageId`
  - `Version`
  - `Authors`
  - `Company`
  - `Description`
  - `PackageTags`
  - `PackageProjectUrl`
  - `RepositoryUrl`
  - `RepositoryType`
  - `PackageReleaseNotes`

### Changed

- Se actualizó el `README.md` para reflejar el estado actual del proyecto.
- Se eliminaron referencias indicando que el paquete aún no estaba publicado en NuGet.
- Se mejoró la presentación y documentación del paquete para su primera publicación oficial.

## [1.0.1] - 2026-07-10

### Added

- Comentarios XML de documentación en los modelos financieros principales (`Discount`, `Invoice`, `InvoiceLine`, `InvoiceLineResult`, `InvoiceModel`, `Retention`, `TaxCalculation`, `TaxResult`, `TaxRule`, entre otros).

### Changed

- `README.md` ampliado con badges de licencia y versión de .NET, y enlaces a la documentación detallada (`Core.md`, `Architecture.md`, `Changelog.md`).

### Removed

- Eliminado el workflow de GitHub Actions de CodeQL.

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
