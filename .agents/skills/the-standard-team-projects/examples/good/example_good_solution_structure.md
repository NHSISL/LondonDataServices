---
skill: the-standard-team-projects
type: example
source-section: "4.1.2.1 Projects In The Solution"
demonstrates: "tst-projects-001, tst-projects-002 — correct solution structure and naming"
---

# ✅ Standard Solution Structure

```
Taarafo.Core                              (API)
Taarafo.Core.Infrastructure.Build         (Console App)
Taarafo.Core.Infrastructure.Provision     (Console App)
Taarafo.Core.Tests.Acceptance             (xUnit Test Project)
Taarafo.Core.Tests.Build                  (xUnit Test Project)
```

All project names follow the `{Product}.{Layer}` pattern.
All five required project types are present.
