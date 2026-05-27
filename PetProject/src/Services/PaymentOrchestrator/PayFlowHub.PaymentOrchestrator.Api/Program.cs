using PayFlowHub.Contracts.Diagnostics;
using PayFlowHub.Contracts.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var descriptor = new PlatformServiceDescriptor(
    Service: "payflowhub-payment-orchestrator",
    BoundedContext: "Payment Lifecycle",
    Description: "Lifecycle owner for payment, refund, and future subscription state.",
    Capabilities:
    [
        "payment lifecycle ownership",
        "domain invariants",
        "workflow coordination"
    ],
    Dependencies:
    [
        "Routing Engine",
        "Provider Gateway"
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
        ApiVersion: ApiVersions.InternalV1,
        UtcTimestamp: DateTimeOffset.UtcNow)));

app.MapGet("/internal/meta/workflows", () => Results.Ok(new
{
    sagas =
    new[]
    {
        "Payment Authorization Saga",
        "Refund Saga",
        "Recurring Charge Saga"
    }
}));

app.Run();

public partial class Program;
