# The Standard React + TypeScript + Vite — View Services — Anti-Patterns

## Direct Broker Call

**Violates:** tsr-viewservices-004
**What happens:** A view service imports `PatientApiBroker` and calls `broker.getPatientAsync()` directly.
**Why it's wrong:** View services sit above foundation services in the dependency chain. Calling brokers directly bypasses the validation and exception-localization responsibilities of foundation services.
**Fix:** Inject `IPatientService` into the view service and call `patientService.retrievePatientAsync()`.

## Rendering Concern in View Service

**Violates:** tsr-viewservices-006
**What happens:** A view model field is set to a Bootstrap class string: `statusClass: patient.isActive ? "text-success" : "text-danger"`.
**Why it's wrong:** CSS class names are rendering details. The view service would be coupled to the visual framework, making it impossible to change the framework without touching business logic.
**Fix:** Use a semantic display value: `statusDisplayText: patient.isActive ? "Active" : "Inactive"`. Let the component choose the class.

## React in View Service

**Violates:** tsr-viewservices-003
**What happens:** A view service imports `useCallback` to memoize its data fetch.
**Why it's wrong:** View services are pure TypeScript classes — they have no lifecycle. Memoization belongs in the hook that calls the view service.
**Fix:** Remove the React import. The hook (`useDashboardPage`) manages caching and re-rendering.
