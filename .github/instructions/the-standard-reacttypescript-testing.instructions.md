---
applyTo: "**/*.test.ts,**/*.test.tsx,**/*.spec.ts,**/*.spec.tsx"
---

# The Standard React + TypeScript + Vite — Testing

## Applies To
All test files: `*.test.ts`, `*.test.tsx`, `*.spec.ts`, `*.spec.tsx`.

## Rules — Do
- Test file must mirror the directory structure of the file under test (tsr-testing-002)
- Use `userEvent` to simulate user interactions (tsr-testing-003)
- Assert on rendered DOM output and user-visible behaviour (tsr-testing-004)
- Mock brokers and services at the component boundary (tsr-testing-005)
- Follow the FAIL/PASS TDD cycle (tsr-testing-006)
- Write one focused test per observable behaviour (tsr-testing-007)
- Name tests using `Should{Outcome}` or `Should{Outcome}When{Condition}` (tsr-testing-008)

## Rules — Do Not
- Never test implementation details (tsr-testing-004)
- Never leave a test perpetually in FAIL state (tsr-testing-006)

## Defaults
- Vitest + React Testing Library is the default test stack.
- Test file for `src/components/StudentCard.tsx` lives at `src/components/StudentCard.test.tsx`.
