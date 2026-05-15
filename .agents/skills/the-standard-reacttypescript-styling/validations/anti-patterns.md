# The Standard React + TypeScript + Vite — Styling — Anti-Patterns

## Business Rule in Class

**Violates:** tsr-styles-002
**What happens:** `className={patient.age >= 18 ? "text-success" : "text-danger"}` — an age threshold drives class selection.
**Why it's wrong:** Age >= 18 is a domain rule. It is duplicated every time a component renders a patient status, making it untestable and easy to miss when the threshold changes.
**Fix:** Add `statusClassName: "text-success"` to the view model (where the domain rule is evaluated once in the view service), and render `className={patient.statusClassName}`. Or better: use a semantic text field (`statusDisplayText`) and apply a static class around it.

## Global Style Pollution

**Violates:** tsr-styles-003, tsr-styles-004
**What happens:** `.patient-card { border-radius: 8px; }` is added to `global.css`.
**Why it's wrong:** Component-specific rules in the global stylesheet grow uncontrolled. Changes risk unintended side effects on other components.
**Fix:** Move the rule to `PatientCard.module.css` and apply it with `styles.patientCard`.

## Unnecessary Inline Style

**Violates:** tsr-styles-005
**What happens:** `<div style={{ marginTop: "16px", color: "red" }}>Error</div>` — static values set inline.
**Why it's wrong:** Inline styles override CSS modules and utility classes, are hard to override consistently, and clutter JSX.
**Fix:** Use a Bootstrap class (`mt-3 text-danger`) or a CSS module rule.
