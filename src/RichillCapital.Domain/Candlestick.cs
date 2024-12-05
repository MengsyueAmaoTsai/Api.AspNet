using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class Candlestick : ValueObject
{
    private Candlestick(
        Symbol symbol,
        decimal open,
        decimal high,
        decimal low,
        decimal close,
        decimal volume,
        DateTimeOffset time)
    {
        Symbol = symbol;
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
        Time = time;
    }

    public Symbol Symbol { get; private set; }
    public decimal Open { get; private set; }
    public decimal High { get; private set; }
    public decimal Low { get; private set; }
    public decimal Close { get; private set; }
    public decimal Volume { get; private set; }
    public DateTimeOffset Time { get; private set; }

    public static ErrorOr<Candlestick> Create(
        Symbol symbol,
        decimal open,
        decimal high,
        decimal low,
        decimal close,
        decimal volume,
        DateTimeOffset time)
    {
        var candlestick = new Candlestick(
            symbol,
            open,
            high,
            low,
            close,
            volume,
            time);

        return ErrorOr<Candlestick>.With(candlestick);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Symbol;
        yield return Open;
        yield return High;
        yield return Low;
        yield return Close;
        yield return Volume;
        yield return Time;
    }
}