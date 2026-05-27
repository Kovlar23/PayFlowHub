namespace PayFlowHub.Contracts.Diagnostics;

public sealed record PlatformServiceDescriptor(
    string Service,
    string BoundedContext,
    string Description,
    string[] Capabilities,
    string[] Dependencies);
