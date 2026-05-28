# Module 1: System Vision

## Course-Level Goal

`PayFlow Hub` should become a training platform for understanding how a payment orchestrator is designed, evolved, tested, and operated.

It is not meant to be a fake enterprise buzzword project. It is meant to be a pedagogical system that gradually exposes real engineering concerns.

## Why A Payment Orchestrator

A payment orchestrator is a strong learning shape because it forces you to reason about:

- public API contracts;
- provider abstraction;
- routing decisions;
- asynchronous workflows;
- retries and compensations;
- observability and operational support.

## Core High-Level Components

- `API Gateway` — the public entrypoint for merchant requests.
- `Payment Orchestrator` — the owner of payment lifecycle state.
- `Routing Engine` — the system that decides which provider should be used.
- `Provider Gateway` — the adapter layer hiding provider-specific behavior.
- `Ops Dashboard` — the operational view for understanding what the system is doing.

## Why Not Start With Everything At Once

A beginner does not benefit from starting with Kafka, RabbitMQ, Camunda, Redis, Kubernetes, and React all in one step.

That creates confusion instead of understanding.

The course should instead:

1. teach the payment problem first;
2. teach the system shape second;
3. teach one layer of technology at a time;
4. revisit earlier simple solutions and improve them later.

## First Architectural Principle

The system should evolve from:

- understandable,
- minimal,
- explicit,

to:

- reliable,
- scalable,
- observable,
- production-oriented.

That evolution is itself part of the course.
