# Bounded Contexts

## Edge / Unified API

Owned by `ApiGateway`.

Responsibilities:

- expose a stable merchant-facing REST API;
- enforce cross-cutting request standards;
- separate public contracts from internal service topology.

## Payment Lifecycle

Owned by `PaymentOrchestrator`.

Responsibilities:

- own the `Payment`, `Refund`, and future `Subscription` lifecycles;
- enforce domain invariants;
- emit lifecycle events for downstream consumers.

## Routing Intelligence

Owned by `RoutingEngine`.

Responsibilities:

- evaluate provider fitness for a payment attempt;
- account for priorities, weights, and health state;
- record why each routing decision was made.

## Provider Connectivity

Owned by `ProviderGateway`.

Responsibilities:

- normalize provider-specific contracts and failure modes;
- expose a stable internal execution surface;
- host mock providers for learning and testing scenarios.

## Operations Visibility

Owned by the future `Ops Dashboard`.

Responsibilities:

- show payment state, attempts, and workflow timelines;
- surface queue lag, provider health, and degraded paths;
- support incident diagnostics without exposing internal service coupling.
