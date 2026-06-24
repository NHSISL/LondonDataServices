// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "6.1 The same token MUST flow through all layers / 6.4 Methods using CancellationToken MUST validate cancellation"
// demonstrates: "tsc-csharp-cp-007, tsc-csharp-cp-017 — token flowing through all layers; ThrowIfCancellationRequested at foundation service"
// ---

// ✅ The same CancellationToken flows from the controller down through every layer
//    to the storage broker without being dropped or replaced.
//    ThrowIfCancellationRequested() is called at the foundation service before the broker call.

// --- Controller (ASP.NET Core) ---
[HttpGet("{studentId}")]
public async ValueTask<ActionResult<Student>> GetStudentByIdAsync(
    Guid studentId,
    CancellationToken cancellationToken)
{
    Student student =
        await this.studentService.RetrieveStudentByIdAsync(
            studentId,
            cancellationToken);

    return Ok(student);
}

// --- Foundation Service ---
public partial class StudentService : IStudentService
{
    public ValueTask<Student> RetrieveStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default) =>
        TryCatch(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await this.studentBroker.SelectStudentByIdAsync(
                studentId,
                cancellationToken);
        });
}

// --- Storage Broker ---
public partial class StorageBroker : IStorageBroker
{
    public async ValueTask<Student> SelectStudentByIdAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        return await this.students.FindAsync(
            new object[] { studentId },
            cancellationToken);
    }
}
