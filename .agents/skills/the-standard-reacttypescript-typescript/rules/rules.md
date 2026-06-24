# The Standard React + TypeScript + Vite — TypeScript — Rules

## Strict Configuration

**tsr-typescript-001** [ERROR] TypeScript must be configured with strict settings. The following compiler options must be enabled unless an explicit project-level exception is documented:

```json
{
  "compilerOptions": {
    "strict": true,
    "noImplicitAny": true,
    "strictNullChecks": true,
    "noUncheckedIndexedAccess": true,
    "exactOptionalPropertyTypes": true
  }
}
```

## Explicit Boundary Types

**tsr-typescript-002** [ERROR] Explicit domain types must be used at all architectural boundaries (broker signatures, service signatures, hook return types, component props). Implicit `any` or missing return types at boundaries are forbidden.

## Type vs Interface

**tsr-typescript-003** [ERROR] Use `type` for data shapes (models, view models, props). Use `interface` for behavioral contracts (service interfaces, broker interfaces).

## No Any

**tsr-typescript-004** [ERROR] `any` must not be used. Use `unknown` at unsafe external boundaries and narrow to a known type through validation or explicit casting with documentation.

## Export Style

**tsr-typescript-005** [ERROR] Services, brokers, models, hooks, and utilities must use named exports. Pages and components may use default exports only when routing or framework conventions require it.

## Barrel Files

**tsr-typescript-006** [WARN] Barrel `index.ts` files must not obscure dependency direction. An import through a barrel file must not make it unclear which architectural layer is being consumed.
