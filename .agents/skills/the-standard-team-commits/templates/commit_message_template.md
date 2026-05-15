---
skill: the-standard-team-commits
type: template
source-section: "4.1.3 Commits"
---

# Commit Message Template

## For TDD Work (FOUNDATIONS, PROCESSINGS, ORCHESTRATIONS, VIEWS, COMPONENTS, etc.)

```
[Test Name] -> FAIL
```

```
[Test Name] -> PASS
```

**Examples:**
```
ShouldAddStudentAsync -> FAIL
ShouldAddStudentAsync -> PASS
ShouldThrowValidationExceptionOnAddIfStudentIsNullAsync -> FAIL
ShouldThrowValidationExceptionOnAddIfStudentIsNullAsync -> PASS
```

---

## For Non-TDD Work (DATA, BROKERS, CONTROLLERS, INFRA, CONFIG, etc.)

```
[CATEGORY]: [Description Of Work Completed]
```

**Rules:**
- `[CATEGORY]` → always CAPS, from approved category list
- `[Description]` → Pascal Case, specific and action-oriented

**Examples:**
```
DATA: Add Student Model
DATA: Add Student Migration
BROKERS: Insert Student
BROKERS: Select Student By Id
CONTROLLERS: POST Student
INFRA: Setup Build Project
CONFIG: Add AppSettings Development
DOCUMENTATION: Update README With Setup Instructions
```
