# C# Coding Standard — Methods — Checklist

- [ ] Every `return` statement that is preceded by other statements has exactly one blank line before it (tsc-csharp-methods-001)
- [ ] No line in any method exceeds 120 characters (tsc-csharp-methods-002)
- [ ] All method calls / declarations that exceed 120 characters use one-parameter-per-line breaking aligned to the opening parenthesis (tsc-csharp-methods-003)
- [ ] All literal arguments in method calls use named parameters (tsc-csharp-methods-004)
- [ ] Each method operates at a single level of abstraction (tsc-csharp-methods-005)
- [ ] Consecutive stacked single-argument calls have no blank lines between them (tsc-csharp-methods-006)
- [ ] LINQ / fluent chains that exceed 120 characters are broken to one call per line (tsc-csharp-methods-007)
- [ ] No mixed uglification inside a single chain — all calls are either all-inline or all-broken (tsc-csharp-methods-008)
- [ ] All method names use full English words with no abbreviations (tsc-csharp-methods-009)
