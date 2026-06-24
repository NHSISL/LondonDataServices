# The Standard React + TypeScript + Vite — Brokers — Anti-Patterns

## Business Rule in Broker

**Violates:** tsr-brokers-002
**What happens:** A broker method applies domain logic: `return patient.age >= 18 ? "Adult" : "Child";`
**Why it's wrong:** Business rules belong in foundation services. A broker that applies domain logic becomes untestable as a pure dependency wrapper.
**Fix:** Return the raw `Patient` model. Let the foundation service or view service compute the age group.

## Component Shaping in Broker

**Violates:** tsr-brokers-003
**What happens:** A broker returns `{ displayName: patient.firstName + " " + patient.lastName }` — a field designed for a component.
**Why it's wrong:** Display shaping is the view service's responsibility. A broker that shapes for components creates a hidden coupling between infrastructure and UI.
**Fix:** Return the raw `Patient` model. Let the view service produce `PatientProfileView` with `displayName`.

## No Interface

**Violates:** tsr-brokers-005
**What happens:** A service directly imports and instantiates `PatientApiBroker` without an interface.
**Why it's wrong:** Without an interface, the broker cannot be replaced in tests. The service is tightly coupled to the HTTP layer.
**Fix:** Create `IPatientApiBroker` with the method signatures. Inject it into the service constructor.

## Broker Calling Service

**Violates:** tsr-brokers-006
**What happens:** A `LoggingBroker` calls `patientService.retrievePatientAsync()` to enrich a log entry.
**Why it's wrong:** Dependency direction must flow Service → Broker. Reversing it creates circular dependencies and layer violations.
**Fix:** Pass the enrichment data as a parameter to the logging broker method.
