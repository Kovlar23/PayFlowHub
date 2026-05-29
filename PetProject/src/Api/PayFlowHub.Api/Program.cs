using PayFlowHub.Api.Workspace;
using PayFlowHub.BuildingBlocks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WorkspaceOverviewFactory>();

var app = builder.Build();

app.MapGet("/", (WorkspaceOverviewFactory factory) =>
{
    var overview = factory.Create();
    return Results.Ok(overview);
});

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "PayFlowHub.Api",
    stage = DeveloperWorkflowDefaults.CourseStage
}));

app.MapGet("/course/workspace", (WorkspaceOverviewFactory factory) =>
{
    var overview = factory.Create();
    return Results.Ok(overview.Modules);
});

app.Run();

public partial class Program;
