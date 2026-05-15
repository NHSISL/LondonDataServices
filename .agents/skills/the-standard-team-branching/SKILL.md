---
name: the-standard-team-branching
version: 0.1.0
standard-team-version: v0.1.0
applies-to: ["*", ".git/*"]
depends-on: []
---

# The Standard Team — Branching

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: Any repository using The Standard Team open-source forking and branching workflow.
0.1/ Who: Any engineer creating, naming, or reviewing branches.
0.2/ What: Governs the forking workflow, branch naming conventions, and category usage for all branches.
0.3/ Applies to: Branch names in any git repository following The Standard Team practices.
0.4/ Version: The Standard Team v0.1.0
0.5/ Depends on: none

---

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

### 1.0/ Dos

| Rule ID | Requirement |
|---|---|
| [tst-branching-001](rules/rules.md#tst-branching-001) | All open-source contributions must follow the forking workflow — fork the official repo, clone the fork, create a branch, push to the fork, open a PR. |
| [tst-branching-002](rules/rules.md#tst-branching-002) | Branch names must follow the pattern: `users/[username]/[category]-[entity]-[action]`. |
| [tst-branching-003](rules/rules.md#tst-branching-003) | `[category]` must be taken from the approved category list (see contracts/contracts.json). |
| [tst-branching-004](rules/rules.md#tst-branching-004) | `[username]` must be the contributor's GitHub username. |
| [tst-branching-005](rules/rules.md#tst-branching-005) | `[entity]` must identify the model or service being worked on. |
| [tst-branching-006](rules/rules.md#tst-branching-006) | `[action]` must describe what is being done to the entity. |

### 1.1/ Don'ts

| Rule ID | Prohibition |
|---|---|
| [tst-branching-007](validations/anti-patterns.md#direct-push-to-official-repo) | Contributors must not push directly to the official repository — always use a fork. |
| [tst-branching-008](validations/anti-patterns.md#invalid-branch-name) | Branch names must not deviate from the `users/[username]/[category]-[entity]-[action]` pattern. |
| [tst-branching-009](validations/anti-patterns.md#unapproved-category) | Branch names must not use categories not present in the approved category list. |

### 1.2/ Ask

- Ask when the correct category is ambiguous between two similar categories (e.g., `MINOR FIX` vs `MINOR CODE RUB`).
- Ask when the entity name is unclear or spans multiple models.

### 1.3/ Defaults

- When no category fits precisely: choose the closest approved category and note the deviation in the PR description.
- When action is a simple TDD iteration: use `Add` for new work, `Update` for changes.

### 1.4/ Examples

| | File |
|---|---|
| ✅ | [examples/good/example_good_branch_name.md](examples/good/example_good_branch_name.md) |
| ❌ | [examples/bad/example_bad_branch_name.md](examples/bad/example_bad_branch_name.md) |

---

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Branch name string following `users/[username]/[category]-[entity]-[action]`.
2.1/ Outcome: Every branch is traceable to a contributor, a category of work, an entity, and an action — enabling reviewers to understand scope before opening a PR.
2.2/ Tone: Direct. Cite rule IDs. No personal preference.
