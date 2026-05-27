# ADR 0001: Payment-Orchestrator-Centric System Shape

## Status

Accepted

## Context

The project needs to teach realistic payment-platform concerns while still staying small enough for iterative daily implementation. The system should make routing, provider behavior, asynchronous workflows, and operational diagnostics explicit.

## Decision

Adopt a platform shape with four main backend components:

- `API Gateway`
- `Payment Orchestrator`
- `Routing Engine`
- `Provider Gateway`

Keep `Payment Orchestrator` as the center of lifecycle ownership. It is responsible for state transitions and workflow coordination. `Routing Engine` provides decision support but does not own payment lifecycle state. `Provider Gateway` normalizes PSP integrations and hides provider-specific quirks from the orchestrator.

## Consequences

### Positive

- Payment invariants stay in one place.
- Routing logic can evolve independently from provider integration details.
- Provider mocks and failure simulation become easy to isolate.
- The public API can stay stable while internal service boundaries evolve.

### Negative

- The orchestrator becomes a critical dependency and must be kept disciplined.
- Some flows require extra hops compared to a simpler layered monolith.
- Local development is more operationally expensive than a single service.

## Rejected Alternatives

### CRUD-Centric Modular Monolith As The Primary Shape

Rejected for the first version because it hides the boundaries that matter most for routing, provider abstraction, and asynchronous money movement.

### Provider-Owned Payment State

Rejected because it makes invariant enforcement and cross-provider fallback logic harder to reason about and test.
