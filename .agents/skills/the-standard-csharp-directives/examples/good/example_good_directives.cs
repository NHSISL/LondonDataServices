// ---
// skill: the-standard-csharp-directives
// type: example
// source-section: "3. Directives — ordering, grouping, hygiene"
// demonstrates: "tsc-csharp-directives-001, tsc-csharp-directives-002, tsc-csharp-directives-003, tsc-csharp-directives-004, tsc-csharp-directives-005"
// ---

// ✅ GOOD: Three groups, one blank line between groups, alphabetically sorted within each group.

// Group 1 — System namespaces (alphabetical)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Group 2 — Third-party namespaces (alphabetical)
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// Group 3 — Internal / project namespaces (alphabetical)
using MyProject.Brokers.Storages;
using MyProject.Models.Students;
using MyProject.Services.Students;

namespace MyProject.Services.Students
{
    public class StudentService : IStudentService
    {
        // implementation
    }
}
