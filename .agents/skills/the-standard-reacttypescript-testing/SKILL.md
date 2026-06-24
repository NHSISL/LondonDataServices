---
name: the-standard-reacttypescript-testing
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.test.ts", "*.test.tsx", "*.spec.ts", "*.spec.tsx"]
depends-on: ["the-standard-reacttypescript-services", "the-standard-reacttypescript-view-services", "the-standard-reacttypescript-components", "the-standard-reacttypescript-pages"]
---

# The Standard React + TypeScript + Vite — Testing

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All test files (`*.test.ts`, `*.test.tsx`, `*.spec.ts`, `*.spec.tsx`).
0.1/ Who: Any engineer writing or reviewing tests in a Standard frontend project.
0.2/ What: Governs what each layer must test, where mocks are permitted, prohibitions on snapshot overuse and React-internal testing, and component testing through visible behavior.
0.3/ Applies to: `*.test.ts`, `*.test.tsx`, `*.spec.ts`, `*.spec.tsx`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-services, the-standard-reacttypescript-view-services, the-standard-reacttypescript-components, the-standard-reacttypescript-pages

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Foundation services must have unit tests for logic, validation, and exception paths — see rules/rules.md#tsr-testing-002
  2. View services must have unit tests for aggregation and view model shaping — see rules/rules.md#tsr-testing-003
  3. Components must be tested through visible behavior — see rules/rules.md#tsr-testing-004
  4. Pages must be tested for all route-level states (loading, error, empty, success) — see rules/rules.md#tsr-testing-005
  5. Use mocks only at architectural boundaries — see rules/rules.md#tsr-testing-008

1.1/ Don'ts:
  1. Never test React internals (state variable names, hook call counts, internal refs) — see rules/rules.md#tsr-testing-006
  2. Never use large snapshot tests as primary proof of correctness — see rules/rules.md#tsr-testing-007

1.2/ Ask:
  - Ask when it is unclear whether a dependency should be mocked or tested through integration.

1.3/ Defaults:
  - Vitest for unit tests. React Testing Library for component tests. Playwright for e2e.
  - Mocks are placed at the broker boundary for service tests and at the service boundary for component/page tests.

1.4/ Examples:
  - ✅ see examples/good/example_good_service_test.ts
  - ✅ see examples/good/example_good_component_test.tsx
  - ❌ see examples/bad/example_bad_test.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Vitest / React Testing Library tests covering each layer's responsibility through public behavior.
2.1/ Outcome: Each architectural layer has tests that verify its specific responsibility without testing other layers' internals.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-testing-007). No prose justification unless asked.
