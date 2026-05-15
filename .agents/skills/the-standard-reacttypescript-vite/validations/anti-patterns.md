# The Standard React + TypeScript + Vite — Vite Configuration — Anti-Patterns

## Secret in Environment File

**Violates:** tsr-vite-003
**What happens:** `.env` contains `VITE_STRIPE_SECRET_KEY=sk_live_...`
**Why it's wrong:** Any `VITE_`-prefixed variable is bundled into the client JavaScript and visible in the browser. Secret keys exposed this way can be extracted and abused.
**Fix:** Move secrets server-side. The frontend must never hold private keys or credentials.

## Missing VITE_ Prefix

**Violates:** tsr-vite-002
**What happens:** `.env` contains `API_BASE_URL=https://api.example.com` (no `VITE_` prefix). The code references `import.meta.env.API_BASE_URL` which evaluates to `undefined` at runtime.
**Why it's wrong:** Vite does not expose non-`VITE_`-prefixed variables to the client bundle. The failure is silent.
**Fix:** Rename to `VITE_API_BASE_URL=https://api.example.com`.

## Undocumented Plugin

**Violates:** tsr-vite-005
**What happens:** `plugins: [react(), svgr(), visualizer()]` — three plugins with no comments.
**Why it's wrong:** Reviewers and future engineers cannot tell why each plugin is present, what it does, or whether it is still needed.
**Fix:** Add a comment before each plugin: `// svgr: enables SVG imports as React components`.

## Business Logic in Config

**Violates:** tsr-vite-001
**What happens:** `vite.config.ts` reads `process.env.FEATURE_FLAG_X === "true"` to conditionally include a plugin or alias.
**Why it's wrong:** Build configuration must remain infrastructure-only. Feature flag evaluation belongs in runtime application code.
**Fix:** Expose the flag as `VITE_FEATURE_FLAG_X` and read it at runtime inside the application.
