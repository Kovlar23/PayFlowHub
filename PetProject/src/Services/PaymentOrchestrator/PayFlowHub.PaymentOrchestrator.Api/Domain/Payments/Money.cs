namespace PayFlowHub.PaymentOrchestrator.Api.Domain.Payments;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(currency) || currency.Trim().Length != 3)
        {
            throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency));
        }

        Amount = decimal.Round(amount, 2, MidpointRounding.ToEven);
        Currency = currency.Trim().ToUpperInvariant();
    }
}
