# The Standard Brokers — Anti-Patterns

## Broker with Validation

**Violates:** ts-brokers-001
**What happens:** The broker checks whether a student is null or whether the Id is empty before calling the storage layer.
**Why it's wrong:** Validation is a service-layer responsibility. Putting it in a broker mixes concerns and makes the broker untestable as a pure infrastructure shim.
**Fix:** Remove all validation from the broker. Add validation in the foundation service.

## Broker Calling Broker

**Violates:** ts-brokers-001
**What happens:** `StudentStorageBroker.InsertStudentAsync` calls `LoggingBroker.LogInformationAsync` internally.
**Why it's wrong:** Brokers are the first point of abstraction and must not depend on other brokers. Cross-cutting concerns like logging belong in services.
**Fix:** Inject `ILoggingBroker` into the service, not the broker.

## Leaking Infrastructure Types

**Violates:** ts-brokers-007
**What happens:** The broker method returns a `DbSet<Student>` or `IQueryable<Student>` instead of `IQueryable<Student>` materialized into a collection.
**Why it's wrong:** Leaking EF Core types into the service layer couples services to the storage technology.
**Fix:** Materialize the query inside the broker and return `IQueryable<Student>` only when explicitly required by The Standard's pattern for foundation services.

## Technology-Specific Naming

**Violates:** ts-brokers-002
**What happens:** The interface is named `ISqlServerStorageBroker` or `IMongoStorageBroker`.
**Why it's wrong:** The interface name reveals the underlying technology, coupling all consumers to that technology choice.
**Fix:** Name the interface `IStorageBroker`. Apply the technology name only to the concrete class (e.g., `SqlStorageBroker`, `MongoStorageBroker`).
