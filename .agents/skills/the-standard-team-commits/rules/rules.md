---
skill: the-standard-team-commits
type: rules
source-section: "4.1.3 Commits"
---

# The Standard Team — Commits — Rules

Severity levels: `[ERROR]` = must fix · `[WARN]` = should fix.

---

## TDD Commits (source: section 4.1.3)

**tst-commits-001** [ERROR] When committing a failing test in the TDD cycle, the commit message must follow the format: `[Test Name] -> FAIL` — e.g., `ShouldAddStudentAsync -> FAIL`.

**tst-commits-002** [ERROR] When committing the passing implementation in the TDD cycle, the commit message must follow the format: `[Test Name] -> PASS` — e.g., `ShouldAddStudentAsync -> PASS`.

---

## Non-TDD Commits (source: section 4.1.3)

**tst-commits-003** [ERROR] Non-TDD commits (DATA, BROKERS, CONTROLLERS, CONFIG, INFRA, etc.) must follow the format: `[CATEGORY]: [Description Of Work Completed]` — e.g., `DATA: Add Student Model`, `BROKERS: Insert Student`, `CONTROLLERS: POST Student`.

**tst-commits-004** [ERROR] `[CATEGORY]` must always be written in CAPS and must be taken from the approved category list (see contracts/contracts.json).

**tst-commits-005** [ERROR] `[Description Of Work Completed]` must be written in Pascal Case.

---

## Prohibitions

**tst-commits-006** [ERROR] TDD commit messages must not use the `[CATEGORY]: [Description]` format — they must use `[Test Name] -> FAIL` or `[Test Name] -> PASS`.

**tst-commits-007** [ERROR] The category in a non-TDD commit must not be lowercase or mixed-case (e.g., `foundations:` or `Foundations:` are both wrong — must be `FOUNDATIONS:`).

**tst-commits-008** [WARN] Commit descriptions must not be vague. Terms like `fix`, `update`, `changes`, or `wip` without context are forbidden.
