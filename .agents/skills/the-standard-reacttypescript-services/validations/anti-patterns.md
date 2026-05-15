# The Standard React + TypeScript + Vite — Foundation Services — Anti-Patterns

## React Import

**Violates:** tsr-foundation-002
**What happens:** A foundation service imports `useState` to track loading: `import { useState } from "react";`
**Why it's wrong:** Foundation services must be pure TypeScript — no React lifecycle dependencies. State belongs in hooks and pages.
**Fix:** Remove the import. Pass data in and out through method parameters and return values.

## JSX in Service

**Violates:** tsr-foundation-003
**What happens:** A service method returns a React element: `return <ErrorMessage text="Not found" />;`
**Why it's wrong:** Services are not rendering units. JSX in a service couples business logic to the rendering layer.
**Fix:** Throw a domain exception. Let the page or component decide how to render the error.

## No Validation Before Broker

**Violates:** tsr-foundation-004
**What happens:** A service calls the broker immediately without checking the input: `return await this.patientApiBroker.getPatientAsync(patientId);` where `patientId` could be empty.
**Why it's wrong:** Invalid inputs sent to brokers produce unhelpful infrastructure errors. Validation at the service boundary produces meaningful domain errors.
**Fix:** Call `validatePatientId(patientId)` before the broker call.

## Swallowed Exception

**Violates:** tsr-foundation-005
**What happens:** A broker error is silently swallowed: `try { ... } catch { return null; }`
**Why it's wrong:** Silent failures hide real problems and make debugging impossible.
**Fix:** Catch the error and re-throw a domain-specific exception: `throw createFailedPatientRetrievalException(error);`

## Navigation in Service

**Violates:** tsr-foundation-006
**What happens:** A service calls `navigate("/login")` after detecting an unauthorized response.
**Why it's wrong:** Navigation is an exposure concern. Services must not know about routes or UI flow.
**Fix:** Throw an `UnauthorizedException`. Let the page hook or route guard handle the navigation.
