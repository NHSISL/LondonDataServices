---
name: the-standard-reacttypescript-models
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/models/**/*"]
depends-on: ["the-standard-reacttypescript-typescript"]
---

# The Standard React + TypeScript + Vite — Models

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/models/**/*` — foundation models, view models, component prop models, error models.
0.1/ Who: Any engineer defining or reviewing data contracts between architectural layers.
0.2/ What: Governs what belongs in each model category, prevents model reuse across inappropriate layers, and prohibits behavior in models.
0.3/ Applies to: `src/models/**/*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-typescript

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Foundation models must represent domain or API-facing concepts — see rules/rules.md#tsr-models-001
  2. View models must represent UI-ready data produced by view services — see rules/rules.md#tsr-models-002
  3. Component prop models must describe rendering input only — see rules/rules.md#tsr-models-003
  4. Create a dedicated view model when the UI needs a different shape than the API response — see rules/rules.md#tsr-models-004

1.1/ Don'ts:
  1. Never reuse raw API response models directly as component props when shapes differ — see validations/anti-patterns.md#api-model-as-props
  2. Never put behavior into models — see rules/rules.md#tsr-models-005 and validations/anti-patterns.md#behavior-in-model

1.2/ Ask:
  - Ask when it is unclear whether a field belongs on a foundation model or a view model.

1.3/ Defaults:
  - If a field requires formatting, calculation, or display logic, it belongs on a view model produced by the view service.
  - Foundation models mirror the API/domain contract.
  - Prop models are a subset or mapping of the view model relevant to one component.

1.4/ Examples:
  - ✅ see examples/good/example_good_models.ts
  - ❌ see examples/bad/example_bad_models.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TypeScript `type` declarations organized by model category.
2.1/ Outcome: Each layer exchanges data through explicit, layer-appropriate models with no behavior.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-models-001). No prose justification unless asked.
