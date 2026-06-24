---
applyTo: "**/*.ts,**/*.tsx"
---

# The Standard React + TypeScript + Vite — TypeScript

## Applies To
All TypeScript source files: `*.ts` and `*.tsx`.

## Rules — Do
- Enable strict mode in `tsconfig.json` (tsr-typescript-001)
- Use explicit return types on all exported functions (tsr-typescript-002)
- Prefer `interface` for object shapes that are open for extension; prefer `type` for unions, intersections, and computed shapes (tsr-typescript-003)
- Use TypeScript generics over `any` (tsr-typescript-004)
- Use `unknown` and narrow before use rather than casting with `as` (tsr-typescript-005)

## Rules — Do Not
- Never use `any` as a type (tsr-typescript-006)
- Never use `as` to cast without narrowing first (tsr-typescript-005)
- Never disable `strict` in tsconfig (tsr-typescript-001)

## Defaults
- `strict: true` in `tsconfig.json`.
- No `any`, no `@ts-ignore` without a comment explaining the exception.
