# The Standard React + TypeScript + Vite — Error Handling — Anti-Patterns

## Raw Error in Component

**Violates:** tsr-errors-005
**What happens:** A component checks `if (error.status === 404) return <p>Not found</p>;`
**Why it's wrong:** HTTP status codes are infrastructure detail. Components must not know about the transport layer. Status 404 might also mean a misconfigured URL — only the service layer can give it domain meaning.
**Fix:** The foundation service converts a 404 into a `PatientNotFoundException`. The view service converts that into an error view model field. The component renders `<ErrorSummary error={props.error} />`.

## Silent Swallow

**Violates:** tsr-errors-006
**What happens:** `try { await broker.getPatientAsync(id); } catch { }` — the error disappears.
**Why it's wrong:** Silent failures hide real problems. Debugging becomes impossible. The user receives no feedback.
**Fix:** At minimum, log through `loggingBroker.logError(error)` then re-throw or set error state.

## Broker Converting Error to UI String

**Violates:** tsr-errors-001
**What happens:** A broker catches a failed fetch and returns `"Error loading patient"` as a string instead of throwing.
**Why it's wrong:** The caller (service) cannot distinguish a real error from a valid string result. The service layer loses its chance to localize the error.
**Fix:** Throw from the broker. Let the service localize, and the page display.
