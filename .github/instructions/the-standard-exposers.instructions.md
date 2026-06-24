---
applyTo: "**/*Controller*.cs"
---

# The Standard — Exposers (RESTful APIs)

## Applies To
The exposer layer — RESTful API controllers (`*Controller*.cs`).

## Rules — Do
- Name controllers in plural form: `StudentsController` (ts-exposers-001)
- Map HTTP verbs to service operations: POST→Add, GET→Retrieve, PUT→Modify, DELETE→Remove (ts-exposers-002)
- Return correct HTTP status codes: 200 OK, 201 Created, 400 BadRequest, 404 NotFound, 500 InternalServerError (ts-exposers-003)
- Catch service exceptions and map them to the correct HTTP response using `IActionResult` (ts-exposers-004)
- Depend only on service-layer interfaces; never inject brokers (ts-exposers-005)
- Keep controller action bodies thin — one service call and exception mapping only (ts-exposers-006)

## Rules — Do Not
- Must not contain business logic (ts-exposers-001)
- Must not call brokers directly (ts-exposers-002)
- Must not expose internal exception details to the client (ts-exposers-003)
- Must not use singular resource names in the controller class name (ts-exposers-004)

## Defaults
- POST returns 201 Created with the created resource.
- GET by Id returns 200 OK or 404 NotFound.
- PUT returns 200 OK with the updated resource.
- DELETE returns 200 OK with the deleted resource.
