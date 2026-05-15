// ---
// skill: the-standard-core
// type: example
// source-section: "0.2 Principles"
// demonstrates: "ts-core-001, ts-core-006 — God service violating single responsibility and purity"
// ---

// ❌ BAD: God service that mixes foundation CRUD, business workflows, and notification sending.
// Violates ts-core-001 (single responsibility) and ts-core-006 (purity principle).

public class StudentService : IStudentService
{
    private readonly IStorageBroker storageBroker;
    private readonly SmtpClient smtpClient; // ❌ infrastructure in a service

    public async ValueTask<Student> AddStudentAsync(Student student)
    {
        // ❌ business logic mixed with notification sending
        Student addedStudent = await this.storageBroker.InsertStudentAsync(student);

        // ❌ sends email directly — should be a broker
        await this.smtpClient.SendMailAsync(
            new MailMessage("admin@school.com", student.Email, "Welcome!", "You are enrolled."));

        // ❌ upsert logic here instead of a ProcessingService
        if (student.IsTransfer)
            await this.storageBroker.UpdateTransferRecordAsync(student);

        return addedStudent;
    }
}
