using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.DataFeeds.Queries;

internal sealed class ListDataFeedsQueryHandler :
    IQueryHandler<ListDataFeedsQuery, ErrorOr<IEnumerable<DataFeedDto>>>
{
    public Task<ErrorOr<IEnumerable<DataFeedDto>>> Handle(
        ListDataFeedsQuery query,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}