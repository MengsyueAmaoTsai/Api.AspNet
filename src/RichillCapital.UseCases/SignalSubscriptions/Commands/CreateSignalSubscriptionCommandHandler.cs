using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSubscriptions.Commands;

internal sealed class CreateSignalSubscriptionCommandHandler(
    IReadOnlyRepository<User> _userRepository,
    IReadOnlyRepository<SignalSource> _signalSourceRepository,
    IDateTimeProvider _dateTimeProvider,
    IRepository<SignalSubscription> _signalSubscriptionRepository,
    IUnitOfWork _unitOfWork) :
    ICommandHandler<CreateSignalSubscriptionCommand, ErrorOr<SignalSubscriptionId>>
{
    public async Task<ErrorOr<SignalSubscriptionId>> Handle(
        CreateSignalSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = Result<(UserId, SignalSourceId)>
            .Combine(
                UserId.From(command.UserId),
                SignalSourceId.From(command.SignalSourceId));

        if (validationResult.IsFailure)
        {
            return ErrorOr<SignalSubscriptionId>.WithError(validationResult.Error);
        }

        var (userId, signalSourceId) = validationResult.Value;

        if (!await _userRepository.AnyAsync(u => u.Id == userId, cancellationToken))
        {
            return ErrorOr<SignalSubscriptionId>.WithError(SignalSubscriptionErrors.UserNotFound(userId));
        }

        if (!await _signalSourceRepository.AnyAsync(s => s.Id == signalSourceId, cancellationToken))
        {
            return ErrorOr<SignalSubscriptionId>.WithError(SignalSubscriptionErrors.SignalSourceNotFound(signalSourceId));
        }

        var errorOrSignalSubscription = SignalSubscription
            .Create(
                id: SignalSubscriptionId.NewSignalSubscriptionId(),
                userId: userId,
                signalSourceId: signalSourceId,
                createdTime: _dateTimeProvider.UtcNow);

        if (errorOrSignalSubscription.HasError)
        {
            return ErrorOr<SignalSubscriptionId>.WithError(errorOrSignalSubscription.Errors);
        }

        var signalSubscription = errorOrSignalSubscription.Value;

        _signalSubscriptionRepository.Add(signalSubscription);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ErrorOr<SignalSubscriptionId>.With(signalSubscription.Id);
    }
}