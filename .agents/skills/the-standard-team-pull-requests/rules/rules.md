---
skill: the-standard-team-pull-requests
type: rules
source-section: "4.1.4 Pull Requests"
---

# The Standard Team — Pull Requests — Rules

Severity levels: `[ERROR]` = must fix · `[WARN]` = should fix.

---

## PR Title (source: section 4.1.4)

**tst-pull-requests-001** [ERROR] PR title must follow the format: `[CATEGORY]: [Description Of Work Completed]` — e.g., `FOUNDATIONS: Add Student`.

**tst-pull-requests-002** [ERROR] `[CATEGORY]` must be in CAPS and taken from the approved category list.

**tst-pull-requests-003** [ERROR] `[Description Of Work Completed]` must be written in Pascal Case.

---

## PR Description (source: section 4.1.4)

**tst-pull-requests-004** [WARN] The PR description should include any additional information helpful to the approver — e.g., screenshots of controller responses showing `201 Created`, `400 Bad Request`, `424 Failed Dependency`.

**tst-pull-requests-005** [ERROR] To auto-close an issue when the PR is merged, add `Closes #[issue-number]` anywhere in the PR description.

**tst-pull-requests-006** [ERROR] To auto-close multiple issues, list each separately: `Closes #10, closes #123`.

**tst-pull-requests-007** [WARN] To link an issue without auto-closing it, add `#[issue-number]` without a keyword.

---

## Prohibitions

**tst-pull-requests-008** [ERROR] PR title category must not be lowercase or mixed-case.

**tst-pull-requests-009** [WARN] PR title description must not be vague (`fix`, `update`, `changes`, `wip` without specifics are forbidden).

**tst-pull-requests-010** [ERROR] PR title must not use a category that is not in the approved category list.
