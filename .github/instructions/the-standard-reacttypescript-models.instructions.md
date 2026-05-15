---
applyTo: "src/models/**/*"
---

# The Standard React + TypeScript + Vite — Models

## Applies To
All model definitions in `src/models/**/*` — foundation models, view models, prop models, error models.

## Rules — Do
- Foundation models must represent domain or API-facing concepts (tsr-models-001)
- View models must represent UI-ready data produced by view services (tsr-models-002)
- Component prop models must describe rendering input only (tsr-models-003)
- Create a dedicated view model when the UI needs a different shape than the API response (tsr-models-004)

## Rules — Do Not
- Never reuse raw API response models directly as component props when shapes differ (tsr-models-001)
- Never put behavior into models (tsr-models-005)

## Defaults
- If a field requires formatting, calculation, or display logic, it belongs on a view model produced by the view service.
- Foundation models mirror the API/domain contract.
- Prop models are a subset or mapping of the view model relevant to one component.
