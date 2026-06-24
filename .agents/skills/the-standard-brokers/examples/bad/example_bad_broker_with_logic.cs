// ---
// skill: the-standard-brokers
// type: example
// source-section: "1.2 Brokers — Design"
// demonstrates: "ts-brokers-001 — broker containing business logic (validation and conditional routing)"
// ---

// ❌ BAD: Broker performs validation (business logic) and calls another broker.

public class StorageBroker : IStorageBroker
{
    private readonly AppDbContext dbContext;
    private readonly ILoggingBroker loggingBroker; // ❌ broker calling broker

    public async ValueTask<Student> InsertStudentAsync(Student student)
    {
        // ❌ validation belongs in the service layer
        if (student is null)
            throw new ArgumentNullException(nameof(student));

        if (student.Id == Guid.Empty)
            throw new ArgumentException("Student Id must not be empty.");

        // ❌ logging in a broker
        await this.loggingBroker.LogInformationAsync($"Inserting student {student.Id}");

        this.dbContext.Students.Add(student);
        await this.dbContext.SaveChangesAsync();

        return student;
    }
}
