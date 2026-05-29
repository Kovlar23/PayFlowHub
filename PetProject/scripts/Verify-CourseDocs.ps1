param(
    [string]$ProjectRoot = (Split-Path -Parent $PSScriptRoot)
)

$ErrorActionPreference = "Stop"

function Assert-FileExists {
    param([string]$Path)

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Required file is missing: $Path"
    }
}

function Assert-Contains {
    param(
        [string]$Path,
        [string]$Needle
    )

    $content = Get-Content -LiteralPath $Path -Raw
    if ($content -notmatch [regex]::Escape($Needle)) {
        throw "Required text '$Needle' was not found in $Path"
    }
}

$requiredFiles = @(
    (Join-Path $ProjectRoot "README.md"),
    (Join-Path $ProjectRoot "docs/course-roadmap.md"),
    (Join-Path $ProjectRoot "docs/modules/00-fintech-foundations.md"),
    (Join-Path $ProjectRoot "docs/modules/01-system-vision.md"),
    (Join-Path $ProjectRoot "docs/modules/01-glossary.md"),
    (Join-Path $ProjectRoot "docs/modules/01-use-cases.md"),
    (Join-Path $ProjectRoot "docs/modules/01-bounded-contexts.md"),
    (Join-Path $ProjectRoot "docs/modules/01-architecture-map.md"),
    (Join-Path $ProjectRoot "docs/modules/02-workspace-and-monorepo.md")
)

foreach ($file in $requiredFiles) {
    Assert-FileExists -Path $file
}

$stageDirectory = Join-Path $ProjectRoot "docs/stages"
$latestStage = Get-ChildItem -LiteralPath $stageDirectory -File |
    Sort-Object Name -Descending |
    Select-Object -First 1

if ($null -eq $latestStage) {
    throw "No stage note found in $stageDirectory"
}

$requiredStageSections = @(
    "Learning Goal",
    "What Was Built",
    "Why This Matters",
    "Theory",
    "Code Walkthrough",
    "How To Run",
    "How To Verify",
    "Common Mistakes",
    "Simpler Version vs Better Version",
    "What To Learn Next"
)

foreach ($section in $requiredStageSections) {
    Assert-Contains -Path $latestStage.FullName -Needle $section
}

Assert-Contains -Path (Join-Path $ProjectRoot "README.md") -Needle "01-glossary.md"
Assert-Contains -Path (Join-Path $ProjectRoot "README.md") -Needle "02-workspace-and-monorepo.md"
Assert-Contains -Path (Join-Path $ProjectRoot "docs/modules/01-system-vision.md") -Needle "01-bounded-contexts.md"

Write-Host "Course documentation verification passed."
