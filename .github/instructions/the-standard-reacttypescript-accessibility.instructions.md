---
applyTo: "**/*.tsx"
---

# The Standard React + TypeScript + Vite — Accessibility

## Applies To
All React TSX component and page files — `*.tsx`.

## Rules — Do
- Interactive UI must use semantic HTML elements (tsr-accessibility-001)
- All form inputs must have accessible labels (tsr-accessibility-002)
- Images must have meaningful `alt` text or be explicitly decorative with `alt=""` (tsr-accessibility-003)
- Keyboard navigation must be preserved for all interactive elements (tsr-accessibility-004)
- Error messages must be visible and associated with relevant controls (tsr-accessibility-005)

## Rules — Do Not
- Never use non-semantic elements (`<div>`, `<span>`) as interactive controls without ARIA (tsr-accessibility-001)
- Never use `<img>` without an `alt` attribute (tsr-accessibility-002)

## Defaults
- Use `<button type="button">` for actions, `<a href="...">` for navigation.
- Use `<label htmlFor="...">` or `aria-label` for form inputs without visible label text.
- Decorative images must have `alt=""` to signal they are presentational.
