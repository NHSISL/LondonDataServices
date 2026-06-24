# The Standard React + TypeScript + Vite — Accessibility — Anti-Patterns

## Div as Button

**Violates:** tsr-accessibility-001
**What happens:** `<div onClick={handleSave}>Save</div>` is used as an action trigger.
**Why it's wrong:** A `<div>` has no implicit ARIA role, is not keyboard focusable by default, and does not fire on `Enter` or `Space`. Screen reader users cannot discover or activate it.
**Fix:** `<button type="button" onClick={handleSave}>Save</button>`

## Missing Alt Text

**Violates:** tsr-accessibility-003
**What happens:** `<img src="/logo.png" />` — no `alt` attribute.
**Why it's wrong:** Screen readers announce the file name or skip the image unpredictably. Failing to provide `alt` is an accessibility error.
**Fix:** Informative: `<img src="/logo.png" alt="Company logo" />`. Decorative: `<img src="/divider.png" alt="" />`.

## Unlabeled Input

**Violates:** tsr-accessibility-002
**What happens:** `<input type="text" placeholder="Search..." />` — no label, only a placeholder.
**Why it's wrong:** Placeholders disappear when the user types and are not read consistently by screen readers. The input has no programmatic label.
**Fix:** `<label htmlFor="search">Search</label><input id="search" type="text" />`

## Inaccessible Error

**Violates:** tsr-accessibility-005
**What happens:** A validation error is shown as a tooltip that only appears on hover, with no visible text near the input.
**Why it's wrong:** Keyboard users and screen reader users may never see or hear the error.
**Fix:** Render the error as visible text below the input and link it: `<input aria-describedby="nameError" /><p id="nameError">Name is required.</p>`
