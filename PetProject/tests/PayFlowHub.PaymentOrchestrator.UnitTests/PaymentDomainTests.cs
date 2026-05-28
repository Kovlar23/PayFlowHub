using PayFlowHub.PaymentOrchestrator.Api.Domain.Payments;
using PayFlowHub.PaymentOrchestrator.Api.Domain.Refunds;
using Xunit;

namespace PayFlowHub.PaymentOrchestrator.UnitTests;

public sealed class PaymentDomainTests
{
    [Fact]
    public void Create_InitializesAggregateWithCreatedStatus()
    {
        var payment = CreatePayment();

        Assert.Equal(PaymentStatus.Created, payment.Status);
        Assert.Equal("merchant-1", payment.MerchantId);
        Assert.Equal("customer-1", payment.CustomerId);
        Assert.Equal("idem-1", payment.IdempotencyKey);
        Assert.Equal("corr-1", payment.CorrelationId);
        Assert.Equal("RUB", payment.Amount.Currency);
    }

    [Fact]
    public void MarkCaptured_WithoutAuthorization_Throws()
    {
        var payment = CreatePayment();

        var action = () => payment.MarkCaptured(DateTimeOffset.UtcNow);

        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void RequestRefund_WhenApprovedRefundsExceedPayment_Throws()
    {
        var payment = CreateCapturedPayment();
        var refund = payment.RequestRefund(new Money(70m, "RUB"), "duplicate", DateTimeOffset.UtcNow);
        refund.Approve(DateTimeOffset.UtcNow);

        var action = () => payment.RequestRefund(new Money(40m, "RUB"), "fraud", DateTimeOffset.UtcNow);

        Assert.Throws<InvalidOperationException>(action);
    }

    [Fact]
    public void RequestRefund_FullAmount_MarksPaymentAsRefunded()
    {
        var payment = CreateCapturedPayment();

        var refund = payment.RequestRefund(new Money(100m, "RUB"), "requested-by-merchant", DateTimeOffset.UtcNow);

        Assert.Equal(PaymentStatus.Refunded, payment.Status);
        Assert.Equal(RefundStatus.Requested, refund.Status);
        Assert.Single(payment.Refunds);
    }

    [Fact]
    public void Reject_AfterApprove_Throws()
    {
        var refund = Refund.Request(Guid.NewGuid(), new Money(10m, "RUB"), "chargeback", DateTimeOffset.UtcNow);
        refund.Approve(DateTimeOffset.UtcNow);

        var action = () => refund.Reject("late", DateTimeOffset.UtcNow);

        Assert.Throws<InvalidOperationException>(action);
    }

    private static Payment CreatePayment()
    {
        return Payment.Create(
            merchantId: "merchant-1",
            customerId: "customer-1",
            idempotencyKey: "idem-1",
            correlationId: "corr-1",
            amount: new Money(100m, "rub"),
            paymentMethod: PaymentMethodSnapshot.Card("4111********1111", "IVAN IVANOV"),
            createdAtUtc: DateTimeOffset.UtcNow);
    }

    private static Payment CreateCapturedPayment()
    {
        var payment = CreatePayment();
        payment.MarkAuthorized(DateTimeOffset.UtcNow);
        payment.MarkCaptured(DateTimeOffset.UtcNow);
        return payment;
    }
}
