namespace PayFlowHub.Contracts.Payments;

public sealed record CreatePaymentRequest(
    string MerchantId,
    string OrderId,
    long AmountMinor,
    string Currency,
    string? Description);
