# Stage Note: Unified Contract And Domain Baseline

## What Was Added

- Merchant-facing `POST /api/v1/payments` contract in `ApiGateway`.
- Explicit handling of `X-Correlation-Id` and `Idempotency-Key`.
- Baseline request/response DTOs for payment creation.
- In-memory idempotency behavior for repeated requests.
- First `Payment` and `Refund` domain model inside `PaymentOrchestrator`.
- Unit tests for domain invariants.
- Contract tests for the public payment creation flow.

## What To Inspect

- `docs/adr/0002-unified-create-payment-contract.md`
- `docs/adr/0003-payment-domain-model-boundaries.md`
- `src/ApiGateway/PayFlowHub.ApiGateway/Program.cs`
- `src/BuildingBlocks/PayFlowHub.Contracts/Payments/*`
- `src/Services/PaymentOrchestrator/PayFlowHub.PaymentOrchestrator.Api/Domain/Payments/*`
- `src/Services/PaymentOrchestrator/PayFlowHub.PaymentOrchestrator.Api/Domain/Refunds/*`
- `tests/PayFlowHub.ApiGateway.ContractTests/CreatePaymentContractTests.cs`
- `tests/PayFlowHub.PaymentOrchestrator.UnitTests/PaymentDomainTests.cs`

## How To Run

```powershell
dotnet restore PayFlowHub.sln
dotnet build PayFlowHub.sln --configuration Release --no-restore
dotnet test PayFlowHub.sln --configuration Release --no-build
dotnet run --project src/ApiGateway/PayFlowHub.ApiGateway/PayFlowHub.ApiGateway.csproj
```

## Example Request

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

## How It Works

- The gateway rejects requests that do not provide the required operational headers.
- The request body is deserialized with web-style JSON settings, so camelCase API payloads map cleanly to C# DTOs.
- A hash of the raw JSON payload is stored alongside the first accepted response for a given idempotency key.
- Repeating the same key with the same payload returns the first response.
- Repeating the same key with a different payload returns `409 Conflict`.
- The `Payment` aggregate currently models lifecycle invariants independently from transport and persistence concerns.

## Limits Of This Stage

- Idempotency is process-local and not durable.
- The gateway does not call the orchestrator yet.
- The payment domain is not mapped to EF Core yet.
- OpenAPI generation is intentionally postponed until the package choice and tooling are stabilized.
