using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions.Clock;
using RichillCapital.Domain.Abstractions.Repositories;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Abstractions;

namespace RichillCapital.UseCases.Orders.Commands;

internal sealed class CreateOrderCommandHandler(
    IDateTimeProvider _dateTimeProvider,
    IReadOnlyRepository<Account> _accountRepository,
    IReadOnlyRepository<Instrument> _instrumentRepository,
    IRepository<Order> _orderRepository,
    IUnitOfWork _unitOfWork) :
    ICommandHandler<CreateOrderCommand, ErrorOr<OrderId>>
{
    public async Task<ErrorOr<OrderId>> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = Result<(AccountId, Symbol)>
            .Combine(
                AccountId.From(command.AccountId),
                Symbol.From(command.Symbol));

        if (validationResult.IsFailure)
        {
            return ErrorOr<OrderId>.WithError(validationResult.Error);
        }

        var (accountId, symbol) = validationResult.Value;

        if (!await _accountRepository.AnyAsync(a => a.Id == accountId))
        {
            return ErrorOr<OrderId>.WithError(OrderErrors.AccountNotFound(accountId));
        }

        if (!await _instrumentRepository.AnyAsync(i => i.Symbol == symbol))
        {
            return ErrorOr<OrderId>.WithError(OrderErrors.InstrumentNotFound(symbol));
        }

        var errorOrOrder = Order.Create(
            id: OrderId.NewOrderId(),
            accountId: accountId,
            symbol: symbol,
            createdTime: _dateTimeProvider.UtcNow);

        if (errorOrOrder.HasError)
        {
            return ErrorOr<OrderId>.WithError(errorOrOrder.Errors);
        }

        var order = errorOrOrder.Value;

        _orderRepository.Add(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ErrorOr<OrderId>.With(order.Id);
    }
}