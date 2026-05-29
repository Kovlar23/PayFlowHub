namespace PayFlowHub.Contracts;

public sealed record WorkspaceModuleContract(
    string Key,
    string AssemblyName,
    string Responsibility,
    string WhyItExistsNow);
