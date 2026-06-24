# The Standard React + TypeScript + Vite — Routing — Anti-Patterns

## Route to Component

**Violates:** tsr-routing-001
**What happens:** `<Route path="/patients/:id" element={<PatientCard />} />` — a route points to a card component.
**Why it's wrong:** `PatientCard` is a rendering unit, not a route. Without a page, there is no place to handle loading, error, and empty states for the route.
**Fix:** Create `PatientProfilePage.tsx` that composes `PatientCard`. Point the route to `<PatientProfilePage />`.

## Auth Rule in JSX

**Violates:** tsr-routing-004
**What happens:** `{user.role === "Admin" && <Route path="/admin" element={<AdminPage />} />}` — role check in JSX.
**Why it's wrong:** Role-based access is a business rule. It must be testable, auditable, and centralized — not scattered across JSX.
**Fix:** Create a `ProtectedRoute` component that calls `authorizationViewService.canAccessAdminAsync()` and redirects if access is denied.
