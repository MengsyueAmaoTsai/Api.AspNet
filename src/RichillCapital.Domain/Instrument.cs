using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class Instrument : Entity<InstrumentId>
{
    private Instrument(
        InstrumentId id,
        DateTimeOffset createdTime)
        : base(id)
    {
        CreatedTime = createdTime;
    }

    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<Instrument> Create(
        InstrumentId id,
        DateTimeOffset createdTime)
    {
        var instrument = new Instrument(id, createdTime);

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
}


public sealed class Symbol : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private Symbol(string value)
        : base(value)
    {
    }
}

public sealed class InstrumentType : Enumeration<InstrumentType>
{
    private InstrumentType(string name, int value) : base(name, value)
    {
    }
}