# C# Coding Standard — Variables — Checklist

- [ ] All variable names use full English words — no abbreviations (tsc-csharp-variables-001)
- [ ] All collection variables use the plural form of the element type (tsc-csharp-variables-002)
- [ ] No variable name embeds the type (no `List`, `Array`, `Obj` suffixes) (tsc-csharp-variables-003)
- [ ] `var` is used only when the right-hand type is immediately obvious (tsc-csharp-variables-004)
- [ ] Explicit types are used when the right-hand type is not immediately obvious (tsc-csharp-variables-005)
- [ ] Declarations exceeding 120 characters break the right-hand side to the next line (tsc-csharp-variables-006)
- [ ] Logically distinct declaration groups are separated by one blank line (tsc-csharp-variables-007)
- [ ] All null / default placeholder variables are named `maybe{Entity}` (tsc-csharp-variables-008)
- [ ] No multiple variables are declared on the same line (tsc-csharp-variables-009)
