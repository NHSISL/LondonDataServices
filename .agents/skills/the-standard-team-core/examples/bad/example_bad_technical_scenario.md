---
skill: the-standard-team-core
type: example
source-section: "1.1 Scenarios"
demonstrates: "tst-core-009 — scenario containing technical implementation details"
---

# ❌ Technical Scenarios — Anti-Pattern

The scenarios below violate tst-core-009 by including API endpoints, HTTP status codes,
and UI component names that are inaccessible to non-technical stakeholders.

---

## ❌ Scenario A — Technical language in WHEN and THEN

**GIVEN** a customer of ABC Bakery
**WHEN** I submit a POST request to `/api/v1/orders` with a valid JSON payload
**THEN** the API should return a `201 Created` response with the new order ID in the body

*Why it's wrong: The WHEN references an HTTP verb and URL path. The THEN references a
status code and JSON body. A non-technical stakeholder cannot understand or verify this.*

---

## ❌ Scenario B — Database detail in THEN

**GIVEN** a cooker of ABC Bakery
**WHEN** I open the orders dashboard
**THEN** the system should query the `Orders` table with status = 'PENDING' and return the results

*Why it's wrong: The THEN describes a database query, not a user-observable outcome.*

---

## ❌ Scenario C — UI component name in THEN

**GIVEN** a manager of ABC Bakery
**WHEN** I navigate to the analytics page
**THEN** the `<ActivityGrid>` component should render all business events from the last 24 hours

*Why it's wrong: The THEN references a UI component name — an implementation detail.*
