# Module 2: Developer Workspace And Monorepo Basics

## Why This Module Starts With Structure

Beginners often want to start with controllers, databases, and message brokers.

That instinct is understandable, but in a fintech system it creates a hidden mess very quickly.

If the repository does not teach boundaries early, later improvements become much harder:

- contracts start leaking domain behavior;
- API code starts owning business rules;
- shared code turns into a random dump;
- build and verification discipline appears too late.

This module creates the smallest useful `.NET` workspace that still teaches separation of concerns.

## What Exists After This Step

- a real `.NET` solution file;
- a minimal executable API process;
- a `BuildingBlocks` project for cross-cutting defaults;
- a `Contracts` project for transport-safe records;
- a `Payments.Domain` project reserved for future domain logic;
- a simple verification harness that works even in an offline environment.

## Why The Test Harness Is Intentionally Simple

The first checks do not use `xUnit`, `NUnit`, or `MSTest`.

That is intentional for two reasons:

1. this repository currently works in a restricted offline environment, so external test packages are unreliable;
2. the course wants the student to first understand **what** should be verified before introducing richer testing frameworks.

Later modules will replace or extend this simple harness with stronger automated tests.

## Learning Outcome

After this module, the student should understand:

- why one solution can contain multiple bounded projects without becoming a big ball of mud;
- why contracts should be separated from domain behavior;
- why a shared project must stay small and disciplined;
- why even a simple repository should have runnable checks from day one.
