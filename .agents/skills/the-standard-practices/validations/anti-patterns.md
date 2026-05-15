# The Standard Practices — Anti-Patterns

## Secrets in Source

**Violates:** ts-practices-007
**What happens:** `appsettings.json` or `appsettings.Development.json` contains a real database connection string or API key committed to the repository.
**Why it's wrong:** Secrets in source control are permanently accessible to anyone with repository access, including after deletion (via git history).
**Fix:** Use environment variables, Azure Key Vault, or `dotnet user-secrets` for local development. Add secret-containing files to `.gitignore`.

## Failing Build Merge

**Violates:** ts-practices-005
**What happens:** A PR is merged while the CI pipeline shows failing tests or a build error, under the assumption "I'll fix it in the next commit."
**Why it's wrong:** A failing main branch blocks all other contributors from merging and breaks the reliability guarantee of the integration branch.
**Fix:** Fix all build and test failures before requesting a review. Enable required status checks in the repository branch protection rules.

## Vague Commits

**Violates:** ts-practices-003
**What happens:** Commit messages read `fix`, `update stuff`, `wip`, or `changes`.
**Why it's wrong:** Vague messages make the git history unreadable and make debugging regressions via `git bisect` impossible.
**Fix:** Use the format `{CATEGORY}: {Imperative sentence ending with a period}.` e.g., `FOUNDATIONS: Add structural validation for AddStudentAsync.`

## Mixed PR

**Violates:** ts-practices-006
**What happens:** A single PR adds a new feature, fixes an unrelated bug, and updates the CI pipeline configuration.
**Why it's wrong:** Mixed PRs are harder to review, harder to revert, and obscure the purpose of each change in the history.
**Fix:** Open separate PRs: one for the feature, one for the bug fix, one for the CI configuration change.
