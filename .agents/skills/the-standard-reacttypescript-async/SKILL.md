---
name: the-standard-reacttypescript-async
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.ts", "*.tsx"]
depends-on: ["the-standard-reacttypescript-hooks", "the-standard-reacttypescript-errors"]
---

# The Standard React + TypeScript + Vite — Async and Cancellation

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All TypeScript and TSX files performing asynchronous operations.
0.1/ Who: Any engineer writing or reviewing async code in a Standard frontend project.
0.2/ What: Governs async page loading state coverage, stale update protection, correct use of `Promise.all`, sequential vs. parallel execution, and rejection handling.
0.3/ Applies to: `*.ts`, `*.tsx`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-hooks, the-standard-reacttypescript-errors

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Async page loading must handle loading, success, empty, and failure states — see rules/rules.md#tsr-async-001
  2. Effects with async work must protect against stale updates — see rules/rules.md#tsr-async-002
  3. Use `Promise.all` only when operations are truly independent — see rules/rules.md#tsr-async-003

1.1/ Don'ts:
  1. Never run dependent async operations in parallel — see rules/rules.md#tsr-async-004 and validations/anti-patterns.md#parallel-dependent
  2. Never ignore rejected promises — see rules/rules.md#tsr-async-005

1.2/ Ask:
  - Ask when it is unclear whether two async operations are truly independent (safe for `Promise.all`) or sequentially dependent.

1.3/ Defaults:
  - Use mounted flags as the default stale update protection pattern.
  - `AbortController` is acceptable for fetch-based brokers.
  - Framework-approved loaders (e.g., React Router `loader`) are acceptable when the framework manages cancellation.

1.4/ Examples:
  - ✅ see examples/good/example_good_async.ts
  - ❌ see examples/bad/example_bad_async.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Async TypeScript with explicit state coverage, stale update protection, and no ignored rejections.
2.1/ Outcome: Async operations are safe, explicit, and resilient — no memory leaks, no silent failures, no stale renders.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-async-004). No prose justification unless asked.
