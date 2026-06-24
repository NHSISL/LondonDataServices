# The Standard React + TypeScript + Vite — Brokers — Rules

## Single External Concern

**tsr-brokers-001** [ERROR] A broker must wrap exactly one external concern. Permitted broker types and their external concerns:

| Broker | External Concern |
|---|---|
| ApiBroker | HTTP / fetch / axios |
| StorageBroker | localStorage / sessionStorage / indexedDB |
| DateTimeBroker | current date and time |
| LoggingBroker | console / telemetry |
| NavigationBroker | browser navigation / router adapter |

## No Business Rules

**tsr-brokers-002** [ERROR] A broker must not contain business rules. Conditional logic based on domain meaning (e.g., age thresholds, status classification) must be placed in a foundation service.

## No Component Shaping

**tsr-brokers-003** [ERROR] A broker must not transform data into a shape designed for a specific component or view. Return foundation model types only.

## Mechanical Serialization

**tsr-brokers-004** [WARN] A broker may perform mechanical serialization and deserialization (JSON parse/stringify, type assertions on raw responses) when required by the external dependency. This is not a business rule.

## Replaceable Interface

**tsr-brokers-005** [ERROR] Every broker must have a corresponding interface that describes its contract. Services must depend on the interface, not the concrete class.

## No Upstream Calls

**tsr-brokers-006** [ERROR] A broker must not call services, pages, or components. Dependency direction is: Service → Broker, never the reverse.
