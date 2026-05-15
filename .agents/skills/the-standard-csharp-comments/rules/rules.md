# C# Coding Standard — Comments and Documentation — Rules

## COMMENT USAGE

**tsc-csharp-comments-001** [ERROR] Comments must only be used to explain what code cannot express — non-obvious intent, inaccessible/generated code, or complex algorithm rationale.
**tsc-csharp-comments-004** [ERROR] Comments must not restate what the code already clearly expresses (redundant comments are forbidden).

## COPYRIGHT

**tsc-csharp-comments-002** [ERROR] Copyright headers must use the exact dashed-line `//` format:
```
// ---------------------------------------------------------------
// Copyright (c) {Owner}
// {License line}
// ---------------------------------------------------------------
```
**tsc-csharp-comments-005** [ERROR] Copyright headers must not use `/* */` block comment syntax.
**tsc-csharp-comments-006** [ERROR] Copyright headers must not use XML `<copyright>` tag syntax.

## METHOD DOCUMENTATION

**tsc-csharp-comments-003** [ERROR] Methods that perform operations inaccessible at dev-time or that implement complex logic must be documented with: Purposing, Incomes, Outcomes, and Side Effects sections.
