// ---
// skill: the-standard-csharp-directives
// type: example
// source-section: "3. Directives — ordering, grouping, hygiene"
// demonstrates: "tsc-csharp-directives-002, tsc-csharp-directives-003, tsc-csharp-directives-006, tsc-csharp-directives-007, tsc-csharp-directives-008, tsc-csharp-directives-009"
// ---

// ❌ BAD: Multiple violations.

namespace MyProject.Services.Students
{
    using System; // ❌ inside namespace block — violates tsc-csharp-directives-007
    using Newtonsoft.Json; // ❌ inside namespace block — violates tsc-csharp-directives-007
    using System.Text; // ❌ unused — violates tsc-csharp-directives-006
    using static System.Math; // ❌ using static — violates tsc-csharp-directives-009
    using S = System.Collections.Generic; // ❌ alias for brevity — violates tsc-csharp-directives-008

    // ❌ Mixed groups with no separator — violates tsc-csharp-directives-002 / 003:
    // using System.Linq;
    // using Newtonsoft.Json;   // third-party mixed in with System

    public class StudentService : IStudentService { }
}
