# The Standard React + TypeScript + Vite — Models — Rules

## Foundation Models

**tsr-models-001** [ERROR] Foundation models must represent domain or API-facing concepts. They must mirror the external data contract without UI-specific fields.

## View Models

**tsr-models-002** [ERROR] View models must represent UI-ready data produced by view services. They must contain display-ready fields (e.g., `displayName`, `ageGroupDisplayText`) rather than raw domain values.

## Component Prop Models

**tsr-models-003** [ERROR] Component prop models must describe rendering input only. They must not contain API-facing raw fields or business logic.

## Model Reuse

**tsr-models-004** [ERROR] Raw API response models must not be passed directly as component props when the UI requires a different shape. Create a view model in the view service instead.

## No Behavior in Models

**tsr-models-005** [ERROR] Models must not contain methods, functions, API calls, storage mutations, or navigation logic. A model is a data-only type declaration.
