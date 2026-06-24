# The Standard Exposers — Rules

## NAMING

**ts-exposers-001** [ERROR] Controller class names must be plural to match the REST resource (e.g., `StudentsController`, not `StudentController`).

## HTTP VERB MAPPING

**ts-exposers-002** [ERROR] HTTP verbs must map to service operations: POST→Add, GET→Retrieve, PUT→Modify, DELETE→Remove.

## RESPONSE CODES

**ts-exposers-003** [ERROR] Actions must return correct HTTP status codes: POST→201 Created, GET→200 OK or 404 NotFound, PUT→200 OK, DELETE→200 OK, validation failure→400 BadRequest, unexpected error→500 InternalServerError.

## EXCEPTION MAPPING

**ts-exposers-004** [ERROR] Controllers must catch service-layer exceptions and map them to the correct IActionResult; service exceptions must not propagate uncaught.
**ts-exposers-006** [ERROR] Controller action bodies must be thin: one service call plus exception mapping only — no business logic.

## DEPENDENCIES

**ts-exposers-005** [ERROR] Controllers must depend only on service-layer interfaces; broker injection is forbidden.
