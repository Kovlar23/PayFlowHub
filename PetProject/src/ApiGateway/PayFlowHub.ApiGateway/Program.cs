using PayFlowHub.Contracts.Diagnostics;
using PayFlowHub.Contracts.Http;
using PayFlowHub.Contracts.Payments;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

var idempotencyCache = new ConcurrentDictionary<string, IdempotencyRecord>(StringComparer.Ordinal);
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

var descriptor = new PlatformServiceDescriptor(
    Service: "payflowhub-api-gateway",
    BoundedContext: "Edge / Unified API",
    Description: "Public REST edge that will expose the unified merchant-facing payment API.",
    Capabilities:
    [
        "request normalization",
        "operational headers policy",
        "edge health visibility"
    ],
    Dependencies:
    [
        "Payment Orchestrator",
        "Routing Engine"
    ]);

app.MapGet("/", () => Results.Ok(new
{
    descriptor.Service,
    descriptor.BoundedContext,
    descriptor.Description
}));

app.MapGet("/health/live", () => Results.Ok(new { status = "live" }));

app.MapGet("/health/ready", () => Results.Ok(
    new ServiceStatusResponse(
        descriptor,
        Status: "ready",
        ApiVersion: ApiVersions.PublicV1,
        UtcTimestamp: DateTimeOffset.UtcNow)));

app.MapGet("/api/meta/http-contract", () => Results.Ok(new
{
    apiVersion = ApiVersions.PublicV1,
    correlationIdHeader = PlatformHeaders.CorrelationId,
    idempotencyKeyHeader = PlatformHeaders.IdempotencyKey
}));

app.MapPost("/api/v1/payments", async (HttpRequest request) =>
{
    if (!request.Headers.TryGetValue(PlatformHeaders.CorrelationId, out var correlationHeader) ||
        string.IsNullOrWhiteSpace(correlationHeader))
    {
        return Results.BadRequest(new { error = $"Missing required header: {PlatformHeaders.CorrelationId}" });
    }

    if (!request.Headers.TryGetValue(PlatformHeaders.IdempotencyKey, out var idempotencyHeader) ||
        string.IsNullOrWhiteSpace(idempotencyHeader))
    {
        return Results.BadRequest(new { error = $"Missing required header: {PlatformHeaders.IdempotencyKey}" });
    }

    using var reader = new StreamReader(request.Body, Encoding.UTF8);
    var rawBody = await reader.ReadToEndAsync();
    if (string.IsNullOrWhiteSpace(rawBody))
    {
        return Results.BadRequest(new { error = "Request body is required." });
    }

    CreatePaymentRequest? command;
    try
    {
        command = JsonSerializer.Deserialize<CreatePaymentRequest>(rawBody, jsonOptions);
    }
    catch (JsonException)
    {
        return Results.BadRequest(new { error = "Request body contains invalid JSON." });
    }

    if (command is null ||
        string.IsNullOrWhiteSpace(command.MerchantId) ||
        string.IsNullOrWhiteSpace(command.OrderId) ||
        command.AmountMinor <= 0 ||
        string.IsNullOrWhiteSpace(command.Currency) ||
        command.Currency.Length != 3)
    {
        return Results.BadRequest(new
        {
            error = "Payload validation failed. merchantId, orderId, amountMinor > 0, currency (ISO-4217, 3 chars) are required."
        });
    }

    var normalizedIdempotencyKey = idempotencyHeader.ToString().Trim();
    var payloadHash = ComputeSha256(rawBody);

    if (idempotencyCache.TryGetValue(normalizedIdempotencyKey, out var existing))
    {
        if (!string.Equals(existing.PayloadHash, payloadHash, StringComparison.Ordinal))
        {
            return Results.Conflict(new
            {
                error = "Idempotency key re-used with a different payload."
            });
        }

        return Results.Ok(existing.Response);
    }

    var response = new CreatePaymentResponse(
        PaymentId: $"pay_{Guid.NewGuid():N}",
        Status: "accepted",
        ApiVersion: ApiVersions.PublicV1,
        CorrelationId: correlationHeader.ToString(),
        IdempotencyKey: normalizedIdempotencyKey);

    idempotencyCache[normalizedIdempotencyKey] = new IdempotencyRecord(payloadHash, response);
    return Results.Accepted($"/api/v1/payments/{response.PaymentId}", response);
})
.WithName("CreatePayment")
.WithSummary("Create a payment intent in the unified merchant API.")
.WithDescription("Requires X-Correlation-Id and Idempotency-Key headers. Repeated key with identical payload returns the original response.")
.Produces<CreatePaymentResponse>(StatusCodes.Status202Accepted)
.Produces<CreatePaymentResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status409Conflict);

static string ComputeSha256(string input)
{
    var inputBytes = Encoding.UTF8.GetBytes(input);
    var hashBytes = SHA256.HashData(inputBytes);
    return Convert.ToHexString(hashBytes);
}

app.Run();

public partial class Program;

file sealed record IdempotencyRecord(string PayloadHash, CreatePaymentResponse Response);
