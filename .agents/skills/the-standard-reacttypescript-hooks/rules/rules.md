# The Standard React + TypeScript + Vite — Hooks — Rules

## Lifecycle and State

**tsr-hooks-001** [ERROR] A hook may manage React lifecycle and state. This is its primary permitted responsibility.

## Not a Service Replacement

**tsr-hooks-002** [ERROR] A hook must not replace a foundation service. A hook that fetches data, filters it by business rules, and returns it is acting as a service — that logic must move to a foundation service and view service.

## No Business Rules

**tsr-hooks-003** [ERROR] A hook must not contain core business rules. Domain logic (age thresholds, status classification, eligibility checks) belongs in foundation services.

## View Service Access

**tsr-hooks-004** [ERROR] A page hook may call a view service to retrieve view models. The view service must be injected as a parameter or resolved through an approved DI mechanism — not instantiated inside the hook.

## Effect Stale Update Protection

**tsr-hooks-005** [ERROR] Effects that perform async work must protect against stale updates using a mounted flag (`let isMounted = true`) or `AbortController`. The cleanup function must set `isMounted = false` or abort the controller.

## No useEffect for Transformation

**tsr-hooks-006** [WARN] Do not use `useEffect` for data transformation that can be calculated during render or that belongs in a view service. Effects are for side effects, not derivations.

## No Suppressed Warnings

**tsr-hooks-007** [ERROR] Do not suppress ESLint hook dependency warnings (`// eslint-disable-next-line react-hooks/exhaustive-deps`) without a documented reason in a comment explaining why the dependency is intentionally omitted.
