# C# Coding Standard — Directives — Anti-Patterns

## Unused Using

**Violates:** tsc-csharp-directives-006
**What happens:** A `using System.Text;` directive is present but `System.Text` types are never referenced in the file.
**Why it's wrong:** Unused directives add noise, slow down compilation, and obscure true dependencies.
**Fix:** Remove any directive whose namespace is not directly referenced in the file.

## Mixed Groups

**Violates:** tsc-csharp-directives-002, tsc-csharp-directives-003
**What happens:** `System.Linq` and `Newtonsoft.Json` appear in the same block with no blank-line separator between System and third-party groups.
**Why it's wrong:** Without visual separation, it is impossible to scan which dependencies are framework vs. external.
**Fix:** Place each category in its own block, separated by exactly one blank line.

## Using Inside Namespace

**Violates:** tsc-csharp-directives-007
**What happens:** `using System;` is placed inside `namespace MyProject { ... }`.
**Why it's wrong:** Directives inside a namespace affect scoping in subtle ways and are inconsistent with the file-level placement rule.
**Fix:** Move all `using` directives to the top of the file, above the namespace declaration.

## Unnecessary Alias

**Violates:** tsc-csharp-directives-008
**What happens:** `using S = System;` is written purely to save typing.
**Why it's wrong:** Aliases for brevity make code harder to read and search. They are only permitted to resolve genuine type-name ambiguities.
**Fix:** Use the full namespace. Remove the alias unless two types from different namespaces share the same name.

## Using Static

**Violates:** tsc-csharp-directives-009
**What happens:** `using static System.Math;` is added so that `Abs(x)` can be written instead of `Math.Abs(x)`.
**Why it's wrong:** `using static` hides where a symbol comes from, reducing readability and discoverability.
**Fix:** Remove the `using static` directive and qualify the call with its class name.
