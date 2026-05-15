---
skill: the-standard-team-core
type: rules
source-section: "0 Introduction, 1 Purposing"
---

# The Standard Team — Core — Rules

Severity levels: `[ERROR]` = must fix · `[WARN]` = should fix.

---

## Purpose Document (source: section 1.0)

**tst-core-001** [ERROR] Every engineering team must define and document an Overall Purpose covering Business Nature, Business Goals, and Future Considerations.

**tst-core-002** [ERROR] Business Nature must state what the business is in plain, non-technical language.

**tst-core-003** [ERROR] Business Goals must state what the business aims to deliver to its customers.

**tst-core-004** [ERROR] Future Considerations must state the anticipated growth or evolution of the business scope.

---

## Scenarios (source: section 1.1)

**tst-core-005** [ERROR] Scenarios must be written in BDD format using exactly three clauses: GIVEN (actor), WHEN (interaction), THEN (outcome).

**tst-core-006** [ERROR] Scenarios must be non-technical and written from the end-user's perspective — not from an engineer's or system's perspective.

**tst-core-007** [ERROR] Scenario outcomes (THEN) must express results that could be advertised to non-technical stakeholders.

**tst-core-008** [WARN] Chained scenarios must explicitly reference the prior scenario they build on.

---

## Prohibitions (source: sections 0, 1)

**tst-core-009** [ERROR] Scenarios must not contain API endpoints, HTTP status codes, database terms, or UI implementation details unless the business itself is technical.

**tst-core-010** [ERROR] A purpose document must not omit any of the three required sections: Business Nature, Business Goals, Future Considerations.

**tst-core-011** [ERROR] A scenario must not omit the actor (GIVEN), the interaction (WHEN), or the outcome (THEN).
