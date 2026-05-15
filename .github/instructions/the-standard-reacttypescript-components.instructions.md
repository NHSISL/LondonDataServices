---
applyTo: "src/components/**/*"
---

# The Standard React + TypeScript + Vite — Components

## Applies To
All shared, layout, form, table, and card components in `src/components/**/*`.

## Rules — Do
- Components must render UI from props (tsr-components-001)
- Components must receive data through props unless they are a page-level container (tsr-components-002)
- Use prop models for non-trivial components (tsr-components-007)
- Interactive elements must be keyboard-accessible and use semantic HTML (tsr-components-009)

## Rules — Do Not
- Never call brokers from a component (tsr-components-003)
- Never call foundation services directly from a component (tsr-components-004)
- Never embed business rules in JSX (tsr-components-005)
- Never duplicate state that already exists in a parent or view model (tsr-components-008)

## Defaults
- A component that mixes data loading, transformation, event orchestration, and rendering must be split.
- Business-meaning conditionals belong in view model fields, not in JSX ternaries.
