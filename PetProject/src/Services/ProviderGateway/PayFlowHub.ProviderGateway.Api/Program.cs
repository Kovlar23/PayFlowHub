using PayFlowHub.Contracts.Diagnostics;
using PayFlowHub.Contracts.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var descriptor = new PlatformServiceDescriptor(
    Service: "payflowhub-provider-gateway",
    BoundedContext: "Provider Connectivity",
    Description: "Internal gateway that will normalize provider-specific execution contracts and failure modes.",
    Capabilities:
    [
        "provider normalization",
        "mock provider hosting",
        "transport abstraction"
    ],
    Dependencies:
    [
        "Mock PSP A",
        "Mock PSP B"
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

app.MapGet("/internal/meta/providers", () => Results.Ok(new
{
    providers =
    new[]
    {
        new { code = "mock-psp-a", profile = "success-first" },
        new { code = "mock-psp-b", profile = "timeout-and-partial-failure" }
    }
}));

app.Run();

public partial class Program;
