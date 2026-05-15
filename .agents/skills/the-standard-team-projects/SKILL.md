---
name: the-standard-team-projects
version: 0.1.0
standard-team-version: v0.1.0
applies-to: ["*.sln", "*.csproj", "**/"]
depends-on: []
---

# The Standard Team — Projects

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: Any Visual Studio solution following The Standard Team practices.
0.1/ Who: Any engineer creating or reviewing a solution structure or project folder layout.
0.2/ What: Governs the solution project types, naming conventions, and folder structure within each project.
0.3/ Applies to: Solution files (`*.sln`), project files (`*.csproj`), and folder structures within those projects.
0.4/ Version: The Standard Team v0.1.0
0.5/ Depends on: none

---

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

### 1.0/ Dos

| Rule ID | Requirement |
|---|---|
| [tst-projects-001](rules/rules.md#tst-projects-001) | A solution must include an API project, a Build infrastructure project, a Provision infrastructure project, a Build test project, and an Acceptance test project. |
| [tst-projects-002](rules/rules.md#tst-projects-002) | Project names must follow the pattern: `{Product}.{Layer}` (e.g., `Taarafo.Core`, `Taarafo.Core.Infrastructure.Build`). |
| [tst-projects-003](rules/rules.md#tst-projects-003) | The API project folder structure must organise code into: `Brokers/`, `Models/`, `Services/`, `Controllers/`, and `Migrations/` (when EF is used). |
| [tst-projects-004](rules/rules.md#tst-projects-004) | `Models/` must be sub-organised by service layer: `Foundations/`, `Processings/`, `Orchestrations/`, etc., each containing entity folders with an `Exceptions/` subfolder. |
| [tst-projects-005](rules/rules.md#tst-projects-005) | `Services/` must be sub-organised by service layer: `Foundations/`, `Processings/`, `Orchestrations/`, etc. |
| [tst-projects-006](rules/rules.md#tst-projects-006) | The test project folder structure must mirror the API project structure, grouping tests by `Services\[Service Type]\[ServiceName]`. |

### 1.1/ Don'ts

| Rule ID | Prohibition |
|---|---|
| [tst-projects-007](validations/anti-patterns.md#flat-project-structure) | Projects must not use a flat folder structure — all code must be organised into the defined hierarchy. |
| [tst-projects-008](validations/anti-patterns.md#missing-exceptions-folder) | Entity model folders must not omit the `Exceptions/` subfolder. |

### 1.2/ Ask

- Ask when a project type (e.g., a MAUI or Blazor front-end) does not fit the standard solution structure.
- Ask when a non-EF storage approach makes `Migrations/` inapplicable.

### 1.3/ Defaults

- When EF Core is not used: omit `Migrations/` from the API project.
- When a service layer is not yet implemented: create the folder in advance to maintain structural intent.

### 1.4/ Examples

| | File |
|---|---|
| ✅ | [examples/good/example_good_solution_structure.md](examples/good/example_good_solution_structure.md) |
| ✅ | [examples/good/example_good_project_structure.md](examples/good/example_good_project_structure.md) |
| ❌ | [examples/bad/example_bad_flat_structure.md](examples/bad/example_bad_flat_structure.md) |

---

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Solution and folder structure definitions in Markdown or directory trees.
2.1/ Outcome: Every solution follows the standard project hierarchy and every project follows the standard folder layout, making navigation consistent across all Standard-compliant codebases.
2.2/ Tone: Direct. Cite rule IDs. No personal preference.
