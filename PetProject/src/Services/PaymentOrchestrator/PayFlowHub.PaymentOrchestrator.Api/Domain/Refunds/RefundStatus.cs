namespace PayFlowHub.PaymentOrchestrator.Api.Domain.Refunds;

public enum RefundStatus
{
    Requested = 0,
    Approved = 1,
    Settled = 2,
    Rejected = 3
}
