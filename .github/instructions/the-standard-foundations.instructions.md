---
applyTo: "**/*Service*.cs,**/*Tests*.cs"
---

# The Standard — Foundation Services

## Applies To
The business logic layer directly above brokers — `*Service*.cs`, `*Tests*.cs`.

## Rules — Do
- Translate broker storage language to business language: Insert→Add, Select→Retrieve, Update→Modify, Delete→Remove (ts-foundations-001)
- Expose only CRUD operations the entity requires — no business-workflow methods (ts-foundations-002)
- Perform structural validation (null checks, empty Guid, empty strings) on all inputs (ts-foundations-003)
- Perform logical validation (date ranges, referential integrity) where applicable (ts-foundations-004)
- Integrate with exactly one entity broker only (ts-foundations-005)
- Wrap all broker and unexpected exceptions in service-specific exception models (ts-foundations-006)
- Write a failing test before implementing every behavior (ts-foundations-007)
- Randomize all test inputs and outputs; use `DeepClone()` for expected variables (ts-foundations-008)

## Rules — Do Not
- Must not call more than one entity broker (ts-foundations-001)
- Must not implement business-workflow methods such as Upsert — that belongs in Processing services (ts-foundations-002)
- Must not expose raw broker exceptions to consumers (ts-foundations-003)
- Must not use specific values in tests where randomization is possible (ts-foundations-004)

## Defaults
- Default to structural validation first, then logical validation.
- Broker exceptions → dependency exceptions; unexpected exceptions → service exceptions.
