---
name: the-standard-practices
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*"]
depends-on: ["the-standard-core"]
---

# The Standard — Practices

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All Standard-compliant projects — source control, CI/CD, and configuration.
0.1/ Who: Engineers contributing to any Standard-compliant project.
0.2/ What: Enforces branching strategy, commit conventions, pull request standards, and configuration management.
0.3/ Applies to: * (all files)
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Fork the target repository before contributing; never push feature branches to upstream → see rules/rules.md#ts-practices-001
  2. Name branches using lowercase-kebab-case with a type prefix: `users/{handle}/{type}/{short-description}` → see rules/rules.md#ts-practices-002
  3. Write commit messages in the imperative mood with a category prefix in ALL CAPS → see rules/rules.md#ts-practices-003
  4. Keep pull requests focused on a single concern; one PR per feature/fix → see rules/rules.md#ts-practices-004
  5. Include a passing build and all tests green before requesting a PR review → see rules/rules.md#ts-practices-005
  6. Store all secrets and environment-specific values in environment variables or secret stores, never in source code → see rules/rules.md#ts-practices-006
  7. Use ADotNet or an equivalent YAML-based CI pipeline configuration → see rules/rules.md#ts-practices-007

1.1/ Don'ts:
  1. Must not commit secrets, connection strings, or API keys to source control → see validations/anti-patterns.md#secrets-in-source
  2. Must not merge a PR with failing tests or a failing build → see validations/anti-patterns.md#failing-build-merge
  3. Must not use vague commit messages (e.g., "fix", "update", "wip") → see validations/anti-patterns.md#vague-commits
  4. Must not submit PRs that mix multiple unrelated concerns → see validations/anti-patterns.md#mixed-pr

1.2/ Ask:
  - Ask when a change touches shared infrastructure — confirm it requires a separate PR from feature work.

1.3/ Defaults:
  - Default branch name format: `users/{github-handle}/{type}/{short-description}`
  - Default commit format: `{CATEGORY}: {imperative sentence}.`
  - Default PR target: `main` (or the project's primary integration branch).

1.4/ Examples:
  - ✅ see examples/good/example_good_branch_name.md
  - ✅ see examples/good/example_good_commit_message.md
  - ❌ see examples/bad/example_bad_commit_message.md

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Branch names, commit messages, PR descriptions, YAML pipeline files.
2.1/ Outcome: Consistent, traceable contribution history with clean CI pipelines and no secrets in source control.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
