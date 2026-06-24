---
skill: the-standard-team-projects
type: checklist
source-section: "4.1.2 Projects"
---

# The Standard Team — Projects — Checklist

Use this checklist when creating or reviewing a solution or project structure.

---

## Solution Structure

- [ ] Solution includes an API project (tst-projects-001)
- [ ] Solution includes a Build infrastructure project (Console App) (tst-projects-001)
- [ ] Solution includes a Provision infrastructure project (Console App) (tst-projects-001)
- [ ] Solution includes a Build test project (xUnit) (tst-projects-001)
- [ ] Solution includes an Acceptance test project (xUnit) (tst-projects-001)
- [ ] All project names follow the `{Product}.{Layer}` naming pattern (tst-projects-002)

## API Project Folder Structure

- [ ] Top-level folders include `Brokers/`, `Models/`, `Services/`, `Controllers/` (tst-projects-003)
- [ ] `Migrations/` is present when EF Core is used (tst-projects-003)
- [ ] No flat structure — code is not placed directly in the project root (tst-projects-007)

## Models Organisation

- [ ] `Models/` is sub-organised by service layer (`Foundations/`, `Processings/`, etc.) (tst-projects-004)
- [ ] Every entity folder within a layer contains an `Exceptions/` subfolder (tst-projects-004, tst-projects-008)

## Services Organisation

- [ ] `Services/` is sub-organised by service layer (`Foundations/`, `Processings/`, etc.) (tst-projects-005)

## Test Project Structure

- [ ] Test project mirrors the API project structure (tst-projects-006)
- [ ] Tests are grouped by `Services\[Service Type]\[ServiceName]` (tst-projects-006)
