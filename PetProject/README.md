# PayFlow Hub Workspace

`PayFlow Hub` is a learning-focused payment platform repo organized as a .NET monorepo workspace.

## Scope Of This Workspace

This folder contains:

- architecture and domain documents for the platform;
- the .NET solution scaffold for the backend services;
- shared contracts that define cross-service HTTP conventions;
- the first runnable service entrypoints for the main bounded contexts.

## Solution Layout

```text
.
├── PayFlowHub.sln
├── Directory.Build.props
├── .editorconfig
├── docs/
│   ├── adr/
│   ├── bounded-contexts.md
│   ├── domain-map.md
│   ├── glossary.md
│   ├── use-cases.md
│   └── vision.md
└── src/
    ├── ApiGateway/
    ├── BuildingBlocks/
    └── Services/
```

## Current Status

The repository now has the architectural baseline and the first service scaffold:

- `PayFlowHub.ApiGateway`
- `PayFlowHub.PaymentOrchestrator.Api`
- `PayFlowHub.RoutingEngine.Api`
- `PayFlowHub.ProviderGateway.Api`
- `PayFlowHub.Contracts`

## Next Steps

1. Introduce public REST contracts with idempotency and correlation requirements.
2. Model `Payment` and `Refund` in the orchestrator domain.
3. Add persistence, migrations, and the first integration tests.
