---
skill: the-standard-team-pull-requests
type: example
source-section: "4.1.4 Pull Requests"
demonstrates: "tst-pull-requests-001, tst-pull-requests-002, tst-pull-requests-003, tst-pull-requests-004, tst-pull-requests-005 — correct PR title and description"
---

# ✅ Correct Pull Request Examples

---

## Example 1 — Foundation Service PR

**Title:** `FOUNDATIONS: Add Student`

**Description:**

Added the full CRUD foundation service for the Student entity including:
- Add, Retrieve by Id, Retrieve All, Modify, and Remove operations
- Validation partials for all operations
- Exception partials for all operations
- Full unit test coverage (TDD — all tests passing)

Closes #15

---

## Example 2 — Controllers PR with Screenshots

**Title:** `CONTROLLERS: POST Student`

**Description:**

Added the POST endpoint for the Student entity.

Response outcomes verified:

- `201 Created` — valid student payload
- `400 Bad Request` — invalid student id or null payload
- `424 Failed Dependency` — storage broker unavailable
- `500 Internal Server Error` — unhandled exception

![201 Created](screenshots/201_created.png)
![400 Bad Request](screenshots/400_bad_request.png)

Closes #22

---

## Example 3 — Multiple Issue Links

**Title:** `MINOR FIX: Student Validation On Modify`

**Description:**

Fixed validation logic for student modify operation. The student id was not being validated against the route id.

Closes #30, closes #31
