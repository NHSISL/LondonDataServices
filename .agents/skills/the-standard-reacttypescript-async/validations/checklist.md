# The Standard React + TypeScript + Vite — Async and Cancellation — Checklist

- [ ] All async page hooks handle loading, success, empty, and failure states (tsr-async-001)
- [ ] All async effects use a mounted flag or AbortController with a cleanup return (tsr-async-002)
- [ ] `Promise.all` is used only for operations that are provably independent (tsr-async-003)
- [ ] No dependent operations are run in parallel — sequential `await` is used when needed (tsr-async-004)
- [ ] No unhandled promise rejections — every async function in an effect has `try/catch` (tsr-async-005)
