# The Standard Core — Rules

## PURPOSE & SINGLE RESPONSIBILITY

**ts-core-001** [ERROR] Every component must serve a single, clearly defined purpose; split any component that serves more than one.
**ts-core-002** [ERROR] Model every real-world concept using the tri-nature structure: Purpose (0/), Dependency (1/), Exposure (2/).
**ts-core-003** [ERROR] Simulate (write a failing test for) every behavior before implementing it.

## NAMING

**ts-core-004** [ERROR] Name every type, method, parameter, and variable using full, non-abbreviated plain English words.
**ts-core-005** [ERROR] Use domain language for business concepts; use technical language only for infrastructure concerns.

## LAYERING PRINCIPLES

**ts-core-006** [ERROR] Apply the Purity principle: broker implementations must contain zero business logic; service implementations must contain zero infrastructure code.
**ts-core-007** [ERROR] Apply the Realism principle: every model must represent a real-world concept — no technical-only models (e.g., no `ResultWrapper<T>` as a domain model).
**ts-core-008** [ERROR] Apply the Fitness principle: a component must not perform responsibilities assigned to a different layer.
**ts-core-009** [ERROR] Apply the Enclosure principle: components must expose only the minimum contract necessary to fulfil their purpose.
**ts-core-010** [WARN]  Apply the Continuance principle: every component must be replaceable without changing its consumers.
