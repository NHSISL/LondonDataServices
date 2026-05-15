---
applyTo: "**/*.tsx,**/*.css,**/*.scss"
---

# The Standard React + TypeScript + Vite — Styling

## Applies To
All component files (`*.tsx`) and stylesheets (`*.css`, `*.scss`) in any Standard React project.

## Rules — Do
- Co-locate component stylesheet alongside its component file (tsr-styles-001)
- Use scoped CSS Modules or a well-defined class naming convention to avoid global collisions (tsr-styles-002)
- Keep layout responsibilities at the page or container component; keep component styles self-contained (tsr-styles-003)

## Rules — Do Not
- Never apply layout margin/padding that changes the component's external spacing from inside the component (tsr-styles-004)
- Never mix business logic and styling decisions in the same expression (tsr-styles-005)

## Defaults
- CSS Modules (`*.module.css`) are the default.
- Layout wrapper classes live in the page/container, not inside the leaf component.
