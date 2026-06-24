---
applyTo: "**/*ProcessingService*.cs"
---

# The Standard — Processing Services

## Applies To
The higher-order business logic layer above foundation services — `*ProcessingService*.cs`, `*ProcessingServiceTests*.cs`.

## Rules — Do
- Combine two or more foundation service primitives to form a business workflow (ts-processings-001)
- Expose higher-order business verbs: Ensure, Process, Calculate, Upsert (ts-processings-002)
- Depend only on foundation services or other processing services; never depend on brokers directly (ts-processings-003)
- Handle cross-cutting concerns such as existence checks within the processing service (ts-processings-004)
- Write a failing test before every behavior (ts-processings-005)

## Rules — Do Not
- Must not call brokers directly (ts-processings-001)
- Must not duplicate validation already performed by the underlying foundation service (ts-processings-002)
- Must not expose raw CRUD primitives (Add, Retrieve, Modify, Remove) as the primary API (ts-processings-003)

## Defaults
- When a processing service checks existence before writing, it calls `Retrieve` then `Add` or `Modify` — not a custom broker query.
