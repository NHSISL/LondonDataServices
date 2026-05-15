// ---
// skill: the-standard-testing
// type: example
// source-section: "2.1.3.0.1 Implementation"
// demonstrates: "ts-testing-003, ts-testing-005, ts-testing-007 — hardcoded values, bad naming, missing verifications"
// ---

// ❌ BAD: Hard-coded values; no DeepClone; missing VerifyNoOtherCalls; poor test name.

[Fact]
private async Task TestAddStudent() // ❌ does not follow ShouldDo{Outcome}Async pattern
{
    // ❌ hard-coded values — violates ts-testing-005
    Student inputStudent = new Student
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = "Hassan"
    };

    Student storageStudent = inputStudent; // ❌ not DeepCloned — violates ts-testing-006

    this.storageBrokerMock.Setup(broker =>
        broker.InsertStudentAsync(inputStudent))
            .ReturnsAsync(storageStudent);

    Student actualStudent =
        await this.studentService.AddStudentAsync(inputStudent);

    // ❌ no // given / // when / // then comments — violates ts-testing-004
    // ❌ no Times.Once verification — violates ts-testing-007
    // ❌ no VerifyNoOtherCalls() — violates ts-testing-007
    actualStudent.Should().BeEquivalentTo(storageStudent);
}
