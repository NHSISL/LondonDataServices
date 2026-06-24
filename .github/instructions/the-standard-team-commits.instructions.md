---
applyTo: "**"
---

# The Standard Team — Commits

## Applies To
All commit messages in repositories following The Standard.

## Rules — Do
- TDD categories: commit a failing test as `[Test Name] -> FAIL` (tst-commits-001)
- TDD categories: commit the passing implementation as `[Test Name] -> PASS` (tst-commits-002)
- Non-TDD categories: format as `[CATEGORY]: [Description Of Work Completed]` (tst-commits-003)
- `[CATEGORY]` must be CAPS from the approved category list (tst-commits-004)
- `[Description Of Work Completed]` must be in Pascal Case (tst-commits-005)

## Rules — Do Not
- Never use `[CATEGORY]: [Description]` format for TDD commits (tst-commits-006)
- Never use lowercase or mixed-case categories (tst-commits-007)
- Never use vague descriptions: `fix`, `update`, `changes`, `wip` without specifics (tst-commits-008)

## Defaults
- TDD categories: FOUNDATIONS, PROCESSINGS, ORCHESTRATIONS, COORDINATIONS, MANAGEMENTS, AGGREGATIONS, VIEWS, COMPONENTS, PAGES, ACCEPTANCE, INTEGRATION.
- Non-TDD categories: DATA, BROKERS, CONTROLLERS, INFRA, CONFIG, DOCUMENTATION, DESIGN, IMPORT, STATUS, PROVISION, RELEASE.
