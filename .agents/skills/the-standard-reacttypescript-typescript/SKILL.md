---
name: the-standard-reacttypescript-typescript
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.ts", "*.tsx"]
depends-on: ["the-standard-reacttypescript-files"]
---

# The Standard React + TypeScript + Vite — TypeScript

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All TypeScript and TSX source files in a Standard-compliant frontend project.
0.1/ Who: Any engineer writing or reviewing TypeScript in a Standard React + TypeScript + Vite project.
0.2/ What: Governs strict TypeScript configuration, type usage at boundaries, `type` vs `interface` conventions, prohibition of `any`, export style, and barrel file hygiene.
0.3/ Applies to: `*.ts`, `*.tsx`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-files

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Enable strict TypeScript compiler settings — see rules/rules.md#tsr-typescript-001
  2. Use explicit domain types at architectural boundaries — see rules/rules.md#tsr-typescript-002
  3. Use `type` for data shapes and `interface` for behavioral contracts — see rules/rules.md#tsr-typescript-003
  4. Use `unknown` at unsafe boundaries and narrow to known models — see rules/rules.md#tsr-typescript-004
  5. Prefer named exports for services, brokers, models, hooks, and utilities — see rules/rules.md#tsr-typescript-005

1.1/ Don'ts:
  1. Never use `any` — see validations/anti-patterns.md#any-usage
  2. Never use barrel files that hide dependency direction — see rules/rules.md#tsr-typescript-006 and validations/anti-patterns.md#barrel-files
  3. Never use implicit return types at architectural boundaries — see validations/anti-patterns.md#implicit-return-types

1.2/ Ask:
  - Ask when it is unclear whether a boundary type should be `type` or `interface`.

1.3/ Defaults:
  - `type` is the default for all data-shape declarations.
  - `interface` is used when the declaration describes a callable contract (service, broker).
  - Pages and components may use default exports when routing conventions require it.

1.4/ Examples:
  - ✅ see examples/good/example_good_typescript.ts
  - ❌ see examples/bad/example_bad_typescript.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Corrected TypeScript source with strict settings, explicit types, and correct export style.
2.1/ Outcome: All code is type-safe, boundary types are explicit, and `any` is absent.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-typescript-001). No prose justification unless asked.
