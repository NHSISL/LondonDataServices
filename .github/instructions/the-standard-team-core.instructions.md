---
applyTo: "**/*.md"
---

# The Standard Team — Core Principles

## Applies To
All documentation and markdown files in repositories following The Standard.

## Rules — Do
- Every service must serve one and only one purpose (tst-core-001)
- Naming must describe what something is, not how it works (tst-core-002)
- Every layer must depend only on the layer directly below it (tst-core-003)
- All code must be testable without modification (tst-core-004)
- Every public API must be documented inline (tst-core-005)
- Expose only what callers need — hide implementation details (tst-core-006)
- Every business rule must live in a service, not in a controller or broker (tst-core-007)
- Code must communicate intent clearly without requiring comments (tst-core-009)
- Favour small, composable units over large monolithic implementations (tst-core-010)
- Separate what changes frequently from what stays stable (tst-core-011)

## Rules — Do Not
- Never let a layer skip a level and call two layers below it (tst-core-003)
- Never mix infrastructure and business concerns in the same class (tst-core-008)

## Defaults
- If the name of a method or variable needs a comment to explain it, rename it first.
