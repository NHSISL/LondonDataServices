# The Standard React + TypeScript + Vite — Accessibility — Rules

## Semantic HTML

**tsr-accessibility-001** [ERROR] Interactive UI must use semantic HTML elements. Use `<button>` for actions, `<a>` for navigation links, `<input>` for form inputs, `<select>` for dropdowns, and `<textarea>` for multi-line text. Non-semantic elements (`<div>`, `<span>`) must not substitute for interactive elements without explicit ARIA roles and keyboard handling.

## Form Labels

**tsr-accessibility-002** [ERROR] Every form input must have an accessible label. Use `<label htmlFor="inputId">` paired with the input's `id`, or `aria-label` when a visible label is not appropriate. An unlabeled input is inaccessible to screen readers.

## Image Alt Text

**tsr-accessibility-003** [ERROR] All `<img>` elements must have an `alt` attribute. Informative images must have descriptive text. Decorative images must have `alt=""` to signal they are presentational and should be skipped by assistive technology.

## Keyboard Navigation

**tsr-accessibility-004** [ERROR] All interactive elements must be reachable and operable by keyboard. Do not remove `tabIndex` or `outline` styles without providing an equivalent visible focus indicator. Custom interactive components must handle `Enter` and `Space` key events where applicable.

## Accessible Error Messages

**tsr-accessibility-005** [ERROR] Error messages must be rendered in visible text and associated with the relevant form control where practical. Use `aria-describedby` to link error text to an input, or render the error adjacent to the control with a clear visual and programmatic association.
