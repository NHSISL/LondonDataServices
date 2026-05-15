---
name: the-standard-reacttypescript-vite
version: 0.1.0
standard-version: v0.1.0
applies-to: ["vite.config.*", ".env*"]
depends-on: ["the-standard-reacttypescript-files"]
---

# The Standard React + TypeScript + Vite — Vite Configuration

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: `vite.config.ts`, `vite.config.js`, and all `.env*` files.
0.1/ Who: Any engineer configuring or reviewing Vite build configuration in a Standard frontend project.
0.2/ What: Governs that Vite configuration remains infrastructure-only, environment variables use the correct convention, secrets are never exposed, path aliases are consistent, plugins are justified, and build warnings are treated as signals.
0.3/ Applies to: `vite.config.*`, `.env*`
0.4/ Version: The Standard React + TypeScript + Vite v0.1.0
0.5/ Depends on: the-standard-reacttypescript-files

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Use Vite public environment convention (`VITE_*`) for all client-exposed variables — see rules/rules.md#tsr-vite-002
  2. Use path aliases sparingly and consistently from the recommended set — see rules/rules.md#tsr-vite-004
  3. Document every Vite plugin by purpose — see rules/rules.md#tsr-vite-005
  4. Treat build warnings as review signals — see rules/rules.md#tsr-vite-006

1.1/ Don'ts:
  1. Never put business logic in Vite configuration — see rules/rules.md#tsr-vite-001
  2. Never place secrets in frontend environment variables — see rules/rules.md#tsr-vite-003

1.2/ Ask:
  - Ask when a new Vite plugin is proposed — confirm its purpose before adding.

1.3/ Defaults:
  - Recommended path aliases: `@` → `./src`, `@brokers` → `./src/brokers`, `@services` → `./src/services`, `@models` → `./src/models`, `@components` → `./src/components`, `@pages` → `./src/pages`.
  - `.env.local` is gitignored; `.env` is committed without secrets.

1.4/ Examples:
  - ✅ see examples/good/example_good_vite_config.ts
  - ❌ see examples/bad/example_bad_vite_config.ts

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: Minimal `vite.config.ts` with documented plugins, consistent aliases, and no business logic or secrets.
2.1/ Outcome: Vite configuration is infrastructure-only, readable, and environment-safe.
2.2/ Tone: Direct. Cite rule IDs (e.g., tsr-vite-003). No prose justification unless asked.
