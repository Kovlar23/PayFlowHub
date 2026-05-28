# ADR 0002: Unified Create Payment Contract At API Gateway

## Status

Accepted

## Context

Roadmap step 3 requires a real merchant-facing REST contract with explicit idempotency and correlation behavior. Existing scaffold exposed only metadata and health endpoints.

## Decision

Introduce `POST /api/v1/payments` at `PayFlowHub.ApiGateway` with:

- required headers: `X-Correlation-Id` and `Idempotency-Key`;
- payload contract: `merchantId`, `orderId`, `amountMinor`, `currency`, `description`;
- validation on required fields and amount/currency format;
- in-memory idempotency cache keyed by `Idempotency-Key` and guarded by payload hash.

Behavior:

- first valid request returns `202 Accepted` with `CreatePaymentResponse`;
- repeated request with same key and same payload returns original response with `200 OK`;
- repeated request with same key and different payload returns `409 Conflict`.

## Consequences

### Positive

- Gives a concrete, testable API surface for future orchestration integration.
- Enforces platform-level idempotency semantics before domain orchestration is wired.
- Makes contract behavior explicit in endpoint metadata and automated contract tests.

### Negative

- In-memory idempotency is not durable and not suitable for multi-instance production.
- Payload hash is based on raw JSON and may differ for semantically identical but differently formatted payloads.

## Follow-up

Replace in-memory cache with durable idempotency store (PostgreSQL/Redis) and canonical request hashing when persistence layer is introduced.
