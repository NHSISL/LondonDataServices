---
name: the-standard-team-pull-requests
version: 0.1.0
standard-team-version: v0.1.0
applies-to: ["*", ".git/*"]
depends-on: ["the-standard-team-branching", "the-standard-team-commits"]
---

# The Standard Team — Pull Requests

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: Any pull request opened against a Standard-compliant repository.
0.1/ Who: Any engineer creating or reviewing a pull request.
0.2/ What: Governs PR title format, description content, issue linking, and screenshot expectations.
0.3/ Applies to: Pull request titles, descriptions, and comments in any Standard-compliant repository.
0.4/ Version: The Standard Team v0.1.0
0.5/ Depends on: the-standard-team-branching, the-standard-team-commits

---

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

### 1.0/ Dos

| Rule ID | Requirement |
|---|---|
| [tst-pull-requests-001](rules/rules.md#tst-pull-requests-001) | PR title must follow the format: `[CATEGORY]: [Description Of Work Completed]`. |
| [tst-pull-requests-002](rules/rules.md#tst-pull-requests-002) | `[CATEGORY]` must be in CAPS and taken from the approved category list. |
| [tst-pull-requests-003](rules/rules.md#tst-pull-requests-003) | `[Description Of Work Completed]` must be in Pascal Case. |
| [tst-pull-requests-004](rules/rules.md#tst-pull-requests-004) | The PR description may include additional information relevant to the approver (e.g., screenshots of controller responses). |
| [tst-pull-requests-005](rules/rules.md#tst-pull-requests-005) | To auto-close an issue on merge, add `Closes #[issue-number]` anywhere in the PR description. |
| [tst-pull-requests-006](rules/rules.md#tst-pull-requests-006) | To close multiple issues, add each on its own: `Closes #10, closes #123`. |
| [tst-pull-requests-007](rules/rules.md#tst-pull-requests-007) | To link an issue without auto-closing, add `#[issue-number]` without a keyword. |

### 1.1/ Don'ts

| Rule ID | Prohibition |
|---|---|
| [tst-pull-requests-008](validations/anti-patterns.md#lowercase-pr-category) | PR title category must not be lowercase or mixed-case. |
| [tst-pull-requests-009](validations/anti-patterns.md#vague-pr-title) | PR title description must not be vague (`fix`, `update`, `changes`, `wip`). |
| [tst-pull-requests-010](validations/anti-patterns.md#unapproved-pr-category) | PR title must not use categories not in the approved category list. |

### 1.2/ Ask

- Ask when a PR spans multiple categories — it may need to be split.
- Ask when the approver context (description/screenshots) is missing for a CONTROLLERS or EXPOSERS PR.

### 1.3/ Defaults

- When a PR has no description: prompt to add at least a one-line summary of what was done.
- When a PR closes an issue: always use `Closes #[n]` rather than just mentioning the issue number.

### 1.4/ Examples

| | File |
|---|---|
| ✅ | [examples/good/example_good_pull_request.md](examples/good/example_good_pull_request.md) |
| ❌ | [examples/bad/example_bad_pull_request.md](examples/bad/example_bad_pull_request.md) |

---

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: PR title as `[CATEGORY]: [Description In Pascal Case]`; description in Markdown with optional screenshots and issue links.
2.1/ Outcome: Every PR is immediately scannable by reviewers — scope is clear from the title, context is provided in the description, and issues are correctly linked.
2.2/ Tone: Direct. Cite rule IDs. No personal preference.
