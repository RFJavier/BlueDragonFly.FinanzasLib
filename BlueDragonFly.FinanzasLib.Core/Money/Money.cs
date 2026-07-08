using System.Globalization;

namespace BlueDragonFly.FinanzasLib.Core;

/// <summary>
/// Representa una cantidad monetaria con exactitud de centavos.
/// Internamente almacena el valor como <see cref="long"/> de centavos.
/// </summary>
public readonly record struct Money : IComparable<Money>, IComparable
{
    private const decimal CentsPerUnit = 100m;

    /// <summary>
    /// Cantidad de centavos.
    /// </summary>
    public long Cents { get; }

    /// <summary>
    /// Valor decimal con dos decimales.
    /// </summary>
    public decimal Value => Cents / CentsPerUnit;

    /// <summary>
    /// Representa cero.
    /// </summary>
    public static Money Zero => new(0);

    private Money(long cents)
    {
        Cents = cents;
    }

    /// <summary>
    /// Crea una instancia de <see cref="Money"/> a partir de centavos.
    /// </summary>
    public static Money FromCents(long cents)
    {
        return new Money(cents);
    }

    /// <summary>
    /// Crea una instancia de <see cref="Money"/> a partir de un valor decimal.
    /// </summary>
    /// <param name="value">Valor decimal.</param>
    /// <param name="mode">Modo de redondeo. Por defecto <see cref="RoundingMode.HalfUp"/>.</param>
    public static Money FromDecimal(decimal value, RoundingMode mode = RoundingMode.HalfUp)
    {
        return new Money(MoneyConverter.DecimalToCents(value, mode));
    }

    /// <summary>
    /// Suma dos cantidades monetarias.
    /// </summary>
    public static Money Add(Money left, Money right)
    {
        return new Money(left.Cents + right.Cents);
    }

    /// <summary>
    /// Resta dos cantidades monetarias.
    /// </summary>
    public static Money Subtract(Money left, Money right)
    {
        return new Money(left.Cents - right.Cents);
    }

    /// <summary>
    /// Multiplica una cantidad monetaria por un factor decimal.
    /// </summary>
    public static Money Multiply(Money money, decimal factor, RoundingMode mode = RoundingMode.HalfUp)
    {
        decimal resultCents = money.Cents * factor;
        return new Money(MoneyConverter.DecimalToCents(resultCents / CentsPerUnit, mode));
    }

    /// <summary>
    /// Multiplica una cantidad monetaria por un factor entero.
    /// </summary>
    public static Money Multiply(Money money, long factor)
    {
        return new Money(money.Cents * factor);
    }

    /// <summary>
    /// Divide una cantidad monetaria por un divisor decimal.
    /// </summary>
    public static Money Divide(Money money, decimal divisor, RoundingMode mode = RoundingMode.HalfUp)
    {
        if (divisor == 0m)
        {
            throw new DivideByZeroException("No se puede dividir una cantidad monetaria por cero.");
        }

        decimal resultCents = money.Cents / divisor;
        return new Money(MoneyConverter.DecimalToCents(resultCents / CentsPerUnit, mode));
    }

    /// <summary>
    /// Divide una cantidad monetaria por un divisor entero.
    /// </summary>
    public static Money Divide(Money money, long divisor, RoundingMode mode = RoundingMode.HalfUp)
    {
        if (divisor == 0)
        {
            throw new DivideByZeroException("No se puede dividir una cantidad monetaria por cero.");
        }

        decimal resultCents = (decimal)money.Cents / divisor;
        return new Money(MoneyConverter.DecimalToCents(resultCents / CentsPerUnit, mode));
    }

    /// <summary>
    /// Calcula el total de una venta: cantidad × precio unitario.
    /// El resultado siempre es exacto en centavos.
    /// </summary>
    /// <param name="quantity">Cantidad de ítems.</param>
    /// <param name="unitPrice">Precio unitario.</param>
    public static Money CalculateTotal(long quantity, Money unitPrice)
    {
        return new Money(quantity * unitPrice.Cents);
    }

    /// <summary>
    /// Aplica un descuento a una cantidad monetaria.
    /// </summary>
    /// <param name="amount">Monto base.</param>
    /// <param name="discountRate">Tasa de descuento (por ejemplo, 0.15m para 15%).</param>
    /// <param name="mode">Modo de redondeo. Por defecto <see cref="RoundingMode.HalfUp"/>.</param>
    public static Money ApplyDiscount(Money amount, decimal discountRate, RoundingMode mode = RoundingMode.HalfUp)
    {
        if (discountRate < 0m || discountRate > 1m)
        {
            throw new ArgumentOutOfRangeException(nameof(discountRate), "La tasa de descuento debe estar entre 0 y 1.");
        }

        Money discount = Multiply(amount, discountRate, mode);
        return Subtract(amount, discount);
    }

    /// <summary>
    /// Calcula el cociente entre dos cantidades monetarias.
    /// </summary>
    public static decimal Ratio(Money dividend, Money divisor)
    {
        if (divisor.Cents == 0)
        {
            throw new DivideByZeroException("No se puede dividir por cero.");
        }

        return (decimal)dividend.Cents / divisor.Cents;
    }

    #region Operators

    public static Money operator +(Money left, Money right) => Add(left, right);
    public static Money operator -(Money left, Money right) => Subtract(left, right);
    public static Money operator *(Money money, decimal factor) => Multiply(money, factor);
    public static Money operator *(decimal factor, Money money) => Multiply(money, factor);
    public static Money operator *(Money money, long factor) => Multiply(money, factor);
    public static Money operator *(long factor, Money money) => Multiply(money, factor);
    public static Money operator /(Money money, decimal divisor) => Divide(money, divisor);
    public static Money operator /(Money money, long divisor) => Divide(money, divisor);
    public static bool operator <(Money left, Money right) => left.Cents < right.Cents;
    public static bool operator >(Money left, Money right) => left.Cents > right.Cents;
    public static bool operator <=(Money left, Money right) => left.Cents <= right.Cents;
    public static bool operator >=(Money left, Money right) => left.Cents >= right.Cents;

    #endregion

    #region Comparisons

    public int CompareTo(Money other)
    {
        return Cents.CompareTo(other.Cents);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        return obj is Money other
            ? CompareTo(other)
            : throw new ArgumentException($"El objeto debe ser de tipo {nameof(Money)}.", nameof(obj));
    }

    #endregion

    /// <summary>
    /// Devuelve una representación formateada como moneda.
    /// </summary>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return Value.ToString(format ?? "C", formatProvider ?? CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Devuelve una representación formateada como moneda en la cultura actual.
    /// </summary>
    public override string ToString()
    {
        return ToString(null, CultureInfo.CurrentCulture);
    }
}
