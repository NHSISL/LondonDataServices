---
skill: the-standard-team-pull-requests
type: template
source-section: "4.1.4 Pull Requests"
---

# Pull Request Author Checklist

Use before opening a pull request.

## Title

- [ ] Format is `[CATEGORY]: [Description Of Work Completed]` (tst-pull-requests-001)
- [ ] Category is in CAPS (tst-pull-requests-002)
- [ ] Category is from the approved list (tst-pull-requests-002)
- [ ] Description is in Pascal Case (tst-pull-requests-003)
- [ ] Description is specific — not `fix`, `update`, `changes` (tst-pull-requests-009)

## Description

- [ ] Summary of changes is included (tst-pull-requests-004)
- [ ] Screenshots included for CONTROLLERS / EXPOSERS PRs (tst-pull-requests-004)
- [ ] `Closes #[n]` used for each issue being auto-closed (tst-pull-requests-005, tst-pull-requests-006)
- [ ] `#[n]` used for issues being linked only (tst-pull-requests-007)

## Scope

- [ ] PR covers a single category — if not, split into separate PRs
- [ ] All commits follow the Standard commit message format (see the-standard-team-commits)
- [ ] Branch name follows the approved naming convention (see the-standard-team-branching)
