namespace PayFlowHub.Contracts.Diagnostics;

public sealed record ServiceStatusResponse(
    PlatformServiceDescriptor Service,
    string Status,
    string ApiVersion,
    DateTimeOffset UtcTimestamp);
