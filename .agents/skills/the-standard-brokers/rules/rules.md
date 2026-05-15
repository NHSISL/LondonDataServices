# The Standard Brokers — Rules

## DESIGN

**ts-brokers-001** [ERROR] Brokers must be thin wrappers over external resources and must contain zero business logic.
**ts-brokers-002** [ERROR] Broker interface names must be technology-agnostic (e.g., `IStorageBroker`, not `ISqlStorageBroker`).
**ts-brokers-009** [ERROR] Brokers supporting multiple entities must be implemented as partial classes.

## LANGUAGE

**ts-brokers-003** [ERROR] Storage broker methods must use storage language: `InsertAsync`, `SelectAsync`, `SelectAllAsync`, `UpdateAsync`, `DeleteAsync`.
**ts-brokers-004** [ERROR] API broker methods must use RESTful language: `PostAsync`, `GetAsync`, `PutAsync`, `DeleteAsync`.
**ts-brokers-005** [ERROR] Queue broker methods must use queue language: `EnqueueAsync`, `DequeueAsync`.

## CONTRACTS

**ts-brokers-006** [ERROR] Broker input parameters must be primitive types or native models; must not accept upstream service-layer models.
**ts-brokers-007** [ERROR] Brokers must return the raw external resource response without transformation.
**ts-brokers-008** [ERROR] Configuration values (connection strings, API keys) must be retrieved via `IConfiguration` injected through the constructor.

## VERSIONING (SPAL)

**ts-brokers-010** [WARN]  Broker contracts must be versioned using the SPAL strategy when breaking changes are introduced.
