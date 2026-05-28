# Module 0: Fintech Foundations

## Why This Exists

Before building payment software, you need the mental model of what the system is trying to control.

If this foundation is weak, every later technical choice becomes cargo-cult engineering.

## Core Payment Terms

### Authorization

The customer initiates a payment and the system asks a provider to reserve funds.

Money is not necessarily fully moved yet. The main purpose is to confirm that the payer can pay.

### Capture

After authorization, the system confirms that funds should actually be collected.

Some businesses capture immediately. Others capture later when goods are shipped or a service is confirmed.

### Settlement

Settlement is the downstream movement and reconciliation of money between participants.

In real systems, this is often more complex than the merchant-facing API suggests.

### Refund

A refund returns money after capture.

Refund logic is not just “negative payment”. It often has different provider rules, timelines, failure modes, and audit expectations.

### Chargeback

A chargeback is usually initiated by the payer via banking/card rails rather than by the merchant.

Chargebacks are operationally and financially important because they create dispute workflows and risk signals.

### Recurring Payment

Recurring payments reuse a prior customer agreement and typically require scheduling, retry logic, and lifecycle state beyond a single request/response.

## Participants In A Payment System

- `Customer` — the person paying.
- `Merchant` — the business receiving funds.
- `Payment Platform` — the software layer we are building.
- `PSP / Acquirer / Processor` — the provider or rail adapter moving payment requests further downstream.
- `Operator` — an internal human who investigates incidents, failures, delays, or anomalies.

## Why Fintech Systems Are Strict

Payment systems are stricter than many CRUD applications because:

- duplicated requests can move money twice;
- partial failures can create disagreement between systems;
- audit trails matter for finance, compliance, and support;
- timing matters because timeouts do not always mean failure;
- provider behavior is inconsistent and often degraded in realistic ways.

## First Learning Outcome

After this module, the student should understand:

- why payment systems are stateful;
- why money movement requires stronger guarantees than ordinary APIs;
- why patterns like idempotency, outbox, saga, and audit logs appear later in the course.
