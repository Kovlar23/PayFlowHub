namespace PayFlowHub.PaymentOrchestrator.Api.Domain.Abstractions;

public abstract class AggregateRoot
{
    public Guid Id { get; protected init; }
}
