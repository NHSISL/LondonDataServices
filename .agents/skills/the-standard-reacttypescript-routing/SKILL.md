---
name: the-standard-reacttypescript-routing
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/routes/**/*", "routes.tsx"]
depends-on: ["the-standard-reacttypescript-pages", "the-standard-reacttypescript-view-services"]
---

# The Standard React + TypeScript + Vite — Routing

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/routes/**/*` and `routes.tsx` — all route definitions and route guards.
0.1/ Who: Any engineer defining or reviewing routes and navigation guards in a Standard frontend project.
0.2/ What: Governs that routes point to pages, route definitions are centralized, route guards delegate to services, and business authorization rules are not embedded in JSX.
0.3/ Applies to: `src/routes/**/*`, `routes.tsx`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-pages, the-standard-reacttypescript-view-services

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Routes must point to pages, not low-level components — see rules/rules.md#tsr-routing-001
  2. Route definitions must be centralized unless the architecture requires modular registration — see rules/rules.md#tsr-routing-002
  3. Route guards must delegate access decisions to services — see rules/rules.md#tsr-routing-003

1.1/ Don'ts:
  1. Never embed business authorization rules directly in JSX route definitions — see rules/rules.md#tsr-routing-004 and validations/anti-patterns.md#auth-rule-in-jsx

1.2/ Ask:
  - Ask when it is unclear whether an access decision is a business rule (service) or a UI routing concern (guard).

1.3/ Defaults:
  - Route guards call a view service or authorization service to determine access.
  - Centralized route file is `src/app/routes.tsx`.

1.4/ Examples:
  - ✅ see examples/good/example_good_routing.tsx
  - ❌ see examples/bad/example_bad_routing.tsx

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Centralized route definitions pointing to page components, with guards delegating to services.
2.1/ Outcome: Routes are readable, centralized, and free of business logic.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-routing-004). No prose justification unless asked.
