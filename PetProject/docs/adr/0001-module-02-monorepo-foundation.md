# ADR 0001: Start With A Modular Monorepo Foundation

## Status

Accepted

## Context

`PayFlow Hub` is being rebuilt as a course for a beginner.

The project needs a first executable workspace, but it is still too early to introduce:

- many deployable services;
- real payment APIs;
- persistence;
- messaging infrastructure.

At the same time, the repository must already teach separation between:

- host code;
- transport contracts;
- future domain logic;
- shared building blocks.

## Decision

Start `Module 2` with a single `.NET` solution that contains a few explicitly named projects:

- `PayFlowHub.Api`
- `PayFlowHub.BuildingBlocks`
- `PayFlowHub.Contracts`
- `PayFlowHub.Modules.Payments.Domain`
- `PayFlowHub.StructureChecks`

Use a simple executable verification harness instead of a package-based unit test framework for now.

## Consequences

### Positive

- the learner gets a runnable process immediately;
- future boundaries are visible before complex logic exists;
- scripts and CI can already enforce basic discipline;
- the repository remains compatible with offline execution.

### Negative

- the initial test story is intentionally primitive;
- the solution contains placeholder projects before they have deep behavior;
- some students may mistake project boundaries for full runtime isolation.

## Follow-Up

Future modules should:

- keep `Contracts` free from domain behavior;
- grow `Payments.Domain` with aggregates and invariants in `Module 4`;
- replace or extend structure checks with richer test layers in `Module 14`.
