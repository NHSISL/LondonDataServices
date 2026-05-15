# C# Coding Standard — Classes and Interfaces — Rules

## CLASS NAMING

**tsc-csharp-classes-001** [ERROR] Model classes must be named in the singular with no type suffix (e.g., `Student`, not `Students`, `StudentModel`, `StudentObj`).
**tsc-csharp-classes-002** [ERROR] Service classes must be named in the singular with the `Service` suffix (e.g., `StudentService`, not `StudentsService`, `StudentBL`).
**tsc-csharp-classes-003** [ERROR] Broker classes must be named in the singular with the `Broker` suffix (e.g., `StudentBroker`, not `StudentsBroker`).
**tsc-csharp-classes-004** [ERROR] Controller classes must be named in the plural with the `Controller` suffix (e.g., `StudentsController`, not `StudentController`).

## FIELDS

**tsc-csharp-classes-005** [ERROR] Class fields must be named in camelCase (e.g., `studentName`, not `StudentName`, `_studentName`).
**tsc-csharp-classes-006** [ERROR] Private class fields must be referenced using the `this.` keyword to distinguish them from local variables.

## INSTANTIATION

**tsc-csharp-classes-007** [ERROR] When passing literal values to a constructor, named aliases must be used (e.g., `new Student(name: "Josh", score: 150)`, not `new Student("Josh", 150)`).
**tsc-csharp-classes-008** [ERROR] Property assignment in object initializer syntax must follow the same order as the property declarations in the class definition.
**tsc-csharp-classes-009** [ERROR] Must not use target-typed new (`Student student = new (...)`) — use `var student = new Student(...)` when the type is clear from the right side.
