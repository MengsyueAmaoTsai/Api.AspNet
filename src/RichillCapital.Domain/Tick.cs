using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class Tick : ValueObject
{
    private Tick(
        Symbol symbol,
        decimal price,
        decimal volume,
        DateTimeOffset time)
    {
        Symbol = symbol;
        Price = price;
        Volume = volume;
        Time = time;
    }

    public Symbol Symbol { get; private set; }
    public decimal Price { get; private set; }
    public decimal Volume { get; private set; }
    public DateTimeOffset Time { get; private set; }

    public static ErrorOr<Tick> Create(
      Symbol symbol,
      decimal price,
      decimal volume,
      DateTimeOffset time)
    {
        var tick = new Tick(
            symbol,
            price,
            volume,
            time);

        return ErrorOr<Tick>.With(tick);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Symbol;
        yield return Price;
        yield return Volume;
        yield return Time;
    }
}
