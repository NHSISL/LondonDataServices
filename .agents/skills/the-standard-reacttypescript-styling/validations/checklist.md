# The Standard React + TypeScript + Vite — Styling — Checklist

- [ ] Bootstrap utility classes are used for layout and spacing where available (tsr-styles-001)
- [ ] No CSS class ternaries based on domain conditions (age, status, eligibility) in JSX (tsr-styles-002)
- [ ] Component-specific styles are co-located or in a named CSS module — not in the global stylesheet (tsr-styles-003)
- [ ] Global stylesheet contains only resets, typography, and CSS custom properties (tsr-styles-004)
- [ ] No inline styles except for dynamic values that cannot be expressed as classes (tsr-styles-005)
