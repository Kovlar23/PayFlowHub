using PayFlowHub.PaymentOrchestrator.Api.Domain.Abstractions;
using PayFlowHub.PaymentOrchestrator.Api.Domain.Payments;

namespace PayFlowHub.PaymentOrchestrator.Api.Domain.Refunds;

public sealed class Refund : AggregateRoot
{
    private Refund(Guid paymentId, Money amount, string reason, DateTimeOffset requestedAtUtc)
    {
        Id = Guid.CreateVersion7();
        PaymentId = paymentId;
        Amount = amount;
        Reason = reason;
        RequestedAtUtc = requestedAtUtc;
        Status = RefundStatus.Requested;
    }

    public Guid PaymentId { get; private set; }
    public Money Amount { get; private set; }
    public string Reason { get; private set; }
    public RefundStatus Status { get; private set; }
    public DateTimeOffset RequestedAtUtc { get; private set; }
    public DateTimeOffset? ApprovedAtUtc { get; private set; }
    public DateTimeOffset? SettledAtUtc { get; private set; }
    public DateTimeOffset? RejectedAtUtc { get; private set; }
    public string? RejectionCode { get; private set; }

    public static Refund Request(Guid paymentId, Money amount, string reason, DateTimeOffset requestedAtUtc)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("Payment id is required.", nameof(paymentId));
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Reason is required.", nameof(reason));
        }

        return new Refund(paymentId, amount, reason.Trim(), requestedAtUtc);
    }

    public void Approve(DateTimeOffset approvedAtUtc)
    {
        if (Status is not RefundStatus.Requested)
        {
            throw new InvalidOperationException("Only requested refund can be approved.");
        }

        Status = RefundStatus.Approved;
        ApprovedAtUtc = approvedAtUtc;
    }

    public void Settle(DateTimeOffset settledAtUtc)
    {
        if (Status is not RefundStatus.Approved)
        {
            throw new InvalidOperationException("Only approved refund can be settled.");
        }

        Status = RefundStatus.Settled;
        SettledAtUtc = settledAtUtc;
    }

    public void Reject(string rejectionCode, DateTimeOffset rejectedAtUtc)
    {
        if (Status is not RefundStatus.Requested)
        {
            throw new InvalidOperationException("Only requested refund can be rejected.");
        }

        if (string.IsNullOrWhiteSpace(rejectionCode))
        {
            throw new ArgumentException("Rejection code is required.", nameof(rejectionCode));
        }

        Status = RefundStatus.Rejected;
        RejectionCode = rejectionCode.Trim();
        RejectedAtUtc = rejectedAtUtc;
    }
}
