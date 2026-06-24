---
name: the-standard-csharp-comments
version: 0.1.0
csharp-standard-version: v0.8
applies-to: ["*.cs"]
depends-on: ["none"]
---

# C# Coding Standard — Comments and Documentation

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All C# source files requiring comments or documentation.
0.1/ Who: Engineers writing inline comments, copyright headers, or method documentation.
0.2/ What: Enforces comment style, copyright block format, and method documentation requirements.
0.3/ Applies to: *.cs
0.4/ Version: v0.8
0.5/ Depends on: none

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Use comments only to explain what code cannot express — non-obvious intent, inaccessible code, or complex logic → see rules/rules.md#tsc-csharp-comments-001
  2. Format copyright blocks using the exact dashed-line pattern → see rules/rules.md#tsc-csharp-comments-002
  3. Document methods that perform inaccessible or complex operations with Purposing, Incomes, Outcomes, and Side Effects → see rules/rules.md#tsc-csharp-comments-003

1.1/ Don'ts:
  1. Must not use `/* */` block comment syntax for copyright headers → see validations/anti-patterns.md#block-comment-copyright
  2. Must not use XML `<copyright>` tags for copyright headers → see validations/anti-patterns.md#xml-copyright-tag
  3. Must not add comments that merely restate what the code already clearly expresses → see validations/anti-patterns.md#redundant-comment

1.2/ Ask:
  - Ask when a method is complex enough to need documentation — if the intent is clear from the name and types, a comment is not needed.

1.3/ Defaults:
  - Every file that contains production code must start with the copyright header block.
  - Test files follow the same copyright rule.

1.4/ Examples:
  - ✅ see examples/good/example_good_copyright.cs
  - ✅ see examples/good/example_good_method_comment.cs
  - ❌ see examples/bad/example_bad_block_comment_copyright.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code with correctly formatted comments.
2.1/ Outcome: Files with correctly formatted copyright headers; methods documented only when necessary; no redundant or incorrect comment styles.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
