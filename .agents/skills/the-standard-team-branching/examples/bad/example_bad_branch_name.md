---
skill: the-standard-team-branching
type: example
source-section: "4.1.1.2 Branch Name Conventions"
demonstrates: "tst-branching-008, tst-branching-009 — invalid branch names"
---

# ❌ Invalid Branch Names

The following branch names violate the `users/[username]/[category]-[entity]-[action]` pattern
or use unapproved categories.

| Branch Name | Violation | Rule |
|---|---|---|
| `feature/add-student` | Missing `users/[username]/` prefix; unapproved category `feature` | tst-branching-008, tst-branching-009 |
| `fix-bug-123` | Missing all required segments | tst-branching-008 |
| `john/student` | Missing `users/` prefix; missing category and action | tst-branching-008 |
| `FOUNDATIONS-Student-Add` | Missing `users/[username]/` prefix | tst-branching-008 |
| `users/jsmith/BUGFIX-Student-Update` | `BUGFIX` is not an approved category — use `MINOR FIX` / `MEDIUM FIX` / `MAJOR FIX` | tst-branching-009 |
| `users/jsmith/FEATURE-Student-Add` | `FEATURE` is not an approved category | tst-branching-009 |
| `users/jsmith/Student-Add` | Missing category segment | tst-branching-008 |
| `users/jsmith/foundations-Student-Add` | Category must be CAPS: `FOUNDATIONS` not `foundations` | tst-branching-003 |
