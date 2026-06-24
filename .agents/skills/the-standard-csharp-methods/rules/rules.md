# C# Coding Standard — Methods — Rules

## Spacing

**tsc-csharp-methods-001** [ERROR] A blank line must appear immediately before a `return` statement when one or more other statements precede it in the same method body.

**tsc-csharp-methods-006** [ERROR] Multiple consecutive single-argument method calls that form a logical stack must be written one per line with no blank lines between them.

## Line Length

**tsc-csharp-methods-002** [ERROR] No line in a method (declaration or body) may exceed 120 characters.

## Parameter Breaking

**tsc-csharp-methods-003** [ERROR] When a method call or declaration exceeds 120 characters, each parameter must be placed on its own line, aligned with the opening parenthesis.

**tsc-csharp-methods-004** [ERROR] Named parameters must be used when calling a method with literal arguments so that each argument is self-documenting.

## Abstraction

**tsc-csharp-methods-005** [ERROR] A method body must operate at a single level of abstraction — it must not mix high-level orchestration calls with low-level implementation details in the same body.

## Chaining

**tsc-csharp-methods-007** [WARN]  LINQ or fluent chains that exceed 120 characters must be broken so each chained call (`.Where(…)`, `.Select(…)`, etc.) starts on its own line, indented by one level.

**tsc-csharp-methods-008** [ERROR] A line break must not be introduced in the middle of a single chained call's argument list while leaving other chained calls on the same line (no mixed uglification/beautification).

## Naming

**tsc-csharp-methods-009** [ERROR] Method names must be full English words — no abbreviations. Use `CalculateTotal`, not `CalcTtl`.
