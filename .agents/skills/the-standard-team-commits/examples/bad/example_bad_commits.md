---
skill: the-standard-team-commits
type: example
source-section: "4.1.3 Commits"
demonstrates: "tst-commits-006, tst-commits-007, tst-commits-008 — incorrect commit messages"
---

# ❌ Incorrect Commit Messages

## Wrong Format for TDD Work (tst-commits-006)

```
# ❌ Category format used instead of FAIL/PASS
FOUNDATIONS: Add ShouldAddStudentAsync Test
FOUNDATIONS: Implement AddStudentAsync Method

# ✅ Correct
ShouldAddStudentAsync -> FAIL
ShouldAddStudentAsync -> PASS
```

## Lowercase or Mixed-Case Category (tst-commits-007)

```
# ❌
foundations: Add Student Service
Brokers: Insert Student
controllers: POST Student

# ✅
FOUNDATIONS: Add Student Service
BROKERS: Insert Student
CONTROLLERS: POST Student
```

## Vague Description (tst-commits-008)

```
# ❌
DATA: fix
BROKERS: update
CONTROLLERS: changes
FOUNDATIONS: wip
fix bug
update service

# ✅
DATA: Add Student Model
BROKERS: Insert Student
CONTROLLERS: POST Student
FOUNDATIONS: Add Student Validation For Identity
```
