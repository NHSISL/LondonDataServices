// ---
// skill: the-standard-csharp-comments
// type: example
// source-section: "1. Comments and Documentation — 12.1 Copyrights"
// demonstrates: "tsc-csharp-comments-005, tsc-csharp-comments-006 — forbidden copyright styles"
// ---

// ❌ BAD: Block comment copyright syntax — violates tsc-csharp-comments-005
/* 
 * ==============================================================
 * Copyright (c) Coalition of the Good-Hearted Engineers
 * FREE TO USE TO CONNECT THE WORLD
 * ==============================================================
 */

// ❌ BAD: XML copyright tag syntax — violates tsc-csharp-comments-006
//----------------------------------------------------------------
// <copyright file="StudentService.cs" company="OpenSource">
//      Copyright (C) Coalition of the Good-Hearted Engineers
// </copyright>
//----------------------------------------------------------------

// ❌ BAD: Redundant comment — violates tsc-csharp-comments-004
public async ValueTask<Student> AddStudentAsync(Student student)
{
    // Add the student to the database  <-- redundant: code already says this
    return await this.storageBroker.InsertStudentAsync(student);
}
