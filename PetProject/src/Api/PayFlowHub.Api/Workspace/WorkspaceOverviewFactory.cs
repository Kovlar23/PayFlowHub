using PayFlowHub.BuildingBlocks;
using PayFlowHub.Contracts;
using PayFlowHub.Modules.Payments.Domain;
using BuildingBlocksAssemblyMarker = PayFlowHub.BuildingBlocks.AssemblyMarker;
using PaymentsDomainAssemblyMarker = PayFlowHub.Modules.Payments.Domain.AssemblyMarker;

namespace PayFlowHub.Api.Workspace;

public sealed class WorkspaceOverviewFactory
{
    public WorkspaceOverviewContract Create()
    {
        var modules = new[]
        {
            new WorkspaceModuleContract(
                "api",
                typeof(Program).Assembly.GetName().Name ?? "PayFlowHub.Api",
                "Hosts the first executable ASP.NET Core process and exposes operationally useful starter endpoints.",
                "Module 2 needs a runnable process so the learner can connect solution structure to something concrete."),
            new WorkspaceModuleContract(
                "building-blocks",
                typeof(BuildingBlocksAssemblyMarker).Assembly.GetName().Name ?? "PayFlowHub.BuildingBlocks",
                "Stores cross-cutting defaults and infrastructure-safe primitives shared by many projects.",
                "A monorepo needs one obvious home for small shared conventions before duplication spreads."),
            new WorkspaceModuleContract(
                "contracts",
                typeof(WorkspaceOverviewContract).Assembly.GetName().Name ?? "PayFlowHub.Contracts",
                "Defines transport-friendly records that can be shared without leaking domain behavior.",
                "The course introduces contracts early so API and domain can evolve with cleaner boundaries."),
            new WorkspaceModuleContract(
                PaymentsDomainDescription.ModuleKey,
                typeof(PaymentsDomainAssemblyMarker).Assembly.GetName().Name ?? "PayFlowHub.Modules.Payments.Domain",
                PaymentsDomainDescription.Responsibility,
                "The future payment model needs a reserved project now, even before aggregates are implemented in Module 4.")
        };

        return new WorkspaceOverviewContract(
            DeveloperWorkflowDefaults.CourseStage,
            DeveloperWorkflowDefaults.CurrentModule,
            "Establish a minimal monorepo that builds, runs, and teaches future boundaries without premature complexity.",
            modules);
    }
}
