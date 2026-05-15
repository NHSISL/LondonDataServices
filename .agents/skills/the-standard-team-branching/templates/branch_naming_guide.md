---
skill: the-standard-team-branching
type: template
source-section: "4.1.1.2 Branch Name Conventions"
---

# Branch Naming Guide

## Pattern

```
users/[username]/[category]-[entity]-[action]
```

## Variables

| Variable | Description | Example |
|---|---|---|
| `[username]` | Your GitHub username | `jsmith` |
| `[category]` | Approved category from the list (CAPS) | `FOUNDATIONS` |
| `[entity]` | The model, service, broker, or component | `Student` |
| `[action]` | What you are doing to the entity | `Add`, `Update`, `Remove` |

## Examples

```
users/jsmith/FOUNDATIONS-Student-Add
users/jsmith/DATA-Student-AddMigration
users/jsmith/BROKERS-StudentStorage-Insert
users/jsmith/MINOR-FIX-StudentService-Validation
users/jsmith/INFRA-Build-Setup
```

## Approved Categories (common)

| Category | When to use |
|---|---|
| `INFRA` | Initial project setup, build scripts |
| `DATA` | Creating or modifying a data model / EF migration |
| `BROKERS` | Creating or modifying a broker |
| `FOUNDATIONS` | Creating or modifying a foundation service |
| `PROCESSINGS` | Creating or modifying a processing service |
| `ORCHESTRATIONS` | Creating or modifying an orchestration service |
| `CONTROLLERS` | Creating or modifying a controller |
| `MINOR FIX` / `MEDIUM FIX` / `MAJOR FIX` | Bug fixes by severity |
| `CODE RUB` | Style, formatting, spelling cleanup (not bug fixes) |
| `DOCUMENTATION` | General documentation |
| `CONFIG` | Configuration changes |

See `contracts/contracts.json` for the full approved category list.
