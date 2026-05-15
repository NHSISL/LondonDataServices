---
name: the-standard-reacttypescript-errors
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.ts", "*.tsx"]
depends-on: ["the-standard-reacttypescript-services", "the-standard-reacttypescript-view-services", "the-standard-reacttypescript-components"]
---

# The Standard React + TypeScript + Vite — Error Handling

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All TypeScript and TSX files — brokers, services, hooks, pages, and components.
0.1/ Who: Any engineer writing or reviewing error handling in a Standard frontend project.
0.2/ What: Governs how errors flow through each layer — brokers expose, services localize, view services convert, pages display, components render — and prohibits silent swallowing and raw infrastructure error inspection.
0.3/ Applies to: `*.ts`, `*.tsx`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-services, the-standard-reacttypescript-view-services, the-standard-reacttypescript-components

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Brokers must expose external failures by throwing — see rules/rules.md#tsr-errors-001
  2. Foundation services must localize broker exceptions into domain exceptions — see rules/rules.md#tsr-errors-002
  3. View services may convert service errors into view-friendly error models — see rules/rules.md#tsr-errors-003
  4. Pages decide how to display errors — see rules/rules.md#tsr-errors-004
  5. Log unexpected errors through a logging broker — see rules/rules.md#tsr-errors-007

1.1/ Don'ts:
  1. Never let components inspect raw infrastructure errors (`error.status === 404`) — see rules/rules.md#tsr-errors-005 and validations/anti-patterns.md#raw-error-in-component
  2. Never swallow errors silently — see rules/rules.md#tsr-errors-006

1.2/ Ask:
  - Ask when it is unclear whether an error should be localized in the service or propagated to the page.

1.3/ Defaults:
  - Components receive an error model or a view model error field — never a raw `Error` or HTTP response.
  - Pages own the decision of whether to show an error summary, redirect, or retry.

1.4/ Examples:
  - ✅ see examples/good/example_good_errors.ts
  - ❌ see examples/bad/example_bad_errors.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Typed error handling at each architectural layer with no silent catches and no raw error inspection in components.
2.1/ Outcome: Errors are explicit, layer-appropriate, and rendered through structured error models — never silently lost.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-errors-005). No prose justification unless asked.
