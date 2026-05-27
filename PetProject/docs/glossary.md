# Glossary

- `Merchant` — the client system integrating with the platform.
- `Payment` — the top-level business object representing an attempt to move money from payer to merchant.
- `Payment Attempt` — one concrete provider-facing execution attempt for a payment.
- `Provider` — an external payment service provider or acquiring adapter.
- `Authorization` — reserving funds before capture.
- `Capture` — collecting funds for a prior authorization.
- `Refund` — returning previously captured funds.
- `Idempotency Key` — a client-supplied key that makes repeated requests safe.
- `Correlation ID` — an operational identifier used to trace one request across services.
- `Outbox` — a persistence-backed buffer that prevents dual-write inconsistencies between state changes and event publishing.
- `Inbox` — a deduplication mechanism for consumed messages.
- `Saga` — a long-running workflow coordinating multiple steps with retries and compensations.
- `DLQ` — dead-letter queue for messages that could not be processed successfully.
