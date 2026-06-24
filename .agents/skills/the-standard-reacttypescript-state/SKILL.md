---
name: the-standard-reacttypescript-state
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.ts", "*.tsx"]
depends-on: ["the-standard-reacttypescript-hooks", "the-standard-reacttypescript-view-services"]
---

# The Standard React + TypeScript + Vite — State Management

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All TypeScript and TSX files that declare or consume React state.
0.1/ Who: Any engineer making decisions about where state lives in a Standard frontend project.
0.2/ What: Governs where state lives (local, URL, component, application), prohibits state duplication, and requires that business-meaningful state transitions go through services.
0.3/ Applies to: `*.ts`, `*.tsx`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-hooks, the-standard-reacttypescript-view-services

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Keep state as local as possible — see rules/rules.md#tsr-state-001
  2. Use URL state for route-significant state (shareable tabs, bookmarkable filters, route-relevant pagination) — see rules/rules.md#tsr-state-003
  3. Use component state for local UI state (modal open/closed, pre-submit input, dropdown) — see rules/rules.md#tsr-state-004
  4. Use application state only for cross-cutting concerns (authenticated user, global theme, feature flags) — see rules/rules.md#tsr-state-005
  5. State transitions with business meaning must go through services — see rules/rules.md#tsr-state-007

1.1/ Don'ts:
  1. Never duplicate server data across multiple independent component states — see rules/rules.md#tsr-state-002 and validations/anti-patterns.md#duplicated-server-state
  2. Never use global state to avoid designing props — see rules/rules.md#tsr-state-006

1.2/ Ask:
  - Ask when it is unclear whether state should be URL state or component state.
  - Ask when it is unclear whether a state transition has business meaning.

1.3/ Defaults:
  - Component state is the default. Elevate only when cross-component sharing is genuinely required.
  - Server data belongs in the page hook, not duplicated into child component state.

1.4/ Examples:
  - ✅ see examples/good/example_good_state.tsx
  - ❌ see examples/bad/example_bad_state.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: State decisions documented in code through clear ownership (hook, component, context, URL).
2.1/ Outcome: State has a clear single owner at the appropriate scope. No duplication. Business transitions use services.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-state-006). No prose justification unless asked.
