# The Standard React + TypeScript + Vite — Files — Anti-Patterns

## Generic Files

**Violates:** tsr-files-005
**What happens:** A file named `utils.ts`, `helpers.ts`, or `common.ts` accumulates miscellaneous functions from multiple concerns.
**Why it's wrong:** Generic files hide architectural responsibility, making it impossible to tell which layer owns which code.
**Fix:** Move each function into a named broker, service, hook, model, or component that describes its architectural role.

## Mixed Responsibility

**Violates:** tsr-files-001
**What happens:** A single file `patientEverything.ts` exports a broker, a service, and a model.
**Why it's wrong:** Mixed responsibilities make the file impossible to replace, test, or evolve independently.
**Fix:** Split into `patientApiBroker.ts`, `patientService.ts`, and `Patient.ts`.

## Wrong Extension

**Violates:** tsr-files-003 / tsr-files-004
**What happens:** A service file is named `patientService.tsx`, or a component file is named `PatientCard.ts`.
**Why it's wrong:** The extension signals whether a file contains JSX. Mismatched extensions mislead tooling and reviewers.
**Fix:** Rename rendering files to `.tsx` and non-rendering files to `.ts`.

## Missing Role in Name

**Violates:** tsr-files-002
**What happens:** A broker is named `patient.ts` instead of `patientApiBroker.ts`, or a page is named `Dashboard.tsx` instead of `DashboardPage.tsx`.
**Why it's wrong:** Without the role suffix, the file's architectural function is ambiguous.
**Fix:** Rename to include the architectural role: `patientApiBroker.ts`, `DashboardPage.tsx`.
