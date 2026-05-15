// ---
// skill: the-standard-processings
// type: example
// source-section: "2.2 Processing Services"
// demonstrates: "ts-processings-003 — processing service bypassing foundation and calling broker directly"
// ---

// ❌ BAD: Processing service injects a broker directly and performs raw storage queries.

public class StudentProcessingService : IStudentProcessingService
{
    private readonly IStorageBroker storageBroker; // ❌ must not inject brokers

    public StudentProcessingService(IStorageBroker storageBroker)
    {
        this.storageBroker = storageBroker;
    }

    public async ValueTask<Student> UpsertStudentAsync(Student student)
    {
        // ❌ calling broker directly — bypasses foundation validation and exception handling
        Student existingStudent =
            await this.storageBroker.SelectStudentByIdAsync(student.Id);

        if (existingStudent is null)
            return await this.storageBroker.InsertStudentAsync(student);
        else
            return await this.storageBroker.UpdateStudentAsync(student);
    }
}
