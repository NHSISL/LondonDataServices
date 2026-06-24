# The Standard React + TypeScript + Vite — TypeScript — Anti-Patterns

## Any Usage

**Violates:** tsr-typescript-004
**What happens:** A function signature uses `any` for parameters or return type: `async function getPatient(id: any): Promise<any>`.
**Why it's wrong:** `any` disables type checking, making errors invisible until runtime.
**Fix:** Use the explicit domain type: `async function getPatientAsync(patientId: string): Promise<Patient>`. At truly unsafe external boundaries, use `unknown` and narrow.

## Implicit Return Types

**Violates:** tsr-typescript-002
**What happens:** A service method has no return type annotation: `async retrievePatientAsync(patientId: string) { ... }`.
**Why it's wrong:** Without an explicit return type at a boundary, the compiler cannot enforce the contract across layers.
**Fix:** Add the explicit return type: `: Promise<Patient>`.

## Interface for Data Shape

**Violates:** tsr-typescript-003
**What happens:** `interface Patient { id: string; name: string; }` is used for a plain data model.
**Why it's wrong:** `interface` implies a behavioral contract. Plain data shapes must use `type` to make intent clear and avoid accidental implementation.
**Fix:** Change to `type Patient = { id: string; name: string; };`.

## Type for Behavioral Contract

**Violates:** tsr-typescript-003
**What happens:** `type IPatientService = { retrievePatientAsync(id: string): Promise<Patient>; }` is used as a service contract.
**Why it's wrong:** Service and broker contracts are implemented by classes. `interface` is the correct keyword for implementable contracts.
**Fix:** Change to `export interface IPatientService { retrievePatientAsync(patientId: string): Promise<Patient>; }`.

## Barrel Files Hiding Layers

**Violates:** tsr-typescript-006
**What happens:** `import { PatientService } from "@services"` resolves through a barrel that mixes foundation services and view services.
**Why it's wrong:** The import hides which architectural layer is consumed, making layer violation detection harder.
**Fix:** Import directly from the file: `import { PatientService } from "@services/foundations/patients/patientService"`.
