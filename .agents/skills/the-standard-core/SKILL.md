---
name: the-standard-core
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*.cs"]
depends-on: ["none"]
---

# The Standard — Core

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All C# projects following The Standard architecture.
0.1/ Who: Engineers designing or reviewing any software component.
0.2/ What: Enforces the foundational theory, purposing, modeling, simulation, and principles of The Standard.
0.3/ Applies to: *.cs
0.4/ Version: v2.50.0
0.5/ Depends on: none

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Every component must serve a single, clearly defined purpose → see rules/rules.md#ts-core-001
  2. Model every real-world concept as a tri-nature entity (Purpose / Dependency / Exposure) → see rules/rules.md#ts-core-002
  3. Simulate behavior through tests before implementing it → see rules/rules.md#ts-core-003
  4. Name every type, method, and variable in plain, non-abbreviated English → see rules/rules.md#ts-core-004
  5. Follow the Purity principle: brokers are pure infrastructure, services are pure logic → see rules/rules.md#ts-core-005
  6. Enforce the Realism principle: every model must reflect a real-world concept → see rules/rules.md#ts-core-006
  7. Enforce the Fitness principle: no component may take on responsibilities outside its layer → see rules/rules.md#ts-core-007

1.1/ Don'ts:
  1. Must not create "God objects" or services with mixed responsibilities → see validations/anti-patterns.md#god-objects
  2. Must not use abbreviations, acronyms (except ubiquitous ones), or meaningless names → see validations/anti-patterns.md#poor-naming
  3. Must not skip the simulation (test-first) step → see validations/anti-patterns.md#no-tdd
  4. Must not bypass layer boundaries (e.g., controller calling a broker directly) → see validations/anti-patterns.md#layer-bypass

1.2/ Ask:
  - Ask when a concept does not map clearly to a known layer (broker / service / exposer).
  - Ask when a business rule feels like it belongs to multiple layers.

1.3/ Defaults:
  - When a component's layer is ambiguous, default to the lowest layer that satisfies the requirement.
  - When naming is ambiguous, use the terminology from the problem domain, not the technical domain.

1.4/ Examples:
  - ✅ see examples/good/example_good_tri_nature.cs
  - ❌ see examples/bad/example_bad_god_service.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code and architectural guidance.
2.1/ Outcome: Components that are purposeful, single-responsibility, correctly layered, and named in plain English.
2.2/ Tone: Direct. Cite rule IDs. No suggestions — violations must be fixed.
