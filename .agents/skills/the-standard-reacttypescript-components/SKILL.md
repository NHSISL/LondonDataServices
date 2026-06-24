---
name: the-standard-reacttypescript-components
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/components/**/*"]
depends-on: ["the-standard-reacttypescript-models", "the-standard-reacttypescript-view-services"]
---

# The Standard React + TypeScript + Vite — Components

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/components/**/*` — all shared, layout, form, table, and card components.
0.1/ Who: Any engineer creating or reviewing React components in a Standard frontend project.
0.2/ What: Governs component rendering responsibility, prop usage, prohibition of broker and service calls, business-rule-free JSX, accessibility requirements, and component size discipline.
0.3/ Applies to: `src/components/**/*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-models, the-standard-reacttypescript-view-services

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Components must render UI from props — see rules/rules.md#tsr-components-001
  2. Components must receive data through props unless they are a page-level container — see rules/rules.md#tsr-components-002
  3. Use prop models for non-trivial components — see rules/rules.md#tsr-components-007
  4. Interactive elements must be keyboard-accessible and use semantic HTML — see rules/rules.md#tsr-components-009

1.1/ Don'ts:
  1. Never call brokers from a component — see rules/rules.md#tsr-components-003 and validations/anti-patterns.md#broker-call-in-component
  2. Never call foundation services directly from a component — see rules/rules.md#tsr-components-004
  3. Never embed business rules in JSX — see rules/rules.md#tsr-components-005 and validations/anti-patterns.md#business-rule-in-jsx
  4. Never duplicate state that already exists in a parent or view model — see rules/rules.md#tsr-components-008

1.2/ Ask:
  - Ask when it is unclear whether a component has grown too large and should be split.

1.3/ Defaults:
  - A component that mixes data loading, transformation, event orchestration, and rendering must be split.
  - Business-meaning conditionals belong in view model fields, not in JSX ternaries.

1.4/ Examples:
  - ✅ see examples/good/example_good_component.tsx
  - ❌ see examples/bad/example_bad_component.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Focused React TSX component receiving typed props, rendering UI only.
2.1/ Outcome: Components are small, reusable, accessible, and free of business logic and infrastructure calls.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-components-005). No prose justification unless asked.
