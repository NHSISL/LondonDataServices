# C# Coding Standard — Classes and Interfaces — Checklist

- [ ] tsc-csharp-classes-001: Model classes are singular with no type suffix (Student, not StudentModel).
- [ ] tsc-csharp-classes-002: Service classes are singular with Service suffix (StudentService, not StudentsService or StudentBL).
- [ ] tsc-csharp-classes-003: Broker classes are singular with Broker suffix (StudentBroker, not StudentsBroker).
- [ ] tsc-csharp-classes-004: Controller classes are plural with Controller suffix (StudentsController, not StudentController).
- [ ] tsc-csharp-classes-005: All class fields are camelCase (no underscores, no PascalCase).
- [ ] tsc-csharp-classes-006: All private field references use this. keyword.
- [ ] tsc-csharp-classes-007: Constructor calls with literal values use named aliases.
- [ ] tsc-csharp-classes-008: Object initializer property order matches class property declaration order.
- [ ] tsc-csharp-classes-009: No target-typed new expressions (Student student = new (...) is forbidden).
