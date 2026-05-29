namespace PayFlowHub.Contracts;

public sealed record WorkspaceOverviewContract(
    string CourseStage,
    string CurrentModule,
    string Goal,
    IReadOnlyList<WorkspaceModuleContract> Modules);
