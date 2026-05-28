namespace PayFlowHub.PaymentOrchestrator.Api.Domain.Payments;

public enum PaymentStatus
{
    Created = 0,
    Authorized = 1,
    Captured = 2,
    Failed = 3,
    Refunded = 4
}
