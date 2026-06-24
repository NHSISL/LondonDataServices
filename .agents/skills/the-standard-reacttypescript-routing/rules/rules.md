# The Standard React + TypeScript + Vite — Routing — Rules

## Routes to Pages

**tsr-routing-001** [ERROR] Routes must point to page components (`{Domain}Page.tsx`), not to low-level components. A route that renders a card, list, or form component directly bypasses the page layer.

## Centralized Definitions

**tsr-routing-002** [ERROR] Route definitions must be centralized in a single routes file (`src/app/routes.tsx` or equivalent) unless the application architecture explicitly requires modular route registration.

## Service-Delegating Guards

**tsr-routing-003** [ERROR] Route guards must delegate access decisions to a service (authorization view service or authentication service). Guards must not contain inline role-checking logic.

## No Auth Rules in JSX

**tsr-routing-004** [ERROR] Business authorization rules must not be embedded directly in JSX route definitions. `{user.role === "Admin" && <Route ... />}` is a violation.
