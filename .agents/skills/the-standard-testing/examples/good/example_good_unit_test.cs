// ---
// skill: the-standard-testing
// type: example
// source-section: "2.1.3.0.1 Implementation"
// demonstrates: "ts-testing-003, ts-testing-004, ts-testing-005, ts-testing-006, ts-testing-007"
// ---

// ✅ GOOD: Named correctly, Given/When/Then, randomized inputs, DeepClone, full mock verification.

[Fact]
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
    this.dateTimeBrokerMock.VerifyNoOtherCalls();
}

// Helper (in test support class):
private static Student CreateRandomStudent() =>
    new Filler<Student>().Create();
