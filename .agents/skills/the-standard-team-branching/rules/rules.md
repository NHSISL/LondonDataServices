---
skill: the-standard-team-branching
type: rules
source-section: "4.1.1 Forking and Branching Strategies"
---

# The Standard Team — Branching — Rules

Severity levels: `[ERROR]` = must fix · `[WARN]` = should fix.

---

## Forking Workflow (source: section 4.1.1.1)

**tst-branching-001** [ERROR] All open-source contributions must follow the forking workflow: fork the official repository, clone the fork locally, create a feature branch, push to the fork, and open a pull request to the official repository.

---

## Branch Naming (source: section 4.1.1.2)

**tst-branching-002** [ERROR] Branch names must follow the pattern: `users/[username]/[category]-[entity]-[action]`.

**tst-branching-003** [ERROR] `[category]` must be taken from the approved category list defined in contracts/contracts.json.

**tst-branching-004** [ERROR] `[username]` must be the contributor's GitHub username — not a display name or alias.

**tst-branching-005** [ERROR] `[entity]` must identify the model, service, or component being worked on.

**tst-branching-006** [ERROR] `[action]` must describe what is being done to the entity (e.g., `Add`, `Update`, `Remove`).

---

## Prohibitions

**tst-branching-007** [ERROR] Contributors must not push directly to the official repository. All changes must go through a fork and pull request.

**tst-branching-008** [ERROR] Branch names must not deviate from the `users/[username]/[category]-[entity]-[action]` pattern.

**tst-branching-009** [ERROR] Branch names must not use categories that are not in the approved category list.
