# C# Coding Standard — Directives — Checklist

- [ ] All `using` directives appear at the top of the file, outside any namespace block (tsc-csharp-directives-001, tsc-csharp-directives-007)
- [ ] Directives are split into exactly three groups: System → third-party → internal (tsc-csharp-directives-002)
- [ ] Each group is separated by exactly one blank line (tsc-csharp-directives-003)
- [ ] Directives within each group are sorted alphabetically ascending (tsc-csharp-directives-004)
- [ ] Each directive occupies its own line (tsc-csharp-directives-005)
- [ ] No unused `using` directives remain (tsc-csharp-directives-006)
- [ ] No `using` aliases used for brevity — only for genuine ambiguity (tsc-csharp-directives-008)
- [ ] No `using static` present unless Standard-permitted (tsc-csharp-directives-009)
