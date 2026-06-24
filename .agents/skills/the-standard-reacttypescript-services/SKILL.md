---
name: the-standard-reacttypescript-services
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/services/foundations/**/*"]
depends-on: ["the-standard-reacttypescript-brokers", "the-standard-reacttypescript-models"]
---

# The Standard React + TypeScript + Vite — Foundation Services

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/services/foundations/**/*` — foundation service classes, validation partials, exception partials, and interfaces.
0.1/ Who: Any engineer creating or reviewing foundation services in a Standard frontend project.
0.2/ What: Governs foundation service dependency rules, validation before broker calls, exception localization, and prohibition of React imports, JSX, navigation, and state ownership.
0.3/ Applies to: `src/services/foundations/**/*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-brokers, the-standard-reacttypescript-models

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Foundation services must depend only on brokers, models, and approved same-layer dependencies — see rules/rules.md#tsr-foundation-001
  2. Foundation services must validate inputs before calling brokers — see rules/rules.md#tsr-foundation-004
  3. Foundation services must localize external exceptions into domain-specific exceptions — see rules/rules.md#tsr-foundation-005

1.1/ Don'ts:
  1. Never import React, `useState`, `useEffect`, or any React hook in a foundation service — see rules/rules.md#tsr-foundation-002 and validations/anti-patterns.md#react-import
  2. Never return JSX from a foundation service — see rules/rules.md#tsr-foundation-003
  3. Never perform page navigation from a foundation service — see rules/rules.md#tsr-foundation-006
  4. Never own React state in a foundation service — see rules/rules.md#tsr-foundation-007

1.2/ Ask:
  - Ask when it is unclear whether exception wrapping adds meaningful domain context or just re-throws.

1.3/ Defaults:
  - Foundation services split across three files: `{domain}Service.ts`, `{domain}Service.validations.ts`, `{domain}Service.exceptions.ts`.
  - Validation failures throw before any broker call is made.

1.4/ Examples:
  - ✅ see examples/good/example_good_foundation_service.ts
  - ❌ see examples/bad/example_bad_foundation_service.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TypeScript foundation service class with interface, validations, and exceptions partials.
2.1/ Outcome: Foundation services are pure business-logic containers — no React, no JSX, no navigation, no state.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-foundation-002). No prose justification unless asked.
