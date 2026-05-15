# The Standard React + TypeScript + Vite — State Management — Rules

## Locality

**tsr-state-001** [ERROR] Keep state as local as possible. Elevate state only when two or more sibling components genuinely need to share it.

**tsr-state-002** [ERROR] Do not duplicate server data into multiple independent component states. A single page hook owns the server data; child components receive it through props.

## URL State

**tsr-state-003** [WARN] Use URL state (query parameters, path segments) for route-significant state that should be shareable or bookmarkable (e.g., selected tab, active filters, pagination page).

## Component State

**tsr-state-004** [ERROR] Use component state for local UI state that has no meaning outside the component: modal open/closed, pre-submit input text, dropdown expanded/collapsed.

## Application State

**tsr-state-005** [ERROR] Use application-level state (context, global store) only for genuinely cross-cutting concerns: authenticated user, global theme, feature flags, tenant context.

**tsr-state-006** [ERROR] Global state must not be used to avoid designing props. Prop avoidance is not a valid reason to elevate state.

## Business Transitions

**tsr-state-007** [ERROR] State transitions that have business meaning (submitting a form, publishing a record, approving a request) must be handled through services, not directly in state setters.
