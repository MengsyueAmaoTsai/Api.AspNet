using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.SignalSources.Queries;

internal sealed class GetSignalSourceQueryHandler(
    IReadOnlyRepository<SignalSource> _repository) :
    IQueryHandler<GetSignalSourceQuery, ErrorOr<SignalSourceDto>>
{
    public async Task<ErrorOr<SignalSourceDto>> Handle(
        GetSignalSourceQuery query,
        CancellationToken cancellationToken)
    {
        var validationResult = SignalSourceId.From(query.SignalSourceId);

        if (validationResult.IsFailure)
        {
            return ErrorOr<SignalSourceDto>.WithError(validationResult.Error);
        }

        var id = validationResult.Value;

        var maybeSignalSource = await _repository.FirstOrDefaultAsync(
            s => s.Id == id,
            cancellationToken);

        if (maybeSignalSource.IsNull)
        {
            return ErrorOr<SignalSourceDto>.WithError(SignalSourceErrors.NotFound(id));
        }

        var dto = maybeSignalSource.Value.ToDto();

        return ErrorOr<SignalSourceDto>.With(dto);
    }
}
