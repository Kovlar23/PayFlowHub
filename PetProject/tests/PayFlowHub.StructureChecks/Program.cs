using PayFlowHub.Api.Workspace;
using PayFlowHub.BuildingBlocks;
using PayFlowHub.Contracts;
using PayFlowHub.Modules.Payments.Domain;

var failures = new List<string>();

Assert(
    DeveloperWorkflowDefaults.SolutionFileName == "PayFlowHub.sln",
    "The solution file name should stay stable for scripts and documentation.",
    failures);

Assert(
    DeveloperWorkflowDefaults.CurrentModule.Contains("Module 2", StringComparison.Ordinal),
    "The course metadata should point to Module 2 for this stage.",
    failures);

var overview = new WorkspaceOverviewFactory().Create();

Assert(
    overview.Modules.Count == 4,
    "The workspace overview should describe the four starter projects.",
    failures);

Assert(
    overview.Modules.Any(module => module.Key == PaymentsDomainDescription.ModuleKey),
    "The payments domain project should already be visible in the learning topology.",
    failures);

Assert(
    typeof(WorkspaceOverviewContract).Assembly.GetReferencedAssemblies().All(assembly => assembly.Name is not "PayFlowHub.Api"),
    "Contracts must not reference the API assembly.",
    failures);

Assert(
    typeof(PaymentsDomainDescription).Assembly.GetReferencedAssemblies().All(assembly => assembly.Name is not "PayFlowHub.Contracts"),
    "The domain assembly must stay independent from transport contracts at this stage.",
    failures);

if (failures.Count > 0)
{
    foreach (var failure in failures)
    {
        Console.Error.WriteLine($"FAIL: {failure}");
    }

    return 1;
}

Console.WriteLine("PASS: workspace structure checks succeeded.");
Console.WriteLine($"PASS: current stage = {overview.CourseStage}");
Console.WriteLine($"PASS: modules = {string.Join(", ", overview.Modules.Select(module => module.Key))}");
return 0;

static void Assert(bool condition, string message, ICollection<string> failures)
{
    if (!condition)
    {
        failures.Add(message);
    }
}
