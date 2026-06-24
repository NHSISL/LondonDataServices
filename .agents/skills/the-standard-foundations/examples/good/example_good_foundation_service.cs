// ---
// skill: the-standard-foundations
// type: example
// source-section: "2.1.3.0.1 Implementation"
// demonstrates: "ts-foundations-001, ts-foundations-007, ts-foundations-008 — happy path test + implementation"
// ---

// ✅ GOOD: TDD happy-path test with randomized inputs, DeepClone, and correct business language.

// Test:
private async Task ShouldAddStudentAsync()
{
    // given
    Student randomStudent = CreateRandomStudent();
    Student inputStudent = randomStudent;
    Student storageStudent = inputStudent;
    Student expectedStudent = storageStudent.DeepClone();

    this.storageBrokerMock.Setup(broker =>
        broker.InsertStudentAsync(inputStudent))
            .ReturnsAsync(storageStudent);

    // when
    Student actualStudent =
        await this.studentService.AddStudentAsync(inputStudent);

    // then
    actualStudent.Should().BeEquivalentTo(expectedStudent);

    this.storageBrokerMock.Verify(broker =>
        broker.InsertStudentAsync(inputStudent),
            Times.Once);

    this.storageBrokerMock.VerifyNoOtherCalls();
    this.loggingBrokerMock.VerifyNoOtherCalls();
}

// Implementation:
public async ValueTask<Student> AddStudentAsync(Student student)
{
    ValidateStudentOnAdd(student);

    return await this.storageBroker.InsertStudentAsync(student);
}
