# The Standard React + TypeScript + Vite — Accessibility — Checklist

- [ ] All interactive elements use semantic HTML (`<button>`, `<a>`, `<input>`, `<select>`) (tsr-accessibility-001)
- [ ] No `<div onClick>` or `<span onClick>` substituting for buttons without ARIA (tsr-accessibility-001)
- [ ] Every form input has a `<label htmlFor>` or `aria-label` (tsr-accessibility-002)
- [ ] All `<img>` elements have an `alt` attribute — descriptive or empty string (tsr-accessibility-003)
- [ ] All interactive elements are keyboard-reachable — no removed `tabIndex` or `outline` without replacement (tsr-accessibility-004)
- [ ] Error messages are rendered as visible text and associated with the relevant control via `aria-describedby` (tsr-accessibility-005)
