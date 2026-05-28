using PayFlowHub.PaymentOrchestrator.Api.Domain.Abstractions;
using PayFlowHub.PaymentOrchestrator.Api.Domain.Refunds;

namespace PayFlowHub.PaymentOrchestrator.Api.Domain.Payments;

public sealed class Payment : AggregateRoot
{
    private readonly List<Refund> _refunds = [];

    private Payment(
        Guid id,
        string merchantId,
        string customerId,
        string idempotencyKey,
        string correlationId,
        Money amount,
        PaymentMethodSnapshot paymentMethod,
        DateTimeOffset createdAtUtc)
    {
        Id = id;
        MerchantId = merchantId;
        CustomerId = customerId;
        IdempotencyKey = idempotencyKey;
        CorrelationId = correlationId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        CreatedAtUtc = createdAtUtc;
        Status = PaymentStatus.Created;
    }

    public string MerchantId { get; private set; }
    public string CustomerId { get; private set; }
    public string IdempotencyKey { get; private set; }
    public string CorrelationId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentMethodSnapshot PaymentMethod { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? AuthorizedAtUtc { get; private set; }
    public DateTimeOffset? CapturedAtUtc { get; private set; }
    public DateTimeOffset? FailedAtUtc { get; private set; }
    public string? FailureCode { get; private set; }
    public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();

    public static Payment Create(
        string merchantId,
        string customerId,
        string idempotencyKey,
        string correlationId,
        Money amount,
        PaymentMethodSnapshot paymentMethod,
        DateTimeOffset createdAtUtc)
    {
        EnsureRequired(merchantId, nameof(merchantId));
        EnsureRequired(customerId, nameof(customerId));
        EnsureRequired(idempotencyKey, nameof(idempotencyKey));
        EnsureRequired(correlationId, nameof(correlationId));

        return new Payment(
            Guid.CreateVersion7(),
            merchantId.Trim(),
            customerId.Trim(),
            idempotencyKey.Trim(),
            correlationId.Trim(),
            amount,
            paymentMethod,
            createdAtUtc);
    }

    public void MarkAuthorized(DateTimeOffset timestampUtc)
    {
        if (Status is not PaymentStatus.Created)
        {
            throw new InvalidOperationException("Only created payment can be authorized.");
        }

        Status = PaymentStatus.Authorized;
        AuthorizedAtUtc = timestampUtc;
    }

    public void MarkCaptured(DateTimeOffset timestampUtc)
    {
        if (Status is not PaymentStatus.Authorized)
        {
            throw new InvalidOperationException("Only authorized payment can be captured.");
        }

        Status = PaymentStatus.Captured;
        CapturedAtUtc = timestampUtc;
    }

    public void MarkFailed(string failureCode, DateTimeOffset timestampUtc)
    {
        if (Status is PaymentStatus.Captured or PaymentStatus.Refunded)
        {
            throw new InvalidOperationException("Captured or refunded payment cannot fail.");
        }

        EnsureRequired(failureCode, nameof(failureCode));

        Status = PaymentStatus.Failed;
        FailureCode = failureCode.Trim();
        FailedAtUtc = timestampUtc;
    }

    public Refund RequestRefund(Money refundAmount, string reason, DateTimeOffset timestampUtc)
    {
        if (Status is not PaymentStatus.Captured and not PaymentStatus.Refunded)
        {
            throw new InvalidOperationException("Refund can be requested only for captured or refunded payment.");
        }

        EnsureRequired(reason, nameof(reason));

        if (!string.Equals(refundAmount.Currency, Amount.Currency, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Refund currency must match payment currency.");
        }

        var totalApproved = _refunds
            .Where(static x => x.Status is RefundStatus.Approved or RefundStatus.Settled)
            .Sum(static x => x.Amount.Amount);

        if (totalApproved + refundAmount.Amount > Amount.Amount)
        {
            throw new InvalidOperationException("Total approved refunds cannot exceed payment amount.");
        }

        var refund = Refund.Request(Id, refundAmount, reason, timestampUtc);
        _refunds.Add(refund);

        if (totalApproved + refundAmount.Amount == Amount.Amount)
        {
            Status = PaymentStatus.Refunded;
        }

        return refund;
    }

    private static void EnsureRequired(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", parameterName);
        }
    }
}
