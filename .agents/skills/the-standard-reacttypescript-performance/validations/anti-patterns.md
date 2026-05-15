# The Standard React + TypeScript + Vite — Performance — Anti-Patterns

## Premature Memoisation

**Violates:** tsr-performance-001
**What happens:** `const value = useMemo(() => items.filter(i => i.active), [items])` is added without any profiler evidence that filtering is a bottleneck.
**Why it's wrong:** Speculative memoisation adds complexity, can mask re-render bugs, and provides no measured benefit. The Standard requires a measured problem before applying memoisation.
**Fix:** Remove the `useMemo`. Profile first; apply only when a measurable improvement results.

## Object Created in Hot Render Path

**Violates:** tsr-performance-002
**What happens:**
```tsx
const MyComponent: React.FC = () => {
  const style = { color: 'red', fontWeight: 'bold' }; // new object every render
  return <Child style={style} />;
};
```
**Why it's wrong:** A new object reference is created on every render, causing `Child` to re-render even when its visual output would not change.
**Fix:** Move `style` outside the component, or use a CSS class.

## Monolithic Context

**Violates:** tsr-performance-004
**What happens:** A single `AppContext` holds auth state, theme, and live data. Every context update re-renders all consumers.
**Why it's wrong:** A live-data update forces auth-only consumers to re-render needlessly.
**Fix:** Split into `AuthContext`, `ThemeContext`, and `DataContext` — each updating at its own frequency.

## Unchecked Dependency Addition

**Violates:** tsr-performance-005
**What happens:** A new 200 KB library is imported and shipped without reviewing its bundle impact.
**Why it's wrong:** Bundle size grows silently, harming initial load time for all users.
**Fix:** Run `vite-bundle-visualizer` (or equivalent) before committing. Prefer tree-shakeable alternatives or import only the needed subset.
