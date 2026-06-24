---
applyTo: "**"
---

# The Standard Team — Pull Requests

## Applies To
All pull requests in repositories following The Standard.

## Rules — Do
- PR title must follow `[CATEGORY]: [Description Of Work Completed]` (tst-pull-requests-001)
- `[CATEGORY]` must be CAPS from the approved category list (tst-pull-requests-002)
- `[Description Of Work Completed]` must be in Pascal Case (tst-pull-requests-003)
- To auto-close an issue on merge, add `Closes #[issue-number]` in the PR description (tst-pull-requests-005)
- To close multiple issues: `Closes #10, closes #123` (tst-pull-requests-006)
- To link without auto-closing, use `#[issue-number]` without a keyword (tst-pull-requests-007)

## Rules — Do Not
- Never use lowercase or mixed-case categories — `foundations:` and `Foundations:` are both wrong (tst-pull-requests-008)
- Never use vague descriptions: `fix`, `update`, `changes`, `wip` without specifics (tst-pull-requests-009)
- Never use a category not in the approved category list (tst-pull-requests-010)

## Defaults
- Examples: `FOUNDATIONS: Add Student` · `BROKERS: Insert Student` · `CONTROLLERS: POST Student`
- Approved categories — TDD: FOUNDATIONS, PROCESSINGS, ORCHESTRATIONS, COORDINATIONS, MANAGEMENTS, AGGREGATIONS, VIEWS, COMPONENTS, PAGES, ACCEPTANCE, INTEGRATION.
- Non-TDD: DATA, BROKERS, CONTROLLERS, INFRA, CONFIG, DOCUMENTATION, DESIGN, IMPORT, STATUS, PROVISION, RELEASE.
