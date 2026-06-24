# C# Coding Standard — Directives — Rules

## Placement

**tsc-csharp-directives-001** [ERROR] All `using` directives must appear at the top of the file, outside any namespace declaration.

## Ordering and Grouping

**tsc-csharp-directives-002** [ERROR] `using` directives must be ordered in three groups: (1) `System.*` namespaces, (2) third-party namespaces, (3) internal/project namespaces. Each group must be separated by exactly one blank line.

**tsc-csharp-directives-003** [ERROR] Groups must be separated by exactly one blank line — no more, no less.

**tsc-csharp-directives-004** [WARN]  Directives within each group must be sorted alphabetically (case-insensitive, ascending).

**tsc-csharp-directives-005** [ERROR] Each `using` directive must appear on its own line — never combine multiple namespaces on one line.

## Hygiene

**tsc-csharp-directives-006** [ERROR] Unused `using` directives must be removed before committing.

**tsc-csharp-directives-007** [ERROR] `using` directives must not appear inside a namespace block.

**tsc-csharp-directives-008** [WARN]  `using` aliases (`using Alias = Full.Type.Name;`) must only be used to resolve genuine ambiguities — never for brevity alone.

**tsc-csharp-directives-009** [WARN]  `using static` directives must not be used unless The Standard explicitly permits them for the target type.
