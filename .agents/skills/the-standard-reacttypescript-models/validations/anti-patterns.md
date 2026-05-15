# The Standard React + TypeScript + Vite — Models — Anti-Patterns

## API Model as Props

**Violates:** tsr-models-004
**What happens:** A component receives the raw API `Patient` model as a prop: `<PatientCard patient={apiPatient} />` where `Patient` has raw fields like `dateOfBirth: string` that require formatting for display.
**Why it's wrong:** The component is forced to contain formatting logic, which is a view service responsibility.
**Fix:** Create a `PatientCardView` type with `displayName` and `ageGroupDisplayText` fields. Let the view service produce it.

## Behavior in Model

**Violates:** tsr-models-005
**What happens:** A `Patient` type includes a method: `type Patient = { id: string; getDisplayName(): string; }`.
**Why it's wrong:** Models are data contracts. Adding behavior couples the model to logic, making it untestable and causing layer confusion.
**Fix:** Remove the method. Place the logic in the view service and expose the computed value as a view model field.

## Foundation Model Used as View Model

**Violates:** tsr-models-002
**What happens:** The `Patient` foundation model is returned directly by a view service as the view model.
**Why it's wrong:** Foundation models reflect the API shape. View services must transform them into display-ready view models.
**Fix:** Create a `PatientProfileView` type with display-ready fields and return it from the view service.
