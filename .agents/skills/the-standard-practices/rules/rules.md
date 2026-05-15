# The Standard Practices — Rules

## BRANCHING

**ts-practices-001** [ERROR] All contributions must originate from a fork of the target repository; feature branches must not be pushed directly to upstream.
**ts-practices-002** [ERROR] Branch names must follow the pattern: `users/{github-handle}/{type}/{short-description}` using lowercase-kebab-case.

Branch type values:

| Type | Usage |
|------|-------|
| `features` | New functionality |
| `bugs` | Bug fixes |
| `configurations` | CI/CD, project config, infrastructure |
| `documentation` | Docs-only changes |

## COMMITS

**ts-practices-003** [ERROR] Commit messages must start with an ALL-CAPS category prefix followed by a colon and an imperative-mood sentence ending with a period. Format: `{CATEGORY}: {Sentence}.`
**ts-practices-004** [WARN]  Commits must be atomic — one logical change per commit.

## PULL REQUESTS

**ts-practices-005** [ERROR] A PR must not be submitted for review unless the build passes and all tests are green.
**ts-practices-006** [ERROR] Each PR must address exactly one concern; mixing features, fixes, and configuration changes in one PR is forbidden.

## CONFIGURATION

**ts-practices-007** [ERROR] Secrets, connection strings, and API keys must never be committed to source control; use environment variables or a secret store.
**ts-practices-008** [ERROR] CI/CD pipelines must be defined in YAML and committed to source control.
