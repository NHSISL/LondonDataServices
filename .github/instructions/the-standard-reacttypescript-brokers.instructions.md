---
applyTo: "src/brokers/**/*"
---

# The Standard React + TypeScript + Vite — Brokers

## Applies To
All broker files in `src/brokers/**/*` — API, storage, date/time, logging, and navigation brokers.

## Rules — Do
- Each broker must wrap exactly one external concern (tsr-brokers-001)
- Brokers must be replaceable through interface contracts (tsr-brokers-005)
- Mechanical serialization and deserialization is permitted inside brokers when required by the external dependency (tsr-brokers-004)

## Rules — Do Not
- Never put business rules inside a broker (tsr-brokers-002)
- Never shape data for components inside a broker (tsr-brokers-003)
- Never call services, pages, or components from a broker (tsr-brokers-006)

## Defaults
- A broker method maps one-to-one with one external operation.
- Broker methods return foundation model types, not view models.
