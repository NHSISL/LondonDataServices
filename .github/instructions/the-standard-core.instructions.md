---
applyTo: "**/*.cs"
---

# The Standard — Core

## Applies To
All C# files in any Standard-compliant project — every architectural layer.

## Rules — Do
- Every component must serve a single, clearly defined purpose (ts-core-001)
- Model every real-world concept as a tri-nature entity: Purpose / Dependency / Exposure (ts-core-002)
- Simulate behavior through tests before implementing it (ts-core-003)
- Name every type, method, and variable in plain, non-abbreviated English (ts-core-004)
- Brokers are pure infrastructure; services are pure logic — enforce this separation (ts-core-005)
- Every model must reflect a real-world concept (ts-core-006)
- No component may take on responsibilities outside its layer (ts-core-007)

## Rules — Do Not
- Must not create "God objects" or services with mixed responsibilities (ts-core-001)
- Must not use abbreviations, acronyms, or meaningless names (ts-core-002)
- Must not skip the simulation (test-first) step (ts-core-003)
- Must not bypass layer boundaries — controllers must not call brokers directly (ts-core-004)

## Defaults
- When a component's layer is ambiguous, default to the lowest layer that satisfies the requirement.
- When naming is ambiguous, use the terminology from the problem domain, not the technical domain.
