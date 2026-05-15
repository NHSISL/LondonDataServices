---
applyTo: "**/*Broker*.cs"
---

# The Standard — Brokers

## Applies To
The infrastructure layer — all `*Broker*.cs` files.

## Rules — Do
- Define brokers as thin wrappers over external resources with no business logic (ts-brokers-001)
- Name broker interfaces generically: `IStorageBroker`, not `ISqlStorageBroker` (ts-brokers-002)
- Use storage language inside brokers: `InsertAsync`, `SelectAsync`, `UpdateAsync`, `DeleteAsync` (ts-brokers-003)
- Use RESTful language for API brokers: `PostAsync`, `GetAsync`, `PutAsync`, `DeleteAsync` (ts-brokers-004)
- Use queue language for queue brokers: `EnqueueAsync`, `DequeueAsync` (ts-brokers-005)
- Accept only primitive types or native models as input; never accept upstream service models (ts-brokers-006)
- Return only the raw external resource response without transformation (ts-brokers-007)
- Retrieve configuration values via `IConfiguration` injected in the constructor (ts-brokers-008)
- Brokers must be partial classes when supporting multiple entities (ts-brokers-009)
- Version broker contracts using the SPAL versioning strategy (ts-brokers-010)

## Rules — Do Not
- Must not contain validation logic (ts-brokers-001)
- Must not call other brokers (ts-brokers-002)
- Must not expose internal infrastructure types such as `DbContext` or `HttpClient` (ts-brokers-003)
- Must not use technology-specific names in the interface, e.g., `ISqlBroker`, `IMongoStorageBroker` (ts-brokers-004)

## Defaults
- When a single entity requires storage operations, use `IStorageBroker` as the interface name.
- When multiple storage technologies exist, append the technology name only to the concrete class, not the interface.
