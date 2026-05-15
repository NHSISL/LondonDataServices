---
skill: the-standard-team-commits
type: anti-patterns
source-section: "4.1.3 Commits"
---

# The Standard Team — Commits — Anti-Patterns

---

## Wrong TDD Format

**Violates:** tst-commits-006

**What happens:** A TDD (test-first) commit uses the category format instead of the `-> FAIL` / `-> PASS` format.

**Why it's wrong:** The FAIL/PASS format is the audit trail of the TDD cycle. It makes clear which specific test was written and whether the commit represents the red phase or the green phase. Using a category format erases this signal.

**Fix:** Use `[Test Name] -> FAIL` for the failing test commit and `[Test Name] -> PASS` for the passing implementation commit.

```
# ❌ Wrong — category format used for TDD work
FOUNDATIONS: Add ShouldAddStudentAsync Test
FOUNDATIONS: Implement AddStudentAsync

# ✅ Correct — TDD format
ShouldAddStudentAsync -> FAIL
ShouldAddStudentAsync -> PASS
```

---

## Lowercase Category

**Violates:** tst-commits-007

**What happens:** A non-TDD commit message uses a lowercase or mixed-case category.

**Why it's wrong:** Categories must be in CAPS to be machine-parseable and visually scannable in git log output. Mixed-case categories break tooling and review consistency.

**Fix:** Capitalise the entire category.

```
# ❌ Wrong casing
foundations: Add Student Service
Foundations: Add Student Service
Brokers: Insert Student

# ✅ Correct casing
FOUNDATIONS: Add Student Service
BROKERS: Insert Student
```

---

## Vague Description

**Violates:** tst-commits-008

**What happens:** A commit description uses generic terms like `fix`, `update`, `changes`, or `wip` without specifying what was fixed, updated, or changed.

**Why it's wrong:** Vague descriptions make git history useless for review, debugging, and release note generation. Reviewers cannot understand the scope of the change without reading the diff.

**Fix:** Use a specific, action-oriented description in Pascal Case.

```
# ❌ Vague
DATA: fix
BROKERS: update
CONTROLLERS: changes
FOUNDATIONS: wip

# ✅ Specific
DATA: Add Student Model
BROKERS: Insert Student
CONTROLLERS: POST Student
FOUNDATIONS: Add Student Validation Logic
```
