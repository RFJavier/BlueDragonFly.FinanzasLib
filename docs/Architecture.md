# Arquitectura de BlueDragonFly.FinanzasLib.Core

Este documento describe las decisiones de diseño, la estructura de capas y los flujos de cálculo de la biblioteca.

## Principios de diseño

1. **Exactitud decimal**: todas las cantidades monetarias se almacenan internamente como centavos (`long`) para evitar errores de redondeo de punto flotante.
2. **Inmutabilidad**: `Money` es un `readonly record struct`. Cada operación devuelve una nueva instancia.
3. **Extensibilidad**: el sistema de impuestos se basa en abstracciones (`ITaxCatalog`) que permiten agregar proveedores sin modificar el calculador.
4. **Configurabilidad**: el modo de redondeo es explícito y configurable, siendo `HalfUp` el predeterminado por ser el más común en finanzas.

## Representación interna del dinero

```csharp
public readonly record struct Money
{
    public long Cents { get; }
    public decimal Value => Cents / 100m;
}
```

- `Cents` almacena la cantidad entera de centavos.
- `Value` expone el valor decimal con dos decimales.
- La conversión de `decimal` a centavos pasa por `MoneyConverter`, que redondea según el modo indicado.

## Capas del sistema

### 1. Money

Ubicación: `BlueDragonFly.FinanzasLib.Core/Money/`

Responsabilidades:

- Representar cantidades monetarias.
- Proveer operaciones aritméticas, comparaciones y formato.
- Calcular totales de venta y descuentos.

Archivos principales:

| Archivo | Responsabilidad |
|---------|-----------------|
| `Money.cs` | Tipo principal de valor monetario. |
| `MoneyConverter.cs` | Conversión entre `decimal` y centavos. |
| `Rounding.cs` | Estrategias de redondeo. |

### 2. Impuestos

Ubicación: `BlueDragonFly.FinanzasLib.Core/Tax/`

Responsabilidades:

- Definir reglas de impuesto (`TaxRule`).
- Calcular impuestos individuales y agrupados.
- Proveer catálogos de impuestos (`ITaxCatalog`).

Archivos principales:

| Archivo | Responsabilidad |
|---------|-----------------|
| `TaxRule.cs` | Regla individual de impuesto. |
| `TaxCalculator.cs` | Cálculo de impuestos sobre un monto. |
| `TaxResult.cs` | Resultado de un impuesto individual. |
| `TaxCalculation.cs` | Resultado agregado de múltiples impuestos. |
| `ITaxCatalog.cs` / `InMemoryTaxCatalog.cs` | Abstracción e implementación de catálogo. |
| `Json/*.cs` | Carga de catálogos desde JSON. |

#### Flujo de cálculo de impuestos

```
Base (Money)
    │
    ├──► TaxRule.Percentage ──► Base × Rate
    │
    └──► TaxRule.Fixed ───────► Rate (monto fijo)
```

El resultado se redondea según `TaxRule.Rounding`. Si el impuesto está incluido (`Included = true`), el monto base no se incrementa en el total.

### 3. Facturación

Ubicación: `BlueDragonFly.FinanzasLib.Core/Invoice/`

Responsabilidades:

- Modelar una factura antes y después de calcularla.
- Aplicar descuentos por línea y globales.
- Calcular impuestos por línea y retenciones sobre el total.

Archivos principales:

| Archivo | Responsabilidad |
|---------|-----------------|
| `InvoiceModel.cs` | Modelo de entrada. |
| `InvoiceLine.cs` | Línea sin calcular. |
| `InvoiceLineResult.cs` | Línea calculada. |
| `Invoice.cs` | Factura calculada con totales. |
| `InvoiceCalculator.cs` | Orquestador del cálculo. |
| `Discount.cs` / `DiscountType.cs` | Descuentos porcentuales y fijos. |
| `Retention.cs` / `RetentionType.cs` | Retenciones porcentuales y fijas. |

#### Flujo de cálculo de una factura

```
1. Por cada línea:
   Gross = quantity × unitPrice
   LineDiscounts = descuentos de línea sobre Gross
   Net = Gross - LineDiscounts
   Taxes = impuestos sobre Net

2. Subtotales:
   SubTotal      = Σ Gross
   LineDiscounts = Σ LineDiscounts
   SubTotalNet   = Σ Net
   TotalTaxes    = Σ Taxes

3. Descuentos globales:
   GlobalDiscounts = descuentos sobre SubTotalNet
   TaxableAmount   = SubTotalNet - GlobalDiscounts

4. Retenciones:
   TotalRetentions = retenciones sobre (TaxableAmount + TotalTaxes)

5. Total:
   Total = TaxableAmount + TotalTaxes - TotalRetentions
```

## Extensibilidad

### Nuevos catálogos de impuestos

Para agregar un proveedor de impuestos personalizado (base de datos, API, etc.), implementa `ITaxCatalog`:

```csharp
public interface ITaxCatalog
{
    TaxRule Get(string code);
}
```

No es necesario modificar `TaxCalculator` ni `InvoiceCalculator`.

### Nuevos modos de redondeo

Agregar un valor al enum `RoundingMode` y su implementación en `Rounding.Round` es suficiente.

## Dependencias

La biblioteca no utiliza paquetes NuGet de terceros. Sus únicas dependencias son:

- .NET 10 Base Class Library.
- `System.Text.Json` para la carga de catálogos desde JSON (incluido en .NET).
- `System.Globalization` para formateo de moneda (incluido en .NET).

## Publicación futura

El proyecto está preparado para generar un archivo XML de documentación de la API (`GenerateDocumentationFile`). Este archivo puede usarse con herramientas como DocFX o Sandcastle para generar sitios de documentación cuando se publique el paquete en NuGet.
