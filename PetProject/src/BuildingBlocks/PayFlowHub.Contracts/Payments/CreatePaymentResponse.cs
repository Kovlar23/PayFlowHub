namespace PayFlowHub.Contracts.Payments;

public sealed record CreatePaymentResponse(
    string PaymentId,
    string Status,
    string ApiVersion,
    string CorrelationId,
    string IdempotencyKey);
