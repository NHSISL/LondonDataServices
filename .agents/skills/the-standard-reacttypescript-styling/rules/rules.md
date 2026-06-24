# The Standard React + TypeScript + Vite — Styling — Rules

## Bootstrap Utility Classes

**tsr-styles-001** [WARN] Use Bootstrap utility classes for layout and common spacing when Bootstrap is the selected UI foundation. Do not recreate spacing or layout rules in custom CSS when a Bootstrap class is available.

## No Business Rules in Class Selection

**tsr-styles-002** [ERROR] Do not encode business rules in CSS class selection. A ternary that selects a class based on a domain condition (age, status, eligibility) is a business rule and belongs in a view model field.

- Bad: `className={patient.age >= 18 ? "text-success" : "text-danger"}`
- Good: `className={patient.statusClassName}` — where `statusClassName` is a purely presentational field set by the view service based on a semantic status value.
- Better: Use a semantic display text field (`patient.statusDisplayText`) and let the component apply a static class.

## Component-Scoped Styles

**tsr-styles-003** [ERROR] Component-specific styles must be co-located with the component (CSS module or adjacent CSS file) or placed in a clearly named style module. Do not add component-specific styles to the global stylesheet.

## Global Styles Discipline

**tsr-styles-004** [ERROR] Global stylesheets must contain only genuinely global concerns: resets, typography base, CSS custom properties, theme variables. Component and layout styles must not be in the global stylesheet.

## No Unnecessary Inline Styles

**tsr-styles-005** [WARN] Do not use inline styles except for dynamic values that cannot reasonably be expressed with utility or module classes (e.g., a calculated pixel width from user input).
