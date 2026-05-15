---
name: the-standard-exposers
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*Controller*.cs"]
depends-on: ["the-standard-core", "the-standard-foundations"]
---

# The Standard — Exposers (RESTful APIs)

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: The exposer layer — RESTful API controllers.
0.1/ Who: Engineers implementing or reviewing API controllers.
0.2/ What: Enforces RESTful API controller design: routing, HTTP verbs, response codes, naming, and exception mapping.
0.3/ Applies to: *Controller*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-foundations

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Name controllers in plural form to match REST resource naming (e.g., `StudentsController`) → see rules/rules.md#ts-exposers-001
  2. Map HTTP verbs to service operations: POST→Add, GET→Retrieve, PUT→Modify, DELETE→Remove → see rules/rules.md#ts-exposers-002
  3. Return correct HTTP status codes: 200 OK, 201 Created, 400 BadRequest, 404 NotFound, 500 InternalServerError → see rules/rules.md#ts-exposers-003
  4. Catch service exceptions and map them to the correct HTTP response using `IActionResult` → see rules/rules.md#ts-exposers-004
  5. Depend only on service-layer interfaces; never inject brokers → see rules/rules.md#ts-exposers-005
  6. Keep controller action bodies thin — one service call + exception mapping only → see rules/rules.md#ts-exposers-006

1.1/ Don'ts:
  1. Must not contain business logic → see validations/anti-patterns.md#controller-with-logic
  2. Must not call brokers directly → see validations/anti-patterns.md#controller-broker-access
  3. Must not expose internal exception details to the client → see validations/anti-patterns.md#exception-detail-leak
  4. Must not use singular resource names in the controller class name → see validations/anti-patterns.md#singular-controller-name

1.2/ Ask:
  - Ask when an action needs to accept or return a DTO that differs from the domain model — confirm whether a mapper is needed.

1.3/ Defaults:
  - POST returns 201 Created with the created resource.
  - GET by Id returns 200 OK or 404 NotFound.
  - PUT returns 200 OK with the updated resource.
  - DELETE returns 200 OK with the deleted resource.

1.4/ Examples:
  - ✅ see examples/good/example_good_controller.cs
  - ❌ see examples/bad/example_bad_controller_with_logic.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code.
2.1/ Outcome: Thin controllers that correctly map HTTP verbs, status codes, and service exceptions.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
