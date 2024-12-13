using RichillCapital.Domain;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSubscriptions.Commands;

public sealed record CreateSignalSubscriptionCommand :
    ICommand<ErrorOr<SignalSubscriptionId>>
{
    public required string UserId { get; init; }
    public required string SignalSourceId { get; init; }
}
