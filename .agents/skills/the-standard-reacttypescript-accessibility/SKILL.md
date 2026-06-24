---
name: the-standard-reacttypescript-accessibility
version: 0.1.0
standard-version: v0.1.0
applies-to: ["*.tsx"]
depends-on: ["the-standard-reacttypescript-components", "the-standard-reacttypescript-pages"]
---

# The Standard React + TypeScript + Vite — Accessibility

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All React TSX component and page files.
0.1/ Who: Any engineer creating or reviewing UI components and pages in a Standard frontend project.
0.2/ What: Governs semantic HTML usage, form labeling, image alt text, keyboard navigation, and error message accessibility.
0.3/ Applies to: `*.tsx`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-components, the-standard-reacttypescript-pages

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Interactive UI must use semantic HTML elements — see rules/rules.md#tsr-accessibility-001
  2. All form inputs must have accessible labels — see rules/rules.md#tsr-accessibility-002
  3. Images must have meaningful `alt` text or be explicitly decorative (`alt=""`) — see rules/rules.md#tsr-accessibility-003
  4. Keyboard navigation must be preserved for all interactive elements — see rules/rules.md#tsr-accessibility-004
  5. Error messages must be visible and associated with relevant controls — see rules/rules.md#tsr-accessibility-005

1.1/ Don'ts:
  1. Never use non-semantic elements (`<div>`, `<span>`) as interactive controls without ARIA — see validations/anti-patterns.md#div-as-button
  2. Never use `<img>` without an `alt` attribute — see validations/anti-patterns.md#missing-alt

1.2/ Ask:
  - Ask when it is unclear whether a decorative image needs `alt=""` or a descriptive `alt` text.

1.3/ Defaults:
  - Use `<button type="button">` for actions, `<a href="...">` for navigation.
  - Use `<label htmlFor="...">` or `aria-label` for form inputs without visible label text.
  - Decorative images must have `alt=""` to signal they are presentational.

1.4/ Examples:
  - ✅ see examples/good/example_good_accessibility.tsx
  - ❌ see examples/bad/example_bad_accessibility.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TSX components using semantic HTML, accessible form controls, and keyboard-navigable interactions.
2.1/ Outcome: All interactive elements are reachable by keyboard, all form inputs are labeled, and all images have appropriate alt text.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-accessibility-001). No prose justification unless asked.
