---
applyTo: "**/*.ts,**/*.tsx"
---

# The Standard React + TypeScript + Vite — Async and Cancellation

## Applies To
All TypeScript and TSX files performing asynchronous operations — `*.ts`, `*.tsx`.

## Rules — Do
- Async page loading must handle loading, success, empty, and failure states (tsr-async-001)
- Effects with async work must protect against stale updates (tsr-async-002)
- Use `Promise.all` only when operations are truly independent (tsr-async-003)

## Rules — Do Not
- Never run dependent async operations in parallel (tsr-async-004)
- Never ignore rejected promises (tsr-async-005)

## Defaults
- Use mounted flags as the default stale update protection pattern.
- `AbortController` is acceptable for fetch-based brokers.
- Framework-approved loaders (e.g., React Router `loader`) are acceptable when the framework manages cancellation.
