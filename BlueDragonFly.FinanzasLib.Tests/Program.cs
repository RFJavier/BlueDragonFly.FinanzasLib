using BlueDragonFly.FinanzasLib.Core;

int passed = 0;
int failed = 0;

void AssertEqual<T>(string name, T expected, T actual)
{
    if (EqualityComparer<T>.Default.Equals(expected, actual))
    {
        Console.WriteLine($"[PASS] {name}");
        passed++;
    }
    else
    {
        Console.WriteLine($"[FAIL] {name}: expected {expected}, got {actual}");
        failed++;
    }
}

void AssertTrue(string name, bool condition)
{
    if (condition)
    {
        Console.WriteLine($"[PASS] {name}");
        passed++;
    }
    else
    {
        Console.WriteLine($"[FAIL] {name}");
        failed++;
    }
}

// Decimal <-> Cents
AssertEqual("DecimalToCents(12.35)", 1235L, MoneyConverter.DecimalToCents(12.35m));
AssertEqual("CentsToDecimal(1235)", 12.35m, MoneyConverter.CentsToDecimal(1235));
AssertEqual("DecimalToCents(0.005) HalfUp", 1L, MoneyConverter.DecimalToCents(0.005m, RoundingMode.HalfUp));
AssertEqual("DecimalToCents(0.005) ToEven", 0L, MoneyConverter.DecimalToCents(0.005m, RoundingMode.ToEven));

// Money creation
Money amount = Money.FromDecimal(12.35m);
AssertEqual("Money.FromDecimal.Cents", 1235L, amount.Cents);
AssertEqual("Money.FromDecimal.Value", 12.35m, amount.Value);
AssertEqual("Money.FromCents", 1235L, Money.FromCents(1235).Cents);

// Arithmetic
AssertEqual("Add", 13.50m, (Money.FromDecimal(10.00m) + Money.FromDecimal(3.50m)).Value);
AssertEqual("Subtract", 6.50m, (Money.FromDecimal(10.00m) - Money.FromDecimal(3.50m)).Value);
AssertEqual("Multiply by long", 7.00m, (Money.FromDecimal(1.00m) * 7L).Value);
AssertEqual("CalculateTotal(7, $1.00)", 7.00m, Money.CalculateTotal(7, Money.FromDecimal(1.00m)).Value);
AssertEqual("Divide by long", 33.33m, (Money.FromDecimal(100.00m) / 3L).Value);

// Discount
AssertEqual("ApplyDiscount 15%", 85.00m, Money.ApplyDiscount(Money.FromDecimal(100.00m), 0.15m).Value);
AssertEqual("ApplyDiscount 0%", 100.00m, Money.ApplyDiscount(Money.FromDecimal(100.00m), 0.00m).Value);
AssertEqual("ApplyDiscount 100%", 0.00m, Money.ApplyDiscount(Money.FromDecimal(100.00m), 1.00m).Value);

// Comparisons
AssertTrue("Equality", Money.FromDecimal(10.00m) == Money.FromDecimal(10.00m));
AssertTrue("LessThan", Money.FromDecimal(10.00m) < Money.FromDecimal(20.00m));
AssertTrue("GreaterThan", Money.FromDecimal(20.00m) > Money.FromDecimal(10.00m));
AssertTrue("Zero", Money.Zero.Cents == 0);

// Taxes
var iva13 = new TaxRule("IVA13", "IVA 13%", TaxType.Percentage, 0.13m);
TaxResult singleTax = TaxCalculator.Calculate(Money.FromDecimal(100.00m), iva13);
AssertEqual("Tax IVA13 amount", 13.00m, singleTax.TaxAmount.Value);
AssertEqual("Tax IVA13 total", 113.00m, singleTax.Total.Value);

var fixedTax = new TaxRule("PROPINA", "Propina", TaxType.Fixed, 5.00m);
TaxResult fixedResult = TaxCalculator.Calculate(Money.FromDecimal(80.00m), fixedTax);
AssertEqual("Fixed tax amount", 5.00m, fixedResult.TaxAmount.Value);

var catalog = new InMemoryTaxCatalog(new[] { iva13, fixedTax });
TaxCalculation multi = TaxCalculator.Calculate(Money.FromDecimal(100.00m), catalog.GetAll());
AssertEqual("Multi total tax", 18.00m, multi.TotalTax.Value);
AssertEqual("Multi grand total", 118.00m, multi.GrandTotal.Value);

string json = """
{
  "taxes": [
    {
      "code": "IVA16",
      "name": "IVA 16%",
      "type": "percentage",
      "rate": 0.16,
      "included": false,
      "rounding": "half_up"
    }
  ]
}
""";
ITaxCatalog jsonCatalog = JsonTaxCatalogProvider.LoadFromJson(json);
TaxResult jsonTax = TaxCalculator.Calculate(Money.FromDecimal(100.00m), jsonCatalog.Get("IVA16"));
AssertEqual("JSON tax amount", 16.00m, jsonTax.TaxAmount.Value);

// Invoice
var invoiceCatalog = new InMemoryTaxCatalog(new[]
{
    new TaxRule("IVA13", "IVA 13%", TaxType.Percentage, 0.13m),
    new TaxRule("EXENTO", "Exento", TaxType.Percentage, 0.00m)
});

var invoiceModel = new InvoiceModel(
    number: "F-001",
    date: DateTime.Today,
    lines: new[]
    {
        new InvoiceLine("Item 1", 2, Money.FromDecimal(10.00m), taxCodes: new[] { "IVA13" }),
        new InvoiceLine("Item 2", 1, Money.FromDecimal(50.00m), discounts: new[] { new Discount("10%", DiscountType.Percentage, 0.10m) }, taxCodes: new[] { "IVA13" })
    },
    globalDiscounts: new[] { new Discount("5 off", DiscountType.FixedAmount, 5.00m) },
    retentions: new[] { new Retention("1%", RetentionType.Percentage, 0.01m) });

Invoice invoice = InvoiceCalculator.Calculate(invoiceModel, invoiceCatalog);
AssertEqual("Invoice SubTotal", 70.00m, invoice.SubTotal.Value);
AssertEqual("Invoice LineDiscounts", 5.00m, invoice.LineDiscounts.Value);
AssertEqual("Invoice SubTotalNet", 65.00m, invoice.SubTotalNet.Value);
AssertEqual("Invoice GlobalDiscounts", 5.00m, invoice.GlobalDiscounts.Value);
AssertEqual("Invoice TaxableAmount", 60.00m, invoice.TaxableAmount.Value);
AssertEqual("Invoice TotalTaxes", 8.45m, invoice.TotalTaxes.Value);
AssertEqual("Invoice TotalRetentions", 0.68m, invoice.TotalRetentions.Value);
AssertEqual("Invoice Total", 67.77m, invoice.Total.Value);

Console.WriteLine($"\nResultados: {passed} pasadas, {failed} fallidas.");
Environment.Exit(failed > 0 ? 1 : 0);

