---
name: the-standard-reacttypescript-hooks
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/hooks/**/*", "use*.ts"]
depends-on: ["the-standard-reacttypescript-view-services", "the-standard-reacttypescript-pages"]
---

# The Standard React + TypeScript + Vite — Hooks

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/hooks/**/*` and any `use*.ts` file co-located with a page or component.
0.1/ Who: Any engineer creating or reviewing custom React hooks in a Standard frontend project.
0.2/ What: Governs what hooks are — React integration mechanisms for lifecycle and state — and what they must not be — service replacements or containers for business rules.
0.3/ Applies to: `src/hooks/**/*`, `use*.ts`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-view-services, the-standard-reacttypescript-pages

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. A hook may manage React lifecycle and state — see rules/rules.md#tsr-hooks-001
  2. A page hook may call a view service to retrieve view models — see rules/rules.md#tsr-hooks-004
  3. Effects that perform async work must protect against stale updates using mounted flags or AbortController — see rules/rules.md#tsr-hooks-005

1.1/ Don'ts:
  1. Never use a hook to replace a foundation service — see rules/rules.md#tsr-hooks-002 and validations/anti-patterns.md#hook-replacing-service
  2. Never put core business rules inside a hook — see rules/rules.md#tsr-hooks-003
  3. Never use `useEffect` for data transformation that belongs in a view service — see rules/rules.md#tsr-hooks-006
  4. Never suppress hook dependency warnings without a documented reason — see rules/rules.md#tsr-hooks-007

1.2/ Ask:
  - Ask when it is unclear whether logic belongs in a hook or a view service.

1.3/ Defaults:
  - A page hook manages state (`isLoading`, `error`, data) and calls the view service inside a `useEffect`.
  - A component hook manages local UI state (modal open/closed, form input, dropdown state).

1.4/ Examples:
  - ✅ see examples/good/example_good_hook.ts
  - ❌ see examples/bad/example_bad_hook.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TypeScript hook file (`use{Purpose}.ts`) returning typed state for consumption by pages or components.
2.1/ Outcome: Hooks integrate React lifecycle with view services — they do not own business logic.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-hooks-002). No prose justification unless asked.
