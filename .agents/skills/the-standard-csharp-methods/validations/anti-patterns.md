# C# Coding Standard — Methods — Anti-Patterns

## Missing Return Blank Line

**Violates:** tsc-csharp-methods-001
**What happens:** A `return` statement immediately follows other statements with no blank line between them.
**Why it's wrong:** The blank line acts as a visual cue that the method is concluding. Its absence makes the return harder to spot during review.
**Fix:** Insert one blank line immediately before the `return` statement.

## Line Too Long

**Violates:** tsc-csharp-methods-002
**What happens:** A method call, declaration, or body line is 130+ characters wide.
**Why it's wrong:** Lines longer than 120 characters require horizontal scrolling and break most diff views.
**Fix:** Break the line using the one-parameter-per-line strategy (see tsc-csharp-methods-003).

## Long Chain

**Violates:** tsc-csharp-methods-007
**What happens:** A LINQ query is written on a single line: `students.Where(s => s.IsActive).OrderBy(s => s.Name).Select(s => s.Id).ToList();`
**Why it's wrong:** The line exceeds 120 characters and is unreadable on small screens or in diff views.
**Fix:** Break each chained call onto its own line, indented one level from the variable.

## Mixed Chaining

**Violates:** tsc-csharp-methods-008
**What happens:** One chained call has its argument broken onto a new line, while the remaining chained calls stay on the same line as the previous call's closing parenthesis.
**Why it's wrong:** Inconsistent breaking makes the chain visually confusing — it is neither fully uglified nor fully beautified.
**Fix:** Either keep all chained calls on one line (if ≤ 120 chars), or break every chained call to its own line.

## Abbreviated Name

**Violates:** tsc-csharp-methods-009
**What happens:** A method is named `CalcTtl`, `GetStud`, or `ProcReq`.
**Why it's wrong:** Abbreviations harm readability and force the reader to decode intent. Full words make code self-documenting.
**Fix:** Rename to `CalculateTotal`, `GetStudent`, `ProcessRequest`.
