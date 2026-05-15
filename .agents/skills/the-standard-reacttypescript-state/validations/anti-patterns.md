# The Standard React + TypeScript + Vite — State Management — Anti-Patterns

## Duplicated Server State

**Violates:** tsr-state-002
**What happens:** A page fetches patients and stores them in hook state. A child component also fetches the same patients and stores them in its own `useState`.
**Why it's wrong:** Two independent states diverge. One may update while the other lags, producing inconsistent UI.
**Fix:** Fetch once in the page hook. Pass the data as props to child components.

## Global State for Prop Avoidance

**Violates:** tsr-state-006
**What happens:** A developer adds `PatientContext` so they can access the selected patient anywhere without threading props through three layers.
**Why it's wrong:** This is prop avoidance, not a genuine cross-cutting concern. Global state grows uncontrolled and makes data flow invisible.
**Fix:** Thread the prop or restructure the component tree. Use context only for authenticated user, theme, feature flags, or tenant.

## Business Transition in State Setter

**Violates:** tsr-state-007
**What happens:** A save button handler calls `setPatient({ ...patient, status: "Published" })` directly, bypassing any service.
**Why it's wrong:** Publishing has business meaning — validation, exception handling, and audit logging belong in a foundation service.
**Fix:** Call `await patientViewService.publishPatientAsync(patient)` in the handler, then update state from the result.
