---
name: the-standard-reacttypescript-styling
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.tsx", "*.css", "*.scss"]
depends-on: ["the-standard-reacttypescript-components", "the-standard-reacttypescript-view-services"]
---

# The Standard React + TypeScript + Vite — Styling

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All TSX component files and CSS/SCSS style files.
0.1/ Who: Any engineer writing or reviewing styles and class name assignment in a Standard frontend project.
0.2/ What: Governs Bootstrap-first utility class usage, prohibition of business rules in CSS class selection, component-scoped styles, global style discipline, and inline style restrictions.
0.3/ Applies to: `*.tsx`, `*.css`, `*.scss`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-components, the-standard-reacttypescript-view-services

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Use Bootstrap utility classes for layout and common spacing when Bootstrap is the selected UI foundation — see rules/rules.md#tsr-styles-001
  2. Component-specific styles must stay near the component or in a clearly named style module — see rules/rules.md#tsr-styles-003
  3. Global styles must be reserved for global concerns only — see rules/rules.md#tsr-styles-004

1.1/ Don'ts:
  1. Never encode business rules in CSS class selection — see rules/rules.md#tsr-styles-002 and validations/anti-patterns.md#business-rule-in-class
  2. Never use inline styles except for dynamic values that cannot reasonably be represented with classes — see rules/rules.md#tsr-styles-005

1.2/ Ask:
  - Ask when it is unclear whether a class selection has business meaning or is purely presentational.

1.3/ Defaults:
  - When a class name encodes domain status (e.g., `text-success` for "active"), expose a semantic display field from the view service instead.
  - When a class name is purely presentational and not domain-driven, it may be set in the component.

1.4/ Examples:
  - ✅ see examples/good/example_good_styling.tsx
  - ❌ see examples/bad/example_bad_styling.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TSX with class names derived from view model fields or static presentation logic only.
2.1/ Outcome: Components apply styles without encoding business rules. Domain status drives display text in view models; class names are purely presentational.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-styles-002). No prose justification unless asked.
