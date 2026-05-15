# The Standard React + TypeScript + Vite — Vite Configuration — Checklist

- [ ] No business logic or domain conditionals in `vite.config.ts` (tsr-vite-001)
- [ ] All client-facing environment variables use `VITE_` prefix (tsr-vite-002)
- [ ] No API keys, tokens, or secrets in any `.env*` file (tsr-vite-003)
- [ ] Path aliases are limited to the recommended set — no per-subfolder aliases (tsr-vite-004)
- [ ] Every plugin entry has a comment explaining its purpose (tsr-vite-005)
- [ ] Build output has been reviewed for warnings — none are silently ignored (tsr-vite-006)
