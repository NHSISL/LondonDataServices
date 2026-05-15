// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "3.0 Method Signature Conventions"
// demonstrates: "tsc-csharp-cp-001, tsc-csharp-cp-002, tsc-csharp-cp-003, tsc-csharp-cp-004 — correct CancellationToken signature"
// ---

// ✅ CancellationToken is last, defaults to `default`, is not nullable,
//    and is named `cancellationToken`.

public interface IStudentService
{
    ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);

    ValueTask<IQueryable<Student>> RetrieveAllStudentsAsync(
        CancellationToken cancellationToken = default);

    ValueTask<Student> ModifyStudentAsync(
        Student student,
        CancellationToken cancellationToken = default);

    ValueTask<Student> RemoveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);
}

public class StudentService : IStudentService
{
    private readonly IStudentBroker studentBroker;

    public StudentService(IStudentBroker studentBroker) =>
        this.studentBroker = studentBroker;

    public ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            return await this.studentBroker.SelectStudentByIdAsync(
                studentId,
                cancellationToken);
        });

    public ValueTask<IQueryable<Student>> RetrieveAllStudentsAsync(
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            return await this.studentBroker.SelectAllStudentsAsync(
                cancellationToken);
        });

    public ValueTask<Student> ModifyStudentAsync(
        Student student,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            return await this.studentBroker.UpdateStudentAsync(
                student,
                cancellationToken);
        });

    public ValueTask<Student> RemoveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            return await this.studentBroker.DeleteStudentByIdAsync(
                studentId,
                cancellationToken);
        });
}
