// ---
// skill: the-standard-foundations
// type: example
// source-section: "2.1.3 Responsibilities"
// demonstrates: "ts-foundations-003, ts-foundations-006 — missing validation and naked broker exceptions"
// ---

// ❌ BAD: No structural validation; broker exception propagates unwrapped.

public async ValueTask<Student> AddStudentAsync(Student student)
{
    // ❌ No null check — violates ts-foundations-003
    // ❌ No Id/Name validation — violates ts-foundations-003

    // ❌ SqlException from broker will propagate naked to controller — violates ts-foundations-006
    return await this.storageBroker.InsertStudentAsync(student);
}
