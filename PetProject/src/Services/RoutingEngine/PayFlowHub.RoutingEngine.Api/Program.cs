using PayFlowHub.Contracts.Diagnostics;
using PayFlowHub.Contracts.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var descriptor = new PlatformServiceDescriptor(
    Service: "payflowhub-routing-engine",
    BoundedContext: "Routing Intelligence",
    Description: "Provider selection engine that will evaluate rules, weights, and health state.",
    Capabilities:
    [
        "provider scoring",
        "fallback planning",
        "routing audit trail"
    ],
    Dependencies:
    [
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

app.MapGet("/internal/meta/routing-model", () => Results.Ok(new
{
    scoringInputs =
    new[]
    {
        "priority",
        "weight",
        "health score",
        "capability match"
    }
}));

app.Run();

public partial class Program;
