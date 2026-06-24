// ---
// skill: the-standard-orchestrations
// type: example
// source-section: "2.3 Orchestration Services"
// demonstrates: "ts-orchestrations-003 — orchestration service injecting and calling broker directly"
// ---

// ❌ BAD: Orchestration service bypasses foundation by calling the storage broker directly.

public class StudentLibraryOrchestrationService : IStudentLibraryOrchestrationService
{
    private readonly IStorageBroker storageBroker; // ❌ broker must not be injected here
    private readonly ILibraryCardService libraryCardService;

    public async ValueTask<Student> RegisterStudentWithLibraryCardAsync(Student student)
    {
        // ❌ direct broker call — bypasses foundation validation and exception handling
        Student savedStudent = await this.storageBroker.InsertStudentAsync(student);

        await this.libraryCardService.AddLibraryCardAsync(
            new LibraryCard { StudentId = savedStudent.Id });

        return savedStudent;
    }
}
