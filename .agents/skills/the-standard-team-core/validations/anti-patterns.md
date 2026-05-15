---
skill: the-standard-team-core
type: anti-patterns
source-section: "0 Introduction, 1 Purposing"
---

# The Standard Team — Core — Anti-Patterns

---

## Technical Scenarios

**Violates:** tst-core-009

**What happens:** A scenario describes implementation-level details such as API endpoints, HTTP status codes, database calls, or UI component names.

**Why it's wrong:** Scenarios are raw requirements written for all stakeholders including non-technical ones. Technical language makes them inaccessible, conflates requirements with design, and couples business intent to implementation decisions that may change.

**Fix:** Rewrite the THEN clause as an outcome observable by the end-user, without reference to how it is implemented.

```markdown
# ❌ Technical scenario
GIVEN a customer of ABC Bakery
WHEN I submit a POST request to /api/orders
THEN I should receive a 201 Created response with the order ID

# ✅ Non-technical scenario
GIVEN a customer of ABC Bakery
WHEN I want to place an order online
THEN I should be able to complete my order from home and receive a confirmation
```

---

## Missing Purpose Sections

**Violates:** tst-core-010

**What happens:** A purpose document is submitted without one or more of the three required sections (Business Nature, Business Goals, Future Considerations).

**Why it's wrong:** Each section serves a distinct function. Business Nature anchors what the system is; Business Goals align engineering effort with customer value; Future Considerations prevent engineers from building a system that cannot scale to its intended scope.

**Fix:** Add every missing section before the document is considered complete.

```markdown
# ❌ Incomplete purpose document
## Business Nature
A bakery that sells bread and cakes.

## Business Goals
Deliver the highest quality baked goods to customers.

# Missing: Future Considerations

# ✅ Complete purpose document
## Business Nature
A bakery is an establishment that produces and sells flour-based food baked in an oven
such as bread, cookies, cakes, pastries, and pies.

## Business Goals
Deliver the highest quality of bakeries to as many customers as possible.

## Future Considerations
Expanding to become a nation-wide bakery business selling across the country.
```

---

## Missing Scenario Clauses

**Violates:** tst-core-011

**What happens:** A scenario is written without one or more of the three required BDD clauses: GIVEN, WHEN, THEN.

**Why it's wrong:** Each clause carries a distinct semantic role. Missing GIVEN means the actor is unknown. Missing WHEN means the trigger is undefined. Missing THEN means the acceptance criterion does not exist.

**Fix:** Ensure every scenario has all three clauses.

```markdown
# ❌ Missing GIVEN
WHEN I want to order a pie
THEN I should be able to place the order from home

# ✅ Complete scenario
GIVEN a customer of ABC Bakery
WHEN I want to order a pie
THEN I should be able to place the order from home
```
