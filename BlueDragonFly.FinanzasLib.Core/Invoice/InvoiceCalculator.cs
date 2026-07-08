namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Calcula una factura completa a partir de un modelo y un catálogo de impuestos.
/// </summary>
public static class InvoiceCalculator
{
    /// <summary>
    /// Calcula una factura completa.
    /// </summary>
    /// <param name="model">Modelo de entrada.</param>
    /// <param name="taxCatalog">Catálogo de impuestos.</param>
    /// <returns>Factura calculada.</returns>
    public static Invoice Calculate(InvoiceModel model, ITaxCatalog taxCatalog)
    {
        var lineResults = model.Lines.Select(line => CalculateLine(line, taxCatalog)).ToList();

        Money subTotal = lineResults.Aggregate(Money.Zero, (acc, line) => acc + line.GrossAmount);
        Money lineDiscounts = lineResults.Aggregate(Money.Zero, (acc, line) => acc + line.DiscountAmount);
        Money subTotalNet = lineResults.Aggregate(Money.Zero, (acc, line) => acc + line.NetAmount);
        Money totalTaxes = lineResults.Aggregate(Money.Zero, (acc, line) => acc + line.Taxes.TotalTax);

        Money globalDiscounts = CalculateDiscounts(model.GlobalDiscounts, subTotalNet);
        Money taxableAmount = subTotalNet - globalDiscounts;

        Money totalRetentions = model.Retentions.Aggregate(Money.Zero, (acc, retention) =>
            acc + retention.CalculateAmount(taxableAmount + totalTaxes));

        Money total = taxableAmount + totalTaxes - totalRetentions;

        return new Invoice(
            model.Number,
            model.Date,
            lineResults,
            subTotal,
            lineDiscounts,
            subTotalNet,
            globalDiscounts,
            taxableAmount,
            totalTaxes,
            totalRetentions,
            total);
    }

    private static InvoiceLineResult CalculateLine(InvoiceLine line, ITaxCatalog taxCatalog)
    {
        Money grossAmount = Money.CalculateTotal(line.Quantity, line.UnitPrice);
        Money lineDiscounts = CalculateDiscounts(line.Discounts, grossAmount);
        Money netAmount = grossAmount - lineDiscounts;

        var taxRules = line.TaxCodes.Select(taxCatalog.Get).ToList();
        TaxCalculation taxes = TaxCalculator.Calculate(netAmount, taxRules);

        Money lineTotal = taxes.Details.Aggregate(netAmount, (acc, detail) =>
            detail.Included ? acc : acc + detail.TaxAmount);

        return new InvoiceLineResult(line, grossAmount, lineDiscounts, netAmount, taxes, lineTotal);
    }

    private static Money CalculateDiscounts(IEnumerable<Discount> discounts, Money baseAmount)
    {
        return discounts.Aggregate(Money.Zero, (acc, discount) => acc + discount.CalculateAmount(baseAmount));
    }
}
