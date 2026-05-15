---
skill: the-standard-team-projects
type: rules
source-section: "4.1.2 Projects"
---

# The Standard Team — Projects — Rules

Severity levels: `[ERROR]` = must fix · `[WARN]` = should fix.

---

## Solution Structure (source: section 4.1.2.1 Projects In The Solution)

**tst-projects-001** [ERROR] A solution must include at minimum: an API project, a Build infrastructure project (Console App), a Provision infrastructure project (Console App), a Build test project (xUnit), and an Acceptance test project (xUnit).

**tst-projects-002** [ERROR] Project names must follow the pattern `{Product}.{Layer}` — e.g., `Taarafo.Core`, `Taarafo.Core.Infrastructure.Build`, `Taarafo.Core.Tests.Acceptance`.

---

## Project Folder Structure (source: section 4.1.2.1 Project Structure)

**tst-projects-003** [ERROR] The API project must organise code into top-level folders: `Brokers/`, `Models/`, `Services/`, `Controllers/`. A `Migrations/` folder must be present when EF Core is used.

**tst-projects-004** [ERROR] `Models/` must be sub-organised by service layer (`Foundations/`, `Processings/`, `Orchestrations/`, etc.). Each entity folder within a layer must contain an `Exceptions/` subfolder.

**tst-projects-005** [ERROR] `Services/` must be sub-organised by service layer: `Foundations/`, `Processings/`, `Orchestrations/`, `Coordinations/`, `Managements/`, `Aggregations/` as applicable.

**tst-projects-006** [ERROR] Test project folder structure must mirror the API project structure, grouping tests by `Services\[Service Type]\[ServiceName]`.

---

## Prohibitions

**tst-projects-007** [ERROR] Projects must not use a flat folder structure — all code must be organised into the defined hierarchy.

**tst-projects-008** [ERROR] Entity model folders must not omit the `Exceptions/` subfolder.
