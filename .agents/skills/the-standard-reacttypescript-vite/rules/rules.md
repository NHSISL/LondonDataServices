# The Standard React + TypeScript + Vite — Vite Configuration — Rules

## No Business Logic

**tsr-vite-001** [ERROR] Vite configuration must not contain business logic. Conditional configuration based on domain concepts (e.g., feature flags evaluated in the build file) must be externalized to environment variables or separate configuration files.

## Public Environment Convention

**tsr-vite-002** [ERROR] All environment variables exposed to client-side code must use the `VITE_` prefix convention. Variables without this prefix are not accessible in the browser bundle and will produce silent failures.

## No Secrets

**tsr-vite-003** [ERROR] Secrets (API keys, private tokens, database credentials, signing keys) must never be placed in frontend environment variable files. Even `VITE_`-prefixed variables are bundled into the client.

## Consistent Path Aliases

**tsr-vite-004** [WARN] Path aliases must be used sparingly and consistently. Recommended aliases:

| Alias | Target |
|---|---|
| `@` | `./src` |
| `@brokers` | `./src/brokers` |
| `@services` | `./src/services` |
| `@models` | `./src/models` |
| `@components` | `./src/components` |
| `@pages` | `./src/pages` |

Do not create aliases for every subfolder.

## Documented Plugins

**tsr-vite-005** [ERROR] Every Vite plugin must have a comment explaining its purpose before the plugin entry in the `plugins` array.

## Build Warnings as Signals

**tsr-vite-006** [WARN] Build warnings must not be ignored. They must be triaged in code review — either resolved or documented with a justification comment.
