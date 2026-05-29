param(
    [string]$ProjectRoot = (Split-Path -Parent $PSScriptRoot)
)

$ErrorActionPreference = "Stop"

Push-Location $ProjectRoot

try {
    dotnet restore .\PayFlowHub.sln
    dotnet build .\PayFlowHub.sln --no-restore
    dotnet run --project .\tests\PayFlowHub.StructureChecks\PayFlowHub.StructureChecks.csproj --no-build
}
finally {
    Pop-Location
}
