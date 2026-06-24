---
skill: the-standard-practices
type: template
source-section: "Chapter 4 / Branching"
---

# Branch Naming Guide

## Pattern

```
users/{github-handle}/{type}/{short-description}
```

## Type Values

| Type | When to Use |
|------|-------------|
| `features` | New functionality or enhancements |
| `bugs` | Bug fixes |
| `configurations` | CI/CD, infrastructure, project settings |
| `documentation` | Documentation-only changes |

## Rules

- All lowercase.
- Words separated by hyphens (kebab-case).
- `{short-description}` must describe the change, not the ticket number.
- Maximum recommended length: 60 characters total.

## Examples

```
users/hassanhabib/features/add-student-registration
users/cjdutoit/bugs/fix-null-reference-in-student-validation
users/johndoe/configurations/add-sonarcloud-analysis
users/janedoe/documentation/add-broker-implementation-guide
```
