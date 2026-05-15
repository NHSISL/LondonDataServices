---
skill: the-standard-team-commits
type: example
source-section: "4.1.3 Commits"
demonstrates: "tst-commits-001, tst-commits-002, tst-commits-003, tst-commits-004, tst-commits-005 — correct commit messages"
---

# ✅ Correct Commit Messages

## TDD Commit Pairs (FOUNDATIONS layer)

```
ShouldAddStudentAsync -> FAIL
ShouldAddStudentAsync -> PASS

ShouldRetrieveStudentByIdAsync -> FAIL
ShouldRetrieveStudentByIdAsync -> PASS

ShouldModifyStudentAsync -> FAIL
ShouldModifyStudentAsync -> PASS

ShouldRemoveStudentByIdAsync -> FAIL
ShouldRemoveStudentByIdAsync -> PASS
```

## Non-TDD Commits

```
DATA: Add Student Model
DATA: Add Student Migration

BROKERS: Insert Student
BROKERS: Select Student By Id
BROKERS: Update Student
BROKERS: Delete Student By Id

CONTROLLERS: POST Student
CONTROLLERS: GET Student By Id
CONTROLLERS: PUT Student
CONTROLLERS: DELETE Student By Id

INFRA: Setup Build Project
CONFIG: Add AppSettings Development
DOCUMENTATION: Update README With Setup Instructions
```
