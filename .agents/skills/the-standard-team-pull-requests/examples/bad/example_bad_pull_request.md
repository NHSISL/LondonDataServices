---
skill: the-standard-team-pull-requests
type: example
source-section: "4.1.4 Pull Requests"
demonstrates: "tst-pull-requests-008, tst-pull-requests-009, tst-pull-requests-010 — incorrect PR titles and descriptions"
---

# ❌ Incorrect Pull Request Examples

---

## Lowercase Category (tst-pull-requests-008)

**Title:** `foundations: Add Student`

*Category must be CAPS: `FOUNDATIONS: Add Student`*

---

## Mixed-Case Category (tst-pull-requests-008)

**Title:** `Foundations: Add Student`

*Category must be CAPS: `FOUNDATIONS: Add Student`*

---

## Vague Description (tst-pull-requests-009)

**Title:** `FOUNDATIONS: fix`

*Description must be specific and Pascal Case: `FOUNDATIONS: Fix Student Validation On Add`*

**Title:** `BROKERS: update`

*Description must be specific: `BROKERS: Update Student Select By Id Query`*

---

## Unapproved Category (tst-pull-requests-010)

**Title:** `FEATURE: Add Student`

*`FEATURE` is not an approved category. Use `FOUNDATIONS: Add Student`.*

**Title:** `BUGFIX: Fix Student Null Check`

*`BUGFIX` is not an approved category. Use `MINOR FIX: Student Null Check On Add`.*

---

## Missing Issue Close Link (tst-pull-requests-005)

**Title:** `FOUNDATIONS: Add Student`

**Description:**

This resolves the student feature request mentioned in issue 15.

*Issue must be linked with `Closes #15` — not an informal mention.*
