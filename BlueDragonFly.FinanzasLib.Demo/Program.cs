using BlueDragonFly.FinanzasLib.Core;

Console.WriteLine("=== BlueDragonFly.FinanzasLib - Demo de Money ===\n");
double value = 12.3345;
// Redondeo a centavos usando diferentes estrategias
// Crear dinero desde decimal
Money amount = Money.FromDecimal((decimal)value);
Console.WriteLine($"Desde decimal: {amount}");
Console.WriteLine($"  Centavos: {amount.Cents}");
Console.WriteLine($"  Valor:    {amount.Value}");

// Crear dinero desde centavos
Money fromCents = Money.FromCents(1235);
Console.WriteLine($"\nDesde centavos: {fromCents}");

// Sumar y restar
Money total = Money.FromDecimal(10.00m) + Money.FromDecimal(2.50m);
Money change = total - Money.FromDecimal(1.25m);
Console.WriteLine($"\nSuma:   {total}");
Console.WriteLine($"Cambio: {change}");

// Multiplicar por cantidad de items (exacto, sin decimales perdidos)
Money unitPrice = Money.FromDecimal(1.00m);
Money saleTotal = Money.CalculateTotal(7, unitPrice);
Console.WriteLine($"\n7 items a $1.00 = {saleTotal}");

// Multiplicar por factor decimal
Money withTax = saleTotal * 1.16m;
Console.WriteLine($"Total con 16% IVA: {withTax}");

// Dividir
Money split = Money.FromDecimal(100.00m) / 3;
Console.WriteLine($"\n$100.00 / 3 = {split}");

// Descuento
Money discounted = Money.ApplyDiscount(Money.FromDecimal(100.00m), 0.15m);
Console.WriteLine($"$100.00 con 15% descuento = {discounted}");

// Comparaciones
Money a = Money.FromDecimal(10.00m);
Money b = Money.FromDecimal(10.00m);
Money c = Money.FromDecimal(20.00m);
Console.WriteLine($"\n{a} == {b}: {a == b}");
Console.WriteLine($"{a} <  {c}: {a < c}");

// === Fase 2 y 3: Impuestos ===
Console.WriteLine("\n=== Impuestos ===\n");

// Catálogo en memoria
var catalog = new InMemoryTaxCatalog(new[]
{
    new TaxRule("IVA13", "IVA 13%", TaxType.Percentage, 0.13m),
    new TaxRule("EXENTO", "Exento", TaxType.Percentage, 0.00m)
});

Money subtotal = Money.FromDecimal(100.00m);
TaxResult iva = TaxCalculator.Calculate(subtotal, catalog.Get("IVA13"));
Console.WriteLine($"Subtotal: {subtotal}");
Console.WriteLine($"{iva.Name}: {iva.TaxAmount}");
Console.WriteLine($"Total:    {iva.Total}");

// Catálogo desde JSON
Console.WriteLine("\n--- Catálogo JSON ---");
string jsonPath = Path.Combine(AppContext.BaseDirectory, "taxes.json");
ITaxCatalog jsonCatalog = JsonTaxCatalogProvider.Load(jsonPath);

TaxCalculation calculation = TaxCalculator.Calculate(Money.FromDecimal(50.00m), jsonCatalog, "IVA16", "PROPINA");
Console.WriteLine($"Base:       {calculation.BaseAmount}");
foreach (var detail in calculation.Details)
{
    Console.WriteLine($"{detail.Name,-12}: {detail.TaxAmount}");
}
Console.WriteLine($"Impuestos:  {calculation.TotalTax}");
Console.WriteLine($"Total:      {calculation.GrandTotal}");

// === Fase 4: Facturación ===
Console.WriteLine("\n=== Facturación ===\n");

var invoiceModel = new InvoiceModel(
    number: "F-0001",
    date: DateTime.Today,
    lines: new[]
    {
        new InvoiceLine(
            description: "Producto A",
            quantity: 2,
            unitPrice: Money.FromDecimal(25.00m),
            taxCodes: new[] { "IVA13" }),
        new InvoiceLine(
            description: "Producto B",
            quantity: 1,
            unitPrice: Money.FromDecimal(60.00m),
            discounts: new[] { new Discount("Promo 10%", DiscountType.Percentage, 0.10m) },
            taxCodes: new[] { "IVA13" }),
        new InvoiceLine(
            description: "Servicio C",
            quantity: 3,
            unitPrice: Money.FromDecimal(10.00m),
            taxCodes: new[] { "EXENTO" })
    },
    globalDiscounts: new[] { new Discount("Descuento fidelidad", DiscountType.FixedAmount, 5.00m) },
    retentions: new[] { new Retention("Retención 1%", RetentionType.Percentage, 0.01m) });

Invoice invoice = InvoiceCalculator.Calculate(invoiceModel, catalog);

Console.WriteLine($"Factura: {invoice.Number} - {invoice.Date:yyyy-MM-dd}");
Console.WriteLine();
foreach (var line in invoice.Lines)
{
    Console.WriteLine($"{line.Input.Description,-12} x{line.Input.Quantity,3} @ {line.Input.UnitPrice,8} = {line.GrossAmount,8} | Desc: {line.DiscountAmount,6} | Neto: {line.NetAmount,8} | Imp: {line.Taxes.TotalTax,6} | Total: {line.Total,8}");
}
Console.WriteLine();
Console.WriteLine($"Subtotal:          {invoice.SubTotal}");
Console.WriteLine($"Descuentos línea:  {invoice.LineDiscounts}");
Console.WriteLine($"Subtotal neto:     {invoice.SubTotalNet}");
Console.WriteLine($"Descuentos global: {invoice.GlobalDiscounts}");
Console.WriteLine($"Gravable:          {invoice.TaxableAmount}");
Console.WriteLine($"Impuestos:         {invoice.TotalTaxes}");
Console.WriteLine($"Retenciones:       {invoice.TotalRetentions}");
Console.WriteLine($"TOTAL A PAGAR:     {invoice.Total}");

