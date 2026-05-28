# PayFlow Hub Course Workspace

`PayFlow Hub` is now intentionally reset and restarted as a course-driven fintech engineering pet project.

## What This Folder Is Now

This workspace is no longer treated as a partially-built platform scaffold.

Instead, it is the starting point of a structured learning course where the codebase will be rebuilt from scratch in small stages. Each stage should teach one layer of the system and explain it in plain language for a beginner, while still growing toward a realistic fintech architecture.

## Learning Objective

The goal is not only to end up with a payment platform demo.

The goal is to learn:

- how payments work conceptually;
- how fintech systems are decomposed into services and workflows;
- why specific technologies appear in modern payment stacks;
- how simple implementations evolve into production-grade designs;
- how to reason about trade-offs, operational risk, reliability, and architecture.

## Current Status

The previous implementation was intentionally cleaned up.

The project is restarting from a fresh course baseline and has now completed the conceptual part of `Module 1`:

- no active service implementation yet;
- no active `.NET` solution scaffold yet;
- the course now includes fintech foundations plus system vision artifacts;
- glossary, use cases, bounded contexts, and architecture map are documented;
- a simple documentation verification script now protects the course structure.

## What To Read First

- [docs/course-roadmap.md](</D:\VS_projects\C#\PetProject\PetProject\docs\course-roadmap.md>)
- [docs/modules/00-fintech-foundations.md](</D:\VS_projects\C#\PetProject\PetProject\docs\modules\00-fintech-foundations.md>)
- [docs/modules/01-system-vision.md](</D:\VS_projects\C#\PetProject\PetProject\docs\modules\01-system-vision.md>)
- [docs/modules/01-glossary.md](</D:\VS_projects\C#\PetProject\PetProject\docs\modules\01-glossary.md>)
- [docs/modules/01-use-cases.md](</D:\VS_projects\C#\PetProject\PetProject\docs\modules\01-use-cases.md>)
- [docs/modules/01-bounded-contexts.md](</D:\VS_projects\C#\PetProject\PetProject\docs\modules\01-bounded-contexts.md>)
- [docs/modules/01-architecture-map.md](</D:\VS_projects\C#\PetProject\PetProject\docs\modules\01-architecture-map.md>)
- [docs/stages/2026-05-28-course-reset-baseline.md](</D:\VS_projects\C#\PetProject\PetProject\docs\stages\2026-05-28-course-reset-baseline.md>)

## How This Project Will Grow

The future implementation should follow this pattern:

1. Start with the simplest useful version of a concept.
2. Explain why that version is good for learning.
3. Show its weaknesses and limits.
4. Later replace or evolve it with a stronger production-style design.
5. Explain clearly why the newer design is better and what trade-offs it introduces.

## Next Practical Step

The next stage should begin `Module 2`:

- recreate the `.NET` solution structure;
- define the first project layout;
- introduce code-style and build discipline;
- prepare the repository for contracts and early runtime code.
