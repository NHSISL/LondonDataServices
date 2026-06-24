# The Standard React + TypeScript + Vite — Error Handling — Checklist

- [ ] Brokers throw on external failure — no returning `null` or converting to UI strings (tsr-errors-001)
- [ ] Foundation services catch broker exceptions and re-throw as domain exceptions (tsr-errors-002)
- [ ] Pages render an explicit error state — not an empty render on failure (tsr-errors-004)
- [ ] No component inspects `error.status`, `error.code`, or HTTP status codes (tsr-errors-005)
- [ ] No empty `catch {}` or `catch { return null; }` blocks (tsr-errors-006)
- [ ] All suppressed errors are logged through a logging broker before suppression (tsr-errors-007)
