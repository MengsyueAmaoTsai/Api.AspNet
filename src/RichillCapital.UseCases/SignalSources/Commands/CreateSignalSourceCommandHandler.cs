
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSources.Commands;

internal sealed class CreateSignalSourceCommandHandler(
    IDateTimeProvider _dateTimeProvider,
    IRepository<SignalSource> _repository,
    IUnitOfWork _unitOfWork) :
    ICommandHandler<CreateSignalSourceCommand, ErrorOr<SignalSourceId>>
{
    public async Task<ErrorOr<SignalSourceId>> Handle(
        CreateSignalSourceCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = Result<(SignalSourceId, SignalSourceStage)>
            .Combine(
                SignalSourceId.From(command.Id),
                SignalSourceStage
                    .FromName(command.Stage)
                    .ToResult(Error.Invalid($"Invalid stage: {command.Stage}")));

        if (validationResult.IsFailure)
        {
            return ErrorOr<SignalSourceId>.WithError(validationResult.Error);
        }

        var (signalSourceId, stage) = validationResult.Value;

        if (await _repository.AnyAsync(
            s => s.Id == signalSourceId,
            cancellationToken))
        {
            return ErrorOr<SignalSourceId>.WithError(SignalSourceErrors.AlreadyExists(signalSourceId));
        }

        var errorOrSignalSource = SignalSource.Create(
            signalSourceId,
            command.Name,
            command.Description,
            command.Version,
            stage,
            _dateTimeProvider.UtcNow);

        if (errorOrSignalSource.HasError)
        {
            return ErrorOr<SignalSourceId>.WithError(errorOrSignalSource.Errors);
        }

        var signalSource = errorOrSignalSource.Value;

        _repository.Add(signalSource);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ErrorOr<SignalSourceId>.With(signalSourceId);
    }
}