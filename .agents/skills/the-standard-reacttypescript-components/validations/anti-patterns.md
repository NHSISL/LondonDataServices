# The Standard React + TypeScript + Vite — Components — Anti-Patterns

## Broker Call in Component

**Violates:** tsr-components-003
**What happens:** A component calls `fetch("/api/patients")` inside a `useEffect`.
**Why it's wrong:** Components are rendering units. Infrastructure calls bypass the service layers and make the component untestable and unreplaceable.
**Fix:** Receive the data as a prop. Move the fetch into a view service called by a page hook.

## Foundation Service Call in Component

**Violates:** tsr-components-004
**What happens:** A component imports `PatientService` and calls `patientService.retrievePatientAsync()` directly.
**Why it's wrong:** Components must not depend on foundation services. This bypasses the view service layer that shapes data for display.
**Fix:** Receive the data as a `PatientCardView` prop. Call the view service from the page hook.

## Business Rule in JSX

**Violates:** tsr-components-005
**What happens:** JSX contains `{patient.age >= 18 ? "Adult" : "Child"}`.
**Why it's wrong:** Business rules in JSX are invisible to services, untestable, and duplicated across every component that renders the same concept.
**Fix:** Add `ageGroupDisplayText` to the view model and render `{patient.ageGroupDisplayText}`.

## Div as Button

**Violates:** tsr-components-009
**What happens:** `<div onClick={onSave}>Save</div>` is used as an interactive control.
**Why it's wrong:** A `<div>` is not keyboard accessible by default, has no implicit ARIA role, and fails accessibility audits.
**Fix:** Use `<button type="button" onClick={onSave}>Save</button>`.
