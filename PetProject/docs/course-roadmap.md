# PayFlow Hub Course Roadmap

## Course Format

This pet project is treated as a practical engineering course for a beginner.

Each stage should behave like a mini-lecture plus a hands-on lab:

- theory first;
- small amount of code or structure second;
- verification and reflection third.

Every future stage should produce:

- a focused implementation step;
- a commit;
- a stage note in `docs/stages/`;
- clear run instructions;
- an explanation of why the chosen design exists;
- a comparison between the simple version and the better version.

## Module Sequence

### Module 0 — Fintech Foundations

Learn:

- what a payment is;
- what authorization, capture, settlement, refund, chargeback, and recurring payment mean;
- who participates in a payment flow;
- why fintech systems care about reliability, auditability, and idempotency.

### Module 1 — System Vision And Language

Learn:

- why `PayFlow Hub` exists;
- what business capabilities it must provide;
- what bounded contexts exist;
- how to build a shared glossary before writing code.

### Module 2 — Developer Workspace And Monorepo Basics

Learn:

- how a `.NET` monorepo can be structured;
- why shared contracts and service boundaries matter;
- what CI, code style, and build discipline do for a backend team.

First practical baseline:

- create the solution and starter project graph;
- add one runnable API host;
- add one offline-friendly verification harness;
- document why this is intentionally simpler than a full production setup.

### Module 3 — Unified Payment API

Learn:

- why public API design matters in payments;
- how request versioning works;
- why `Correlation ID` and `Idempotency Key` exist;
- how to model a first payment creation contract.

### Module 4 — Payment Domain Modeling

Learn:

- aggregate roots, entities, value objects, invariants;
- why `Payment` state transitions must be explicit;
- why a domain model should not leak transport concerns.

### Module 5 — Persistence And Transaction Boundaries

Learn:

- why PostgreSQL is used;
- how EF Core mappings work;
- how migrations and transaction boundaries influence design;
- common ORM mistakes in business-critical systems.

### Module 6 — Provider Connectivity

Learn:

- why PSP adapters are isolated;
- what normalization means for provider APIs;
- how mock providers help engineering quality.

### Module 7 — Routing Decisions

Learn:

- why provider routing exists;
- how priorities, weights, health, and fallback work;
- why auditability matters for routing decisions.

### Module 8 — Business Events

Learn:

- what Kafka is good for;
- why outbox/inbox patterns exist;
- why dual-write bugs are dangerous.

### Module 9 — Operational Task Queues

Learn:

- why RabbitMQ is different from Kafka;
- how retries, DLQ, and polling jobs work;
- common queue anti-patterns.

### Module 10 — Long-Running Workflows

Learn:

- when a saga is justified;
- what Camunda adds;
- why compensation logic matters in payments.

### Module 11 — Cache And Fast Coordination

Learn:

- when Redis/Valkey helps;
- when it does not help;
- how to avoid turning cache into accidental source of truth.

### Module 12 — Ops Dashboard

Learn:

- why operational visibility matters;
- what operators need to see;
- how frontend helps explain backend behavior.

### Module 13 — Resilience And Security

Learn:

- retry vs timeout vs circuit breaker;
- request validation and audit logging;
- secrets handling and operational access boundaries.

### Module 14 — Test Strategy

Learn:

- unit vs integration vs contract vs e2e;
- what to test at each layer;
- why fake coverage is not the goal.

### Module 15 — Delivery And Deployment

Learn:

- Docker Compose basics;
- Kubernetes basics;
- GitHub Actions basics;
- operational packaging of a distributed project.

### Module 16 — Observability

Learn:

- metrics, logs, traces;
- why OpenTelemetry matters;
- how observability changes debugging behavior.

### Module 17 — Product Extensions

Learn:

- recurring payments;
- BNPL / installments;
- evolution pressure on architecture.

### Module 18 — Failure Drills

Learn:

- how systems fail;
- how fallback logic behaves;
- how to run engineering game days.

## Stage Output Template

Each future stage note should contain:

- `Learning Goal`
- `What Was Built`
- `Why This Matters`
- `Theory`
- `Code Walkthrough`
- `How To Run`
- `How To Verify`
- `Common Mistakes`
- `Simpler Version vs Better Version`
- `What To Learn Next`
