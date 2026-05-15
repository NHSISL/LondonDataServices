---
applyTo: "vite.config.*,.env*"
---

# The Standard React + TypeScript + Vite — Vite Configuration

## Applies To
All Vite configuration files and environment variable files: `vite.config.*`, `.env*`.

## Rules — Do
- Use `VITE_` prefix for all client-side environment variables (tsr-vite-001)
- Configure base URL, aliases, and proxy only through `vite.config.ts` (tsr-vite-002)
- Use path aliases for `src/` to avoid deep relative imports (tsr-vite-003)

## Rules — Do Not
- Never commit secrets or production keys in `.env` files (tsr-vite-004)
- Never access `process.env` in client code — use `import.meta.env` (tsr-vite-005)
- Never manually bypass Vite HMR without documenting the reason (tsr-vite-006)

## Defaults
- `VITE_API_URL` for API base URL.
- `@/` alias resolves to `src/`.
- `.env.local` is gitignored; `.env.example` committed with placeholder values.
