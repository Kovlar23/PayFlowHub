namespace PayFlowHub.PaymentOrchestrator.Api.Domain.Payments;

public sealed record PaymentMethodSnapshot(string Type, string MaskedPan, string Holder)
{
    public static PaymentMethodSnapshot Card(string maskedPan, string holder)
    {
        if (string.IsNullOrWhiteSpace(maskedPan))
        {
            throw new ArgumentException("Masked PAN is required.", nameof(maskedPan));
        }

        if (string.IsNullOrWhiteSpace(holder))
        {
            throw new ArgumentException("Card holder is required.", nameof(holder));
        }

        return new PaymentMethodSnapshot("card", maskedPan.Trim(), holder.Trim());
    }
}
