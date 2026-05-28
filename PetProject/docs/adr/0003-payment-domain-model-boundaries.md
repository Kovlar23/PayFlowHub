# ADR 0003: Payment Domain Model Boundaries

## Status
Accepted

## Context
Roadmap step 4 requires a first production-oriented domain model for `Payment` and `Refund` inside the Payment Orchestrator bounded context. At this stage the platform still has no database, outbox, or workflow engine integration, but the codebase needs stable invariants before persistence and sagas appear.

## Decision
Model `Payment` as the aggregate root and keep `Refund` as a child entity managed by that aggregate for now.

Key rules encoded in code:
- `Money` must be positive and normalized to a three-letter currency code.
- `Payment` starts in `Created`, can move to `Authorized`, then `Captured`.
- `Captured` or `Refunded` payments cannot be marked as failed.
- Refund requests are allowed only for `Captured` or already `Refunded` payments.
- Refund currency must match the original payment currency.
- Sum of approved/settled refunds cannot exceed the captured payment amount.

## Consequences
This keeps the first model small enough for unit testing and future EF Core mapping, while leaving room to extract `Refund` into a separate aggregate if asynchronous refund processing later requires independent concurrency boundaries.
