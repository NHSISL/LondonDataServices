---
skill: the-standard-team-commits
type: checklist
source-section: "4.1.3 Commits"
---

# The Standard Team — Commits — Checklist

Use this checklist before committing.

---

## TDD Commits

- [ ] Failing test commit uses format `[Test Name] -> FAIL` (tst-commits-001)
- [ ] Passing implementation commit uses format `[Test Name] -> PASS` (tst-commits-002)
- [ ] TDD commits do NOT use the `[CATEGORY]: [Description]` format (tst-commits-006)

## Non-TDD Commits

- [ ] Commit uses format `[CATEGORY]: [Description Of Work Completed]` (tst-commits-003)
- [ ] `[CATEGORY]` is in CAPS (tst-commits-004, tst-commits-007)
- [ ] `[CATEGORY]` is from the approved category list (tst-commits-004)
- [ ] `[Description]` is in Pascal Case (tst-commits-005)
- [ ] Description is specific — not `fix`, `update`, `changes`, or `wip` (tst-commits-008)
