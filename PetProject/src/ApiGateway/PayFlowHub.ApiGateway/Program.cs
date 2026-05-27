using PayFlowHub.Contracts.Diagnostics;
using PayFlowHub.Contracts.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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

app.Run();

public partial class Program;
