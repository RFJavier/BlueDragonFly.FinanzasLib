# BlueDragonFly.FinanzasLib

Biblioteca .NET para realizar cálculos monetarios con **exactitud decimal**.

En lugar de depender de `float` o `double`, todas las operaciones se ejecutan internamente sobre **centavos** (`long`), eliminando los errores de redondeo propios de la aritmética de punto flotante.

## Características

- `Money`: tipo de valor inmutable para cantidades monetarias.
- Conversiones seguras entre `decimal` y centavos.
- Múltiples modos de redondeo (`HalfUp` por defecto).
- Sistema de impuestos extensible con reglas porcentuales y fijas.
- Catálogos de impuestos en memoria o desde JSON.
- Modelo de facturación con descuentos, retenciones y cálculo de totales.
- Sin dependencias de terceros: solo requiere **.NET 10**.

## Requisitos

- .NET 10 SDK

## Instalación

```bash
dotnet add package BlueDragonFly.FinanzasLib.Core
```

> Aún no se ha publicado en NuGet. Puedes compilar el proyecto localmente o referenciarlo directamente desde el código fuente.

## Ejemplo rápido

```csharp
using BlueDragonFly.FinanzasLib.Core;

Money unitPrice = Money.FromDecimal(9.99m);
int quantity = 5;

Money subtotal = Money.CalculateTotal(quantity, unitPrice);
Money tax = subtotal * 0.16m;
Money total = subtotal + tax;

Console.WriteLine($"Subtotal: {subtotal}");
Console.WriteLine($"Impuesto: {tax}");
Console.WriteLine($"Total:    {total}");
```

## Estructura del repositorio

```
BlueDragonFly.FinanzasLib.Core/    # Núcleo de la biblioteca
BlueDragonFly.FinanzasLib.Demo/    # Aplicación de demostración
BlueDragonFly.FinanzasLib.Tests/   # Pruebas y ejemplos de uso
docs/                              # Documentación detallada
```

## Documentación

La documentación completa de la API y ejemplos de uso está disponible en [docs/Core.md](docs/Core.md).

## Compilar

```bash
dotnet build BlueDragonFly.FinanzasLib.slnx
```

## Ejecutar la demo

```bash
dotnet run --project BlueDragonFly.FinanzasLib.Demo
```

## Licencia

Este proyecto está licenciado bajo la [Licencia MIT](LICENSE).

Copyright (c) 2026 Javier Rivera.
