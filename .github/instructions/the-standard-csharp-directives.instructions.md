---
applyTo: "**/*.cs"
---

# C# Coding Standard — Directives

## Applies To
All C# source files — `using` directive ordering and hygiene.

## Rules — Do
- Place all `using` directives at the top of the file, outside any namespace (tsc-csharp-directives-001)
- Order groups: System namespaces first, third-party next, internal/project last (tsc-csharp-directives-002)
- Separate each group with exactly one blank line (tsc-csharp-directives-003)
- Sort directives alphabetically within each group (tsc-csharp-directives-004)
- Use one `using` directive per line (tsc-csharp-directives-005)

## Rules — Do Not
- Never leave unused `using` directives (tsc-csharp-directives-001)
- Never mix groups without a blank-line separator (tsc-csharp-directives-002)
- Never place `using` directives inside a namespace block (tsc-csharp-directives-003)
- Never alias a namespace unless required to resolve an ambiguity (tsc-csharp-directives-004)
- Never use `using static` except where The Standard explicitly permits it (tsc-csharp-directives-005)

## Defaults
- When only one group is present, no blank-line separator is needed.
- `Microsoft.*` namespaces are treated as third-party unless they are `Microsoft.Extensions.*` already in the SDK.
