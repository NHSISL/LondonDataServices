---
skill: the-standard-team-pull-requests
type: anti-patterns
source-section: "4.1.4 Pull Requests"
---

# The Standard Team — Pull Requests — Anti-Patterns

---

## Lowercase PR Category

**Violates:** tst-pull-requests-008

**What happens:** The PR title category is submitted in lowercase or mixed-case.

**Why it's wrong:** Categories must be in CAPS to be machine-parseable and consistent with the commit and branch naming standards. Mixed-case titles break visual scanning in PR lists and tooling integrations.

**Fix:** Capitalise the entire category.

```
# ❌
foundations: Add Student
Brokers: Insert Student

# ✅
FOUNDATIONS: Add Student
BROKERS: Insert Student
```

---

## Vague PR Title

**Violates:** tst-pull-requests-009

**What happens:** The PR title description uses generic terms that do not communicate scope.

**Why it's wrong:** Reviewers must be able to understand the scope of a PR from its title alone. Vague titles require opening the diff to understand what changed.

**Fix:** Use a specific, action-oriented description in Pascal Case.

```
# ❌
FOUNDATIONS: fix
BROKERS: update
CONTROLLERS: changes

# ✅
FOUNDATIONS: Add Student
BROKERS: Insert Student
CONTROLLERS: POST Student
```

---

## Unapproved PR Category

**Violates:** tst-pull-requests-010

**What happens:** The PR title uses a category not present in the approved list.

**Why it's wrong:** Unapproved categories cannot be correctly scored or classified. They break the category taxonomy that underpins contribution tracking and automated tooling.

**Fix:** Replace with the closest approved category.

```
# ❌
FEATURE: Add Student Management
BUGFIX: Fix Student Validation

# ✅
FOUNDATIONS: Add Student Management
MINOR FIX: Student Validation On Add
```

---

## Missing Issue Link on Closing Work

**Violates:** tst-pull-requests-005

**What happens:** A PR fixes or implements a tracked issue but the description does not include `Closes #[n]`.

**Why it's wrong:** Without the closing keyword, the issue remains open after merge. This creates manual cleanup work and breaks traceability.

**Fix:** Add `Closes #[issue-number]` to the PR description.

```
# ❌ Issue mentioned informally
This fixes the bug described in issue 42.

# ✅ Issue linked with auto-close
Closes #42
```
