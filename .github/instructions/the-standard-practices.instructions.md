---
applyTo: "**"
---

# The Standard — Practices

## Applies To
All files in Standard-compliant projects — source control, CI/CD, and configuration.

## Rules — Do
- Fork the target repository before contributing; never push feature branches to upstream (ts-practices-001)
- Name branches using the pattern `users/{handle}/{type}/{short-description}` (ts-practices-002)
- Write commit messages in the imperative mood with a category prefix in ALL CAPS (ts-practices-003)
- Keep pull requests focused on a single concern — one PR per feature/fix (ts-practices-004)
- Include a passing build and all tests green before requesting a PR review (ts-practices-005)
- Store all secrets and environment-specific values in environment variables or secret stores, never in source code (ts-practices-006)
- Use ADotNet or an equivalent YAML-based CI pipeline configuration (ts-practices-007)

## Rules — Do Not
- Must not commit secrets, connection strings, or API keys to source control (ts-practices-001)
- Must not merge a PR with failing tests or a failing build (ts-practices-002)
- Must not use vague commit messages such as "fix", "update", or "wip" (ts-practices-003)
- Must not submit PRs that mix multiple unrelated concerns (ts-practices-004)

## Defaults
- Default branch name format: `users/{github-handle}/{type}/{short-description}`
- Default commit format: `{CATEGORY}: {imperative sentence}.`
- Default PR target: `main` or the project's primary integration branch.
