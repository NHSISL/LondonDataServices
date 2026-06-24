---
skill: the-standard-team-branching
type: checklist
source-section: "4.1.1 Forking and Branching Strategies"
---

# The Standard Team — Branching — Checklist

Use this checklist before pushing a branch or opening a pull request.

---

## Forking Workflow

- [ ] Work was done on a fork of the official repository — not a direct clone of it (tst-branching-001, tst-branching-007)
- [ ] The branch was pushed to the fork, not to the official repository (tst-branching-007)

## Branch Name

- [ ] Branch name matches the pattern `users/[username]/[category]-[entity]-[action]` (tst-branching-002)
- [ ] `[username]` is the contributor's GitHub username (tst-branching-004)
- [ ] `[category]` is taken from the approved category list (tst-branching-003, tst-branching-009)
- [ ] `[entity]` identifies the model, service, or component being worked on (tst-branching-005)
- [ ] `[action]` describes what is being done to the entity (tst-branching-006)
