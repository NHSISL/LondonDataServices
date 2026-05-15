// ---
// skill: the-standard-csharp-comments
// type: example
// source-section: "1. Comments and Documentation — 12.1 Copyrights"
// demonstrates: "tsc-csharp-comments-002 — correct copyright block format"
// ---

// ✅ GOOD: Correct dashed-line // copyright format.

// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;

public class StudentService : IStudentService
{
    public async ValueTask<Student> AddStudentAsync(Student student) =>
        await this.storageBroker.InsertStudentAsync(student);
}
