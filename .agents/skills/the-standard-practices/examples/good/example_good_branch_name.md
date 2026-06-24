---
skill: the-standard-practices
type: example
source-section: "Chapter 4 / Branching"
demonstrates: "ts-practices-002 — correct branch naming"
---

# ✅ Good Branch Names

```
users/hassanhabib/features/add-student-service
users/cjdutoit/bugs/fix-student-validation-null-check
users/johndoe/configurations/update-github-actions-pipeline
users/janedoe/documentation/update-broker-readme
```

# ❌ Bad Branch Names

```
feature/add-student        # missing users/{handle}/ prefix
users/hassanhabib/feature  # type not plural; no description
AddStudentService          # PascalCase, missing all structure
fix-bug                    # missing users/{handle}/{type}/
```
