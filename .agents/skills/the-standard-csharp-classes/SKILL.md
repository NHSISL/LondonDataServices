---
name: the-standard-csharp-classes
version: 0.1.0
csharp-standard-version: v0.8
applies-to: ["*.cs"]
depends-on: ["the-standard-csharp-files"]
---

# C# Coding Standard — Classes and Interfaces

## 0/ Context (Purpose — WHY this skill exists)

0.0/ Where: All C# class and interface declarations.
0.1/ Who: Engineers writing or reviewing class definitions, fields, and object instantiations.
0.2/ What: Enforces class naming, field naming, field referencing, instantiation conventions, and property ordering.
0.3/ Applies to: *.cs
0.4/ Version: v0.8
0.5/ Depends on: the-standard-csharp-files

## 1/ Actual (Dependency — WHAT the rules are and what they depend on)

1.0/ Dos:
  1. Name model classes in the singular (e.g., `Student`, not `Students`) → see rules/rules.md#tsc-csharp-classes-001
  2. Name service classes in the singular with the `Service` suffix (e.g., `StudentService`) → see rules/rules.md#tsc-csharp-classes-002
  3. Name broker classes in the singular with the `Broker` suffix (e.g., `StudentBroker`) → see rules/rules.md#tsc-csharp-classes-003
  4. Name controller classes in the plural with the `Controller` suffix (e.g., `StudentsController`) → see rules/rules.md#tsc-csharp-classes-004
  5. Name class fields in camelCase (e.g., `studentName`) → see rules/rules.md#tsc-csharp-classes-005
  6. Reference class private fields using `this.` keyword → see rules/rules.md#tsc-csharp-classes-006
  7. Use named aliases when passing values to constructors where variable names differ from parameter names → see rules/rules.md#tsc-csharp-classes-007
  8. Honor property declaration order when instantiating a class with object initializer syntax → see rules/rules.md#tsc-csharp-classes-008

1.1/ Don'ts:
  1. Must not suffix model class names with `Model`, `Obj`, or `Entity` (e.g., `StudentModel` is forbidden) → see validations/anti-patterns.md#model-suffix
  2. Must not suffix service classes with `BL`, `BusinessLogic`, or `Manager` → see validations/anti-patterns.md#bl-suffix
  3. Must not prefix fields with underscores (e.g., `_studentName`) → see validations/anti-patterns.md#underscore-field
  4. Must not omit `this.` when referencing private fields → see validations/anti-patterns.md#missing-this
  5. Must not pass positional arguments without named aliases when the value is a literal → see validations/anti-patterns.md#positional-literals
  6. Must not use `{Type} variable = new (...)` target-typed new with implicit type → see validations/anti-patterns.md#target-typed-new

1.2/ Ask:
  - Ask when a class serves multiple concerns — confirm whether it should be split.

1.3/ Defaults:
  - When the right-hand side type is clear, use `var`. When it is a constructor call with initializer properties, honor property order.

1.4/ Examples:
  - ✅ see examples/good/example_good_class_naming.cs
  - ✅ see examples/good/example_good_instantiation.cs
  - ❌ see examples/bad/example_bad_class_naming.cs

## 2/ Expected (Exposure — WHAT comes out)

2.0/ Format: C# source code.
2.1/ Outcome: Classes and interfaces with correct naming, field conventions, referencing, and instantiation patterns.
2.2/ Tone: Direct. Cite rule IDs. Violations must be fixed.
