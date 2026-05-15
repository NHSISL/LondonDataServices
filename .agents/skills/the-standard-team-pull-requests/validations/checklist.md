---
skill: the-standard-team-pull-requests
type: checklist
source-section: "4.1.4 Pull Requests"
---

# The Standard Team — Pull Requests — Checklist

Use this checklist before opening or approving a pull request.

---

## PR Title

- [ ] Title follows the format `[CATEGORY]: [Description Of Work Completed]` (tst-pull-requests-001)
- [ ] `[CATEGORY]` is in CAPS (tst-pull-requests-002, tst-pull-requests-008)
- [ ] `[CATEGORY]` is from the approved category list (tst-pull-requests-002, tst-pull-requests-010)
- [ ] `[Description]` is in Pascal Case (tst-pull-requests-003)
- [ ] Description is specific — not `fix`, `update`, `changes`, or `wip` (tst-pull-requests-009)

## PR Description

- [ ] Description includes a summary of what was done (tst-pull-requests-004)
- [ ] For CONTROLLERS or EXPOSERS PRs: screenshots of HTTP responses are included (tst-pull-requests-004)
- [ ] Issues being closed use `Closes #[n]` syntax (tst-pull-requests-005)
- [ ] Multiple issues each use `Closes #[n]` — not a single combined statement (tst-pull-requests-006)
- [ ] Issues being linked only (not closed) use `#[n]` without a keyword (tst-pull-requests-007)
