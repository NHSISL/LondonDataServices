---
applyTo: "**/*.razor,**/*Component*.cs,**/*Base*.cs,**/*Page*.cs"
---

# The Standard — User Interfaces (Blazor)

## Applies To
The UI layer — `*.razor`, `*Component*.cs`, `*Base*.cs`, `*Page*.cs`.

## Rules — Do
- Separate component logic from markup: put C# code in a `ComponentBase` class, markup in `.razor` (ts-ui-001)
- Name base classes with the `Base` suffix: `StudentComponentBase` (ts-ui-002)
- Keep components single-responsibility — one component governs one UI concern (ts-ui-003)
- Call only UI broker or service interfaces from the component base; never call brokers directly (ts-ui-004)
- Organize pages in a `Pages/` folder and components in a `Components/` folder (ts-ui-005)
- Apply the Unobtrusiveness principle: no inline styles, no hard-coded magic strings (ts-ui-006)

## Rules — Do Not
- Must not put business logic or service calls directly in `.razor` markup files (ts-ui-001)
- Must not call brokers directly from a component base (ts-ui-002)
- Must not mix page and component responsibilities in a single `.razor` file (ts-ui-003)

## Defaults
- When a Blazor component has any C# code beyond property bindings, create a `ComponentBase` class.
- Pages go under `Pages/`, reusable components go under `Components/`.
