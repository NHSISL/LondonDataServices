---
applyTo: "**/*.sln,**/*.csproj"
---

# The Standard Team — Projects and Solutions

## Applies To
All solution (`.sln`) and project (`.csproj`) files in Standard .NET repositories.

## Rules — Do
- Solution must group projects by layer: `{Name}.sln` containing `{Name}`, `{Name}.Tests.Unit`, `{Name}.Tests.Acceptance`, `{Name}.Infrastructure` (tst-projects-001)
- Each project must target a single responsibility layer (tst-projects-002)
- Reference only the layer directly below: service projects reference broker projects (tst-projects-003)
- Keep test projects in a `Tests/` or `{Name}.Tests.Unit/` folder (tst-projects-004)
- Use `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>` in every project (tst-projects-005)
- Target the latest stable LTS .NET version unless a constraint exists (tst-projects-006)

## Rules — Do Not
- Never add circular project references (tst-projects-007)
- Never add dependencies that skip a layer (tst-projects-003)
- Never commit NuGet package binaries into source control (tst-projects-008)

## Defaults
- Production project: `src/{Name}/{Name}.csproj`.
- Unit test project: `tests/{Name}.Tests.Unit/{Name}.Tests.Unit.csproj`.
