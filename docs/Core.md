# BlueDragonFly.FinanzasLib.Core

El núcleo de la biblioteca proporciona tipos y utilidades para realizar cálculos monetarios con **exactitud decimal**. En lugar de trabajar directamente con `float` o `double`, todas las operaciones se ejecutan internamente sobre **centavos** (`long`), lo que elimina los errores de redondeo propios de la aritmética de punto flotante.

## Contenido

- [Money](#money)
- [MoneyConverter](#moneyconverter)
- [Rounding](#rounding)
- [Impuestos](#impuestos)
  - [TaxRule](#taxrule)
  - [TaxCalculator](#taxcalculator)
  - [Catálogos](#catálogos)
  - [JSON](#json)
- [Facturación](#facturación)
  - [InvoiceModel](#invoicemodel)
  - [InvoiceLine](#invoiceline)
  - [Discount](#discount)
  - [Retention](#retention)
  - [InvoiceCalculator](#invoicecalculator)
- [Ejemplos de uso](#ejemplos-de-uso)
- [Referencia rápida](#referencia-rápida)

---

## Money

`Money` es un `readonly record struct` que representa una cantidad monetaria. Es inmutable y se compara por valor.

### Crear instancias

```csharp
using BlueDragonFly.FinanzasLib.Core;

// Desde un valor decimal
Money amount = Money.FromDecimal(12.35m);

// Desde centavos
Money fromCents = Money.FromCents(1235);

// Valor cero
Money zero = Money.Zero;
```

### Propiedades

```csharp
Money amount = Money.FromDecimal(12.35m);

long cents   = amount.Cents;  // 1235
decimal value = amount.Value; // 12.35m
```

### Operaciones aritméticas

```csharp
Money a = Money.FromDecimal(10.00m);
Money b = Money.FromDecimal(2.50m);

Money sum  = a + b;           // $12.50
Money diff = a - b;           // $7.50
Money mult = a * 3L;          // $30.00
Money tax  = a * 1.16m;       // $11.60
Money div  = a / 3L;          // $3.33
```

### Cálculos de ventas

Para vender una cantidad de ítems a un precio unitario, usa `CalculateTotal`. La operación es exacta porque multiplica centavos enteros.

```csharp
Money unitPrice = Money.FromDecimal(1.00m);
Money total = Money.CalculateTotal(7, unitPrice);
// total.Value == 7.00m
```

### Descuentos

```csharp
Money amount = Money.FromDecimal(100.00m);
Money discounted = Money.ApplyDiscount(amount, 0.15m);
// discounted.Value == 85.00m
```

La tasa de descuento debe estar entre `0` y `1`.

### Comparaciones

```csharp
Money a = Money.FromDecimal(10.00m);
Money b = Money.FromDecimal(10.00m);
Money c = Money.FromDecimal(20.00m);

bool eq = a == b; // true
bool lt = a < c;  // true
bool gt = a > c;  // false
```

### Formato

```csharp
Money amount = Money.FromDecimal(1234.56m);
Console.WriteLine(amount);        // $1,234.56 (según cultura actual)
Console.WriteLine(amount.Value);  // 1234.56
```

---

## MoneyConverter

Convierte de forma segura entre `decimal` y centavos.

```csharp
long cents = MoneyConverter.DecimalToCents(12.35m);
// cents == 1235

decimal value = MoneyConverter.CentsToDecimal(1235);
// value == 12.35m
```

Puedes especificar el modo de redondeo:

```csharp
long cents = MoneyConverter.DecimalToCents(0.005m, RoundingMode.HalfUp);
// cents == 1
```

---

## Rounding

Define cómo se redondean los valores decimales al convertirlos a centavos.

| Modo | Descripción |
|------|-------------|
| `RoundingMode.ToEven` | Redondeo al par más cercano (predeterminado de .NET). |
| `RoundingMode.HalfUp` | 0.5 siempre hacia arriba. **Modo predeterminado de la biblioteca.** |
| `RoundingMode.Down` | Truncamiento hacia abajo. |
| `RoundingMode.Up` | Redondeo hacia arriba. |

### Ejemplos

```csharp
Money a = Money.FromDecimal(0.005m, RoundingMode.HalfUp); // 1 centavo
Money b = Money.FromDecimal(0.005m, RoundingMode.ToEven); // 0 centavos
```

También puedes redondear directamente:

```csharp
decimal rounded = Rounding.RoundToCents(12.345m, RoundingMode.HalfUp);
// rounded == 12.35m
```

---

## Ejemplos de uso

### Venta simple

```csharp
Money unitPrice = Money.FromDecimal(9.99m);
int quantity = 5;

Money subtotal = Money.CalculateTotal(quantity, unitPrice);
Money tax = subtotal * 0.16m;
Money total = subtotal + tax;

Console.WriteLine($"Subtotal: {subtotal}");
Console.WriteLine($"Impuesto: {tax}");
Console.WriteLine($"Total:    {total}");
```

### Aplicar descuento

```csharp
Money price = Money.FromDecimal(250.00m);
Money discounted = Money.ApplyDiscount(price, 0.20m);

Console.WriteLine($"Precio original: {price}");
Console.WriteLine($"Con 20% off:     {discounted}");
```

### Dividir una cuenta

```csharp
Money bill = Money.FromDecimal(100.00m);
int people = 3;

Money perPerson = bill / people;
Console.WriteLine($"Cada uno paga: {perPerson}");
// $33.33
```

---

## Referencia rápida

| Miembro | Descripción |
|---------|-------------|
| `Money.FromDecimal(decimal, RoundingMode)` | Crea `Money` desde un decimal. |
| `Money.FromCents(long)` | Crea `Money` desde centavos. |
| `Money.Zero` | Representa `$0.00`. |
| `Money.Cents` | Obtiene la cantidad en centavos. |
| `Money.Value` | Obtiene el valor decimal. |
| `Money.Add(a, b)` | Suma dos cantidades. |
| `Money.Subtract(a, b)` | Resta dos cantidades. |
| `Money.Multiply(m, factor)` | Multiplica por `decimal` o `long`. |
| `Money.Divide(m, divisor)` | Divide por `decimal` o `long`. |
| `Money.CalculateTotal(qty, unitPrice)` | Total de una venta. |
| `Money.ApplyDiscount(amount, rate)` | Aplica un descuento. |
| `Money.Ratio(a, b)` | Cociente entre dos cantidades. |
| `MoneyConverter.DecimalToCents` | Convierte decimal a centavos. |
| `MoneyConverter.CentsToDecimal` | Convierte centavos a decimal. |
| `Rounding.Round` | Redondea a N decimales. |
| `Rounding.RoundToCents` | Redondea a 2 decimales. |
| `TaxRule` | Define una regla de impuesto. |
| `TaxCalculator.Calculate` | Calcula impuestos sobre un monto. |
| `TaxResult` | Desglose de un impuesto calculado. |
| `TaxCalculation` | Resultado completo con múltiples impuestos. |
| `ITaxCatalog` | Abstracción de catálogo de impuestos. |
| `InMemoryTaxCatalog` | Catálogo en memoria. |
| `JsonTaxCatalogProvider.Load` | Carga catálogo desde archivo JSON. |
| `JsonTaxCatalogProvider.LoadFromJson` | Carga catálogo desde cadena JSON. |
| `InvoiceModel` | Modelo de entrada de una factura. |
| `InvoiceLine` | Línea de factura sin calcular. |
| `InvoiceLineResult` | Línea de factura calculada. |
| `Invoice` | Factura calculada con totales. |
| `InvoiceCalculator.Calculate` | Calcula una factura completa. |
| `Discount` | Descuento porcentual o fijo. |
| `Retention` | Retención porcentual o fija. |

---

## Impuestos

El sistema de impuestos está diseñado para ser **extensible y desacoplado**. Puedes definir reglas en código, en memoria o cargarlas desde JSON sin modificar el calculador.

### TaxRule

Representa una regla de impuesto individual.

```csharp
var iva13 = new TaxRule(
    code: "IVA13",
    name: "IVA 13%",
    type: TaxType.Percentage,
    rate: 0.13m,
    included: false,
    rounding: RoundingMode.HalfUp);
```

| Parámetro | Descripción |
|-----------|-------------|
| `code` | Código único del impuesto. |
| `name` | Nombre descriptivo. |
| `type` | `TaxType.Percentage` o `TaxType.Fixed`. |
| `rate` | Para porcentaje: valor entre `0` y `1`. Para fijo: monto en moneda base. |
| `included` | `true` si el impuesto ya está incluido en el monto base. |
| `rounding` | Modo de redondeo aplicado al resultado. |

### TaxCalculator

Calcula impuestos sobre un monto base.

```csharp
Money subtotal = Money.FromDecimal(100.00m);
TaxResult result = TaxCalculator.Calculate(subtotal, iva13);

Console.WriteLine(result.TaxAmount); // $13.00
Console.WriteLine(result.Total);     // $113.00
```

Para múltiples impuestos:

```csharp
var rules = new[] { iva13, propina };
TaxCalculation calculation = TaxCalculator.Calculate(subtotal, rules);

Console.WriteLine(calculation.TotalTax);   // Suma de impuestos
Console.WriteLine(calculation.GrandTotal); // Total a pagar
```

### Catálogos

Un catálogo permite centralizar las reglas y buscarlas por código.

```csharp
var catalog = new InMemoryTaxCatalog(new[]
{
    new TaxRule("IVA13", "IVA 13%", TaxType.Percentage, 0.13m),
    new TaxRule("EXENTO", "Exento", TaxType.Percentage, 0.00m)
});

TaxResult iva = TaxCalculator.Calculate(subtotal, catalog.Get("IVA13"));
```

La interfaz `ITaxCatalog` permite crear proveedores personalizados (base de datos, API, archivos, etc.) sin tocar `TaxCalculator`.

### JSON

Puedes cargar un catálogo completo desde un archivo JSON:

```csharp
ITaxCatalog catalog = JsonTaxCatalogProvider.Load("taxes.json");
TaxCalculation calc = TaxCalculator.Calculate(Money.FromDecimal(50.00m), catalog, "IVA16", "PROPINA");
```

Ejemplo de archivo `taxes.json`:

```json
{
  "taxes": [
    {
      "code": "IVA13",
      "name": "IVA 13%",
      "type": "percentage",
      "rate": 0.13,
      "included": false,
      "rounding": "half_up"
    },
    {
      "code": "PROPINA",
      "name": "Propina fija",
      "type": "fixed",
      "rate": 5.00,
      "included": false,
      "rounding": "half_up"
    }
  ]
}
```

Valores válidos para `type`: `percentage`, `fixed`.

Valores válidos para `rounding`: `half_up`, `to_even`, `down`, `up`, `away_from_zero`.

---

## Facturación

La facturación combina `Money`, impuestos, descuentos y retenciones en un único documento calculado.

### InvoiceModel

Modelo de entrada para calcular una factura.

```csharp
var model = new InvoiceModel(
    number: "F-0001",
    date: DateTime.Today,
    lines: new[] { line1, line2 },
    globalDiscounts: new[] { new Discount("Fidelidad", DiscountType.FixedAmount, 5.00m) },
    retentions: new[] { new Retention("Retención 1%", RetentionType.Percentage, 0.01m) });
```

### InvoiceLine

Representa una línea de factura sin calcular.

```csharp
var line = new InvoiceLine(
    description: "Producto A",
    quantity: 2,
    unitPrice: Money.FromDecimal(25.00m),
    discounts: new[] { new Discount("10%", DiscountType.Percentage, 0.10m) },
    taxCodes: new[] { "IVA13" });
```

### Discount

Descuento aplicable a una línea o a toda la factura.

```csharp
var percentageDiscount = new Discount("Promo 10%", DiscountType.Percentage, 0.10m);
var fixedDiscount = new Discount("Bonificación", DiscountType.FixedAmount, 5.00m);
```

### Retention

Retención aplicada sobre el total de la factura.

```csharp
var retention = new Retention("Retención 1%", RetentionType.Percentage, 0.01m);
```

### InvoiceCalculator

Calcula la factura completa.

```csharp
Invoice invoice = InvoiceCalculator.Calculate(model, taxCatalog);

Console.WriteLine(invoice.SubTotal);        // Suma de brutos
Console.WriteLine(invoice.LineDiscounts);   // Descuentos de línea
Console.WriteLine(invoice.SubTotalNet);     // Subtotal neto
Console.WriteLine(invoice.GlobalDiscounts); // Descuentos globales
Console.WriteLine(invoice.TaxableAmount);   // Monto gravable
Console.WriteLine(invoice.TotalTaxes);      // Impuestos
Console.WriteLine(invoice.TotalRetentions); // Retenciones
Console.WriteLine(invoice.Total);           // Total a pagar
```

### Flujo de cálculo

1. **Línea**: `Gross = quantity × unitPrice`
2. **Descuentos de línea** sobre el bruto.
3. **Neto de línea**: `Gross - descuentos`.
4. **Impuestos** sobre el neto de línea.
5. **Subtotales**: suma de brutos, descuentos y netos.
6. **Descuentos globales** sobre el subtotal neto.
7. **Monto gravable**: `SubTotalNet - descuentos globales`.
8. **Retenciones** sobre `TaxableAmount + TotalTaxes`.
9. **Total**: `TaxableAmount + TotalTaxes - TotalRetentions`.

---

## Notas importantes

- `Money` es **inmutable**: cada operación devuelve una nueva instancia.
- El redondeo por defecto es `HalfUp`, común en finanzas.
- No se permite dividir por cero; se lanza `DivideByZeroException`.
- Las tasas de descuento deben estar en el rango `[0, 1]`.
- Las reglas de impuesto se validan al crearse; tasas fuera de rango lanzan excepciones.
- Las cantidades de línea deben ser mayores o iguales a cero.
