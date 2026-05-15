---
name: the-standard-reacttypescript-view-services
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/services/views/**/*"]
depends-on: ["the-standard-reacttypescript-services", "the-standard-reacttypescript-models"]
---

# The Standard React + TypeScript + Vite — View Services

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/services/views/**/*` — view service classes, validation partials, exception partials, and interfaces.
0.1/ Who: Any engineer creating or reviewing view services in a Standard frontend project.
0.2/ What: Governs view service coordination of foundation services, view model production, and prohibition of JSX, React imports, direct broker calls, and rendering concerns.
0.3/ Applies to: `src/services/views/**/*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-services, the-standard-reacttypescript-models

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. View services must coordinate foundation services to produce view models — see rules/rules.md#tsr-viewservices-001
  2. View services may aggregate, sort, filter, and map data into view models — see rules/rules.md#tsr-viewservices-005

1.1/ Don'ts:
  1. Never return JSX from a view service — see rules/rules.md#tsr-viewservices-002
  2. Never import React in a view service — see rules/rules.md#tsr-viewservices-003
  3. Never call brokers directly unless no foundation service exists for that dependency — see rules/rules.md#tsr-viewservices-004
  4. Never include rendering concerns (CSS class names, component names, Bootstrap classes) in a view service — see rules/rules.md#tsr-viewservices-006

1.2/ Ask:
  - Ask when it is unclear whether a data transformation belongs in a foundation service or a view service.

1.3/ Defaults:
  - Business-rule computations belong in foundation services.
  - Display-oriented transformations (formatting, mapping to display text) belong in view services.

1.4/ Examples:
  - ✅ see examples/good/example_good_view_service.ts
  - ❌ see examples/bad/example_bad_view_service.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TypeScript view service class implementing its interface, producing view models.
2.1/ Outcome: View services produce display-ready view models without containing React, JSX, or rendering concerns.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-viewservices-004). No prose justification unless asked.
