---
applyTo: "**/*.cs"
---

# C# Coding Standard — Classes and Interfaces

## Applies To
All C# class and interface declarations.

## Rules — Do
- Name model classes in the singular: `Student`, not `Students` (tsc-csharp-classes-001)
- Name service classes in the singular with the `Service` suffix: `StudentService` (tsc-csharp-classes-002)
- Name broker classes in the singular with the `Broker` suffix: `StudentBroker` (tsc-csharp-classes-003)
- Name controller classes in the plural with the `Controller` suffix: `StudentsController` (tsc-csharp-classes-004)
- Name class private fields in camelCase: `studentName` (tsc-csharp-classes-005)
- Reference class private fields using `this.` keyword (tsc-csharp-classes-006)
- Use named aliases when passing values to constructors where variable names differ from parameter names (tsc-csharp-classes-007)
- Honor property declaration order when instantiating a class with object initializer syntax (tsc-csharp-classes-008)

## Rules — Do Not
- Must not suffix model class names with `Model`, `Obj`, or `Entity` — `StudentModel` is forbidden (tsc-csharp-classes-001)
- Must not suffix service classes with `BL`, `BusinessLogic`, or `Manager` (tsc-csharp-classes-002)
- Must not prefix fields with underscores — `_studentName` is forbidden (tsc-csharp-classes-003)
- Must not omit `this.` when referencing private fields (tsc-csharp-classes-004)
- Must not pass positional arguments without named aliases when the value is a literal (tsc-csharp-classes-005)
- Must not use target-typed new with implicit type: `{Type} variable = new (...)` (tsc-csharp-classes-006)

## Defaults
- When the right-hand side type is clear, use `var`.
- When it is a constructor call with initializer properties, honor property declaration order.
