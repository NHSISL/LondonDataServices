---
skill: the-standard-team-core
type: example
source-section: "1.1 Scenarios"
demonstrates: "tst-core-005, tst-core-006, tst-core-007, tst-core-008 — correct BDD scenarios including chaining"
---

# ABC Bakery — Product Scenarios

## Scenario 1 — Customer orders online

**GIVEN** a customer of ABC Bakery
**WHEN** I want to order a pie
**THEN** I should be able to place the order from home

---

## Scenario 2 — Delivery guarantee (chains from Scenario 1)

**GIVEN** a customer of ABC Bakery
**GIVEN** I order a pie online *(established in Scenario 1)*
**THEN** I should receive it within 5 minutes

---

## Scenario 3 — Cooker views pending orders

**GIVEN** a cooker of ABC Bakery
**WHEN** I need to see pending orders
**THEN** I should be able to view all my pending orders

---

## Scenario 4 — Manager monitors business

**GIVEN** a manager of ABC Bakery
**WHEN** I need to monitor the progress of my business
**THEN** I should be able to see all activity of my business
