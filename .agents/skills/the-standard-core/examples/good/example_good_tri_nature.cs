// ---
// skill: the-standard-core
// type: example
// source-section: "0.1 Purposing/Modeling/Simulation"
// demonstrates: "ts-core-002 — tri-nature structure applied to a service"
// ---

// ✅ GOOD: Single-purpose service with a clear tri-nature structure.
// The interface (Purpose) depends on a broker (Dependency) and exposes AddStudentAsync (Exposure).

public interface IStudentService
{
    ValueTask<Student> AddStudentAsync(Student student);
    ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId);
    ValueTask<Student> ModifyStudentAsync(Student student);
    ValueTask<Student> RemoveStudentByIdAsync(Guid studentId);
}

public class StudentService : IStudentService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;

    public StudentService(
        IStorageBroker storageBroker,
        ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }

    public async ValueTask<Student> AddStudentAsync(Student student) =>
        await this.storageBroker.InsertStudentAsync(student);

    public async ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
        await this.storageBroker.SelectStudentByIdAsync(studentId);

    public async ValueTask<Student> ModifyStudentAsync(Student student) =>
        await this.storageBroker.UpdateStudentAsync(student);

    public async ValueTask<Student> RemoveStudentByIdAsync(Guid studentId) =>
        await this.storageBroker.DeleteStudentByIdAsync(studentId);
}
