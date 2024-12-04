using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class Instrument : Entity<InstrumentId>
{
    private Instrument(
        InstrumentId id,
        Symbol symbol,
        string description,
        InstrumentType type,
        DateTimeOffset createdTime)
        : base(id)
    {
        Symbol = symbol;
        Description = description;
        Type = type;
        CreatedTime = createdTime;
    }

    public Symbol Symbol { get; private set; }
    public string Description { get; private set; }
    public InstrumentType Type { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<Instrument> Create(
        InstrumentId id,
        Symbol symbol,
        string description,
        InstrumentType type,
        DateTimeOffset createdTime)
    {
        var instrument = new Instrument(
            id,
            symbol,
            description,
            type,
            createdTime);

        return ErrorOr<Instrument>.With(instrument);
    }
}

public sealed class InstrumentId : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private InstrumentId(string value)
        : base(value)
    {
    }

    public static Result<InstrumentId> From(string value)
    {
        return Result<InstrumentId>.With(new InstrumentId(value));
    }
}


public sealed class Symbol : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private Symbol(string value)
        : base(value)
    {
    }

    public static Result<Symbol> From(string value)
    {
        return Result<Symbol>.With(new Symbol(value));
    }
}

public sealed class InstrumentType : Enumeration<InstrumentType>
{
    public static readonly InstrumentType Index = new(nameof(Index), 0);
    public static readonly InstrumentType CryptoCurrency = new(nameof(CryptoCurrency), 1);
    public static readonly InstrumentType Future = new(nameof(Future), 2);
    public static readonly InstrumentType Option = new(nameof(Option), 3);
    public static readonly InstrumentType Swap = new(nameof(Swap), 4);
    public static readonly InstrumentType Forward = new(nameof(Forward), 5);

    private InstrumentType(string name, int value)
        : base(name, value)
    {
    }
}