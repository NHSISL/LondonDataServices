---
applyTo: "**"
---

# The Standard Team — Branching

## Applies To
All repositories following The Standard — branch naming conventions and workflow.

## Rules — Do
- Branch names must use lowercase kebab-case (tst-branching-001)
- Feature branches must be prefixed with `users/{alias}/` (tst-branching-002)
- Bug fix branches must be prefixed with `bugs/{alias}/` (tst-branching-003)
- Release branches must be prefixed with `releases/` (tst-branching-004)
- Branches must be short-lived and focused on a single concern (tst-branching-005)
- Delete remote branches after merging (tst-branching-007)

## Rules — Do Not
- Never commit directly to `main` (tst-branching-006)
- Never use generic names like `fix`, `test`, `temp`, or `wip` as branch names (tst-branching-008)
- Never reuse old branch names for new work (tst-branching-009)

## Defaults
- Feature branch pattern: `users/{alias}/{short-description}` e.g., `users/jsmith/add-student-view`.
- Merges to `main` go through a pull request with required review.
