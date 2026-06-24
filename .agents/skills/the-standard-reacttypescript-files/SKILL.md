---
name: the-standard-reacttypescript-files
version: 0.1.0
standard-version: v0.1.0
applies-to: ["src/**/*"]
depends-on: []
---

# The Standard React + TypeScript + Vite — Files

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All source files in a Standard-compliant React + TypeScript + Vite project (`src/**/*`).
0.1/ Who: Any engineer creating, naming, or reviewing source files in a Standard frontend project.
0.2/ What: Governs file naming conventions, architectural role naming patterns, extension rules, and prohibition of generic utility files.
0.3/ Applies to: `src/**/*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: none

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Each file must contain one primary responsibility — see rules/rules.md#tsr-files-001
  2. File names must describe architectural role using the defined naming patterns — see rules/rules.md#tsr-files-002
  3. React component files must use `.tsx` extension — see rules/rules.md#tsr-files-003
  4. Non-rendering TypeScript files must use `.ts` extension — see rules/rules.md#tsr-files-004
  5. Organize files into the standard project structure by architectural layer — see contracts/contracts.json

1.1/ Don'ts:
  1. Never create generic `utils`, `helpers`, or `common` files — see rules/rules.md#tsr-files-005 and validations/anti-patterns.md#generic-files
  2. Never mix multiple architectural responsibilities in one file — see validations/anti-patterns.md#mixed-responsibility
  3. Never use `.tsx` for non-rendering files — see validations/anti-patterns.md#wrong-extension

1.2/ Ask:
  - Ask when it is unclear which architectural layer a new file belongs to.

1.3/ Defaults:
  - When a file exports one class or function representing an architectural role, its name follows the role pattern in `contracts/contracts.json`.
  - When a new domain concept is introduced, create a dedicated folder under the appropriate layer.

1.4/ Examples:
  - ✅ see examples/good/example_good_file_naming.md
  - ❌ see examples/bad/example_bad_file_naming.md

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Correct file names and project structure layout.
2.1/ Outcome: Every file has a name and extension that unambiguously communicates its architectural role.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-files-001). No prose justification unless asked.
