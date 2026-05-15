---
applyTo: "**"
---

# Copilot Instructions — The Standard Repository Rules

## Pull Request Rules

- PR title must follow the format: `[CATEGORY]: [Description Of Work Completed]` (tst-pull-requests-001)
- `[CATEGORY]` must be in CAPS and taken from the approved category list (tst-pull-requests-002)
- `[Description Of Work Completed]` must be in Pascal Case (tst-pull-requests-003)
- PR title category must not be lowercase or mixed-case — `foundations:` and `Foundations:` are both wrong (tst-pull-requests-008)
- PR title description must not be vague — `fix`, `update`, `changes`, `wip` without specifics are forbidden (tst-pull-requests-009)
- PR title must not use a category not in the approved category list (tst-pull-requests-010)
- To auto-close an issue on merge, add `Closes #[issue-number]` anywhere in the PR description (tst-pull-requests-005)
- To close multiple issues, list each separately: `Closes #10, closes #123` (tst-pull-requests-006)
- To link an issue without auto-closing, use `#[issue-number]` without a keyword (tst-pull-requests-007)

PR title examples: `FOUNDATIONS: Add Student` · `BROKERS: Insert Student` · `CONTROLLERS: POST Student`

## Commit Message Rules

TDD commits — use FAIL/PASS format for all TDD-required categories:

- When committing a failing test: `[Test Name] -> FAIL` e.g., `ShouldAddStudentAsync -> FAIL` (tst-commits-001)
- When committing the passing implementation: `[Test Name] -> PASS` e.g., `ShouldAddStudentAsync -> PASS` (tst-commits-002)
- TDD commits must not use the `[CATEGORY]: [Description]` format (tst-commits-006)

Non-TDD commits — use category format for all non-TDD categories:

- Format: `[CATEGORY]: [Description Of Work Completed]` e.g., `BROKERS: Insert Student` (tst-commits-003)
- `[CATEGORY]` must always be in CAPS and taken from the approved category list (tst-commits-004)
- `[Description Of Work Completed]` must be in Pascal Case (tst-commits-005)
- Category must not be lowercase or mixed-case (tst-commits-007)
- Description must not be vague — `fix`, `update`, `changes`, `wip` without context are forbidden (tst-commits-008)

## Approved Categories

TDD required (use FAIL/PASS commit pairs):
FOUNDATIONS, PROCESSINGS, ORCHESTRATIONS, COORDINATIONS, MANAGEMENTS, AGGREGATIONS, VIEWS, COMPONENTS, PAGES, ACCEPTANCE, INTEGRATION

Non-TDD (use `[CATEGORY]: [Description]` format):
DATA, BROKERS, CONTROLLERS, INFRA, CONFIG, DOCUMENTATION, DESIGN, IMPORT, STATUS, PROVISION, RELEASE
