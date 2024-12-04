using RichillCapital.SharedKernel;

namespace RichillCapital.Domain;

public static class InstrumentError
{
    public static Error NotFound(Symbol symbol) =>
        Error.NotFound("Instruments.NotFound", $"Instrument with symbol {symbol} not found.");

    public static Error AlreadyExists(Symbol symbol) =>
        Error.NotFound("Instruments.AlreadyExists", $"Instrument with symbol {symbol} already exists.");
}