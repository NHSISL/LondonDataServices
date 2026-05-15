# The Standard UI — Rules

## SEPARATION OF CONCERNS

**ts-ui-001** [ERROR] Component C# logic must be placed in a `ComponentBase` class; `.razor` files must contain markup and minimal binding expressions only.
**ts-ui-002** [ERROR] Component base class names must use the `Base` suffix (e.g., `StudentComponentBase`).
**ts-ui-003** [ERROR] Each component must govern exactly one UI concern; split components with multiple concerns.

## DEPENDENCIES

**ts-ui-004** [ERROR] Component base classes must call only UI broker or service interfaces; direct broker access is forbidden.

## ORGANIZATION

**ts-ui-005** [ERROR] Pages must be organized under a `Pages/` folder; reusable components must be organized under a `Components/` folder.

## UNOBTRUSIVENESS

**ts-ui-006** [ERROR] Components must not contain inline styles; all styling must be in external CSS or CSS isolation files.
**ts-ui-007** [WARN]  Components must not contain hard-coded string literals; extract to constants or resource files.
