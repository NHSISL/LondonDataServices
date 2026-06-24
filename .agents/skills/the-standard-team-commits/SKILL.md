---
name: the-standard-team-commits
version: 0.1.0
standard-team-version: v0.1.0
applies-to: [".git/*", "commit messages"]
depends-on: ["the-standard-team-branching"]
---

# The Standard Team — Commits

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: Any git commit message in a Standard-compliant repository.
0.1/ Who: Any engineer authoring commit messages.
0.2/ What: Governs commit message format for TDD-driven work (FAIL/PASS commits) and non-TDD work (category-based commits).
0.3/ Applies to: All git commit messages.
0.4/ Version: The Standard Team v0.1.0
0.5/ Depends on: the-standard-team-branching

---

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

### 1.0/ Dos

| Rule ID | Requirement |
|---|---|
| [tst-commits-001](rules/rules.md#tst-commits-001) | TDD commits must use the format `[Test Name] -> FAIL` when committing a failing test. |
| [tst-commits-002](rules/rules.md#tst-commits-002) | TDD commits must use the format `[Test Name] -> PASS` when committing the passing implementation. |
| [tst-commits-003](rules/rules.md#tst-commits-003) | Non-TDD commits (DATA, BROKERS, CONTROLLERS, etc.) must use the format `[CATEGORY]: [Description Of Work Completed]`. |
| [tst-commits-004](rules/rules.md#tst-commits-004) | `[CATEGORY]` must always be in CAPS and taken from the approved category list. |
| [tst-commits-005](rules/rules.md#tst-commits-005) | `[Description Of Work Completed]` must be in Pascal Case. |

### 1.1/ Don'ts

| Rule ID | Prohibition |
|---|---|
| [tst-commits-006](validations/anti-patterns.md#wrong-tdd-format) | TDD commit messages must not use category format — they must use `[Test Name] -> FAIL` or `[Test Name] -> PASS`. |
| [tst-commits-007](validations/anti-patterns.md#lowercase-category) | The category in a non-TDD commit must not be lowercase or mixed-case. |
| [tst-commits-008](validations/anti-patterns.md#vague-description) | Commit descriptions must not be vague (e.g., `fix`, `update`, `changes`). |

### 1.2/ Ask

- Ask when it is unclear whether work requires a test (and therefore a FAIL/PASS commit pair).
- Ask when a commit spans multiple categories.

### 1.3/ Defaults

- Code requiring tests (FOUNDATIONS, PROCESSINGS, ORCHESTRATIONS, VIEWS, COMPONENTS): use FAIL/PASS pairs.
- Code not requiring tests (DATA, BROKERS, CONTROLLERS): use `[CATEGORY]: [Description]` format.
- When a commit spans two layers: split into two commits, one per layer.

### 1.4/ Examples

| | File |
|---|---|
| ✅ | [examples/good/example_good_commits.md](examples/good/example_good_commits.md) |
| ❌ | [examples/bad/example_bad_commits.md](examples/bad/example_bad_commits.md) |

---

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Commit message string — either `[Test Name] -> FAIL|PASS` or `[CATEGORY]: [Description In Pascal Case]`.
2.1/ Outcome: Every commit message clearly communicates its category and scope, enabling traceability through the TDD cycle and the category taxonomy.
2.2/ Tone: Direct. Cite rule IDs. No personal preference.
