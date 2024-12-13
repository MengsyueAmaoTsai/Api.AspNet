using RichillCapital.Domain.Abstractions.Events;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Domain;

public sealed class Order : Entity<OrderId>
{
    private Order(
        OrderId id,
        AccountId accountId,
        Symbol symbol,
        DateTimeOffset createdTime)
        : base(id)
    {
        AccountId = accountId;
        Symbol = symbol;
        CreatedTime = createdTime;
    }

    public AccountId AccountId { get; private set; }
    public Symbol Symbol { get; private set; }
    public DateTimeOffset CreatedTime { get; private set; }

    public static ErrorOr<Order> Create(
        OrderId id,
        AccountId accountId,
        Symbol symbol,
        DateTimeOffset createdTime)
    {
        var order = new Order(
            id,
            accountId,
            symbol,
            createdTime);

        order.RegisterDomainEvent(new OrderCreatedDomainEvent
        {
            OrderId = id,
        });

        return ErrorOr<Order>.With(order);
    }
}

public sealed class OrderId : SingleValueObject<string>
{
    internal const int MaxLength = 36;

    private OrderId(string value)
        : base(value)
    {
    }

    public static Result<OrderId> From(string value) =>
        Result<string>
            .With(value)
            .Ensure(id => !string.IsNullOrEmpty(id), Error.Invalid($"'{nameof(OrderId)}' cannot be null or empty."))
            .Ensure(id => id.Length <= MaxLength, Error.Invalid($"'{nameof(OrderId)}' cannot be greater than {MaxLength} characters."))
            .Then(id => new OrderId(id));
}

public abstract record OrderDomainEvent : DomainEvent
{
    public required OrderId OrderId { get; init; }
}

public sealed record OrderCreatedDomainEvent : OrderDomainEvent
{
}

public static class OrderErrors
{
    public static Error NotFound(OrderId id) =>
        Error.NotFound("Orders.NotFound", $"Order with id '{id}' was not found.");
}