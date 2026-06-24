---
name: the-standard-reacttypescript-brokers
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/brokers/**/*"]
depends-on: ["the-standard-reacttypescript-files", "the-standard-reacttypescript-models"]
---

# The Standard React + TypeScript + Vite — Brokers

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `src/brokers/**/*` — API brokers, storage brokers, date/time brokers, logging brokers, navigation brokers.
0.1/ Who: Any engineer creating or reviewing broker classes in a Standard frontend project.
0.2/ What: Governs broker responsibility boundaries — one external concern per broker, no business rules, no component shaping, replaceable through interfaces.
0.3/ Applies to: `src/brokers/**/*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-files, the-standard-reacttypescript-models

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Each broker must wrap exactly one external concern — see rules/rules.md#tsr-brokers-001
  2. Brokers must be replaceable through interface contracts — see rules/rules.md#tsr-brokers-005
  3. Mechanical serialization and deserialization is permitted inside brokers when required by the external dependency — see rules/rules.md#tsr-brokers-004

1.1/ Don'ts:
  1. Never put business rules inside a broker — see rules/rules.md#tsr-brokers-002 and validations/anti-patterns.md#business-rule-in-broker
  2. Never shape data for components inside a broker — see rules/rules.md#tsr-brokers-003
  3. Never call services, pages, or components from a broker — see rules/rules.md#tsr-brokers-006

1.2/ Ask:
  - Ask when it is unclear whether logic is mechanical serialization (broker-allowed) or business rule (service-required).

1.3/ Defaults:
  - A broker method maps one-to-one with one external operation.
  - Broker methods return foundation model types, not view models.

1.4/ Examples:
  - ✅ see examples/good/example_good_broker.ts
  - ❌ see examples/bad/example_bad_broker.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: TypeScript broker class implementing its interface, wrapping one external concern.
2.1/ Outcome: Brokers are thin, replaceable, and free of business logic.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-brokers-002). No prose justification unless asked.
