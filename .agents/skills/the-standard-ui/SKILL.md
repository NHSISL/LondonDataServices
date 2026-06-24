---
name: the-standard-ui
version: 0.1.0
standard-version: v2.50.0
applies-to: ["*.razor", "*Component*.cs", "*Base*.cs", "*Page*.cs"]
depends-on: ["the-standard-core", "the-standard-foundations"]
---

# The Standard — User Interfaces

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: The UI layer — Blazor components, pages, and base classes.
0.1/ Who: Engineers implementing or reviewing Blazor web application components.
0.2/ What: Enforces UI component design: single-responsibility components, base/component separation, unobtrusiveness, and organization.
0.3/ Applies to: *.razor, *Component*.cs, *Base*.cs, *Page*.cs
0.4/ Version: v2.50.0
0.5/ Depends on: the-standard-core, the-standard-foundations

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Separate component logic from markup: put C# code in a `ComponentBase` class, markup in `.razor` → see rules/rules.md#ts-ui-001
  2. Name base classes with the `Base` suffix (e.g., `StudentComponentBase`) → see rules/rules.md#ts-ui-002
  3. Keep components single-responsibility — one component governs one UI concern → see rules/rules.md#ts-ui-003
  4. Call only UI broker or service interfaces from the component base; never call brokers directly → see rules/rules.md#ts-ui-004
  5. Organize pages in a `Pages/` folder and components in a `Components/` folder → see rules/rules.md#ts-ui-005
  6. Apply the Unobtrusiveness principle: no inline styles, no hard-coded magic strings → see rules/rules.md#ts-ui-006

1.1/ Don'ts:
  1. Must not put business logic or service calls directly in `.razor` markup files → see validations/anti-patterns.md#logic-in-razor
  2. Must not call brokers directly from a component base → see validations/anti-patterns.md#component-broker-access
  3. Must not mix page and component responsibilities in a single `.razor` file → see validations/anti-patterns.md#mixed-page-component

1.2/ Ask:
  - Ask when a component needs data from multiple services — confirm whether a view-model aggregation layer is needed.

1.3/ Defaults:
  - When a Blazor component has any C# code beyond property bindings, create a `ComponentBase` class.
  - When organizing files, pages go under `Pages/`, reusable components under `Components/`.

1.4/ Examples:
  - ✅ see examples/good/example_good_component_base.cs
  - ✅ see examples/good/example_good_component.razor
  - ❌ see examples/bad/example_bad_logic_in_razor.razor

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# and Razor source files.
2.1/ Outcome: UI components that are thin, organized, and free of business logic, with markup separated from code-behind.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
