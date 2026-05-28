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

API Gateway now also contains the first unified public payment contract:

- `POST /api/v1/payments` with explicit `X-Correlation-Id` and `Idempotency-Key` handling;
- contract-oriented endpoint metadata for the first public payment creation flow;
- baseline request/response DTO contracts for payment creation.

Payment Orchestrator now also contains the first domain model baseline:

- `Payment` aggregate root with lifecycle transitions;
- `Refund` child entity with approval and rejection rules;
- `Money` and `PaymentMethodSnapshot` value objects;
- unit tests for the most important invariants.

## What To Inspect

- [docs/adr/0002-unified-create-payment-contract.md](</D:\VS_projects\C#\PetProject\PetProject\docs\adr\0002-unified-create-payment-contract.md>)
- [docs/adr/0003-payment-domain-model-boundaries.md](</D:\VS_projects\C#\PetProject\PetProject\docs\adr\0003-payment-domain-model-boundaries.md>)
- [src/ApiGateway/PayFlowHub.ApiGateway/Program.cs](</D:\VS_projects\C#\PetProject\PetProject\src\ApiGateway\PayFlowHub.ApiGateway\Program.cs>)
- [src/Services/PaymentOrchestrator/PayFlowHub.PaymentOrchestrator.Api/Domain/Payments/Payment.cs](</D:\VS_projects\C#\PetProject\PetProject\src\Services\PaymentOrchestrator\PayFlowHub.PaymentOrchestrator.Api\Domain\Payments\Payment.cs>)
- [tests/PayFlowHub.ApiGateway.ContractTests/CreatePaymentContractTests.cs](</D:\VS_projects\C#\PetProject\PetProject\tests\PayFlowHub.ApiGateway.ContractTests\CreatePaymentContractTests.cs>)
- [tests/PayFlowHub.PaymentOrchestrator.UnitTests/PaymentDomainTests.cs](</D:\VS_projects\C#\PetProject\PetProject\tests\PayFlowHub.PaymentOrchestrator.UnitTests\PaymentDomainTests.cs>)

## How To Run This Stage

Restore and build:

```powershell
dotnet restore PayFlowHub.sln
dotnet build PayFlowHub.sln --configuration Release --no-restore
```

Run all current tests:

```powershell
dotnet test PayFlowHub.sln --configuration Release --no-build
```

Run API Gateway locally:

```powershell
dotnet run --project src/ApiGateway/PayFlowHub.ApiGateway/PayFlowHub.ApiGateway.csproj
```

Try the first payment endpoint:

```powershell
$headers = @{
  "X-Correlation-Id" = "corr-001"
  "Idempotency-Key" = "idem-001"
}

$body = @{
  merchantId = "merchant-1"
  orderId = "order-001"
  amountMinor = 1250
  currency = "USD"
  description = "test payment"
} | ConvertTo-Json

Invoke-RestMethod `
  -Method Post `
  -Uri "http://localhost:5000/api/v1/payments" `
  -Headers $headers `
  -Body $body `
  -ContentType "application/json"
```

## How It Works Right Now

- API Gateway validates required operational headers and the minimal payment payload.
- A normalized `Idempotency-Key` is stored in an in-memory cache together with the first accepted response.
- Repeating the same key with the same payload returns the original payment response.
- Reusing the same key with a different payload returns `409 Conflict`.
- Payment Orchestrator domain code is not wired into the endpoint yet; at this stage it is validated independently through unit tests.

## Next Steps

1. Wire API Gateway create payment contract to the orchestrator command surface.
2. Add EF Core persistence mappings and first migrations (PostgreSQL).
3. Persist idempotency state outside process memory and introduce canonical request hashing.
