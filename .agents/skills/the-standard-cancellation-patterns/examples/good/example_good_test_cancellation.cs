// ---
// skill: the-standard-cancellation-patterns
// type: example
// source-section: "9.0 Testing Standards"
// demonstrates: "tsc-csharp-cp-015, tsc-csharp-cp-016 — unit tests for cancellation and timeout"
// ---

// ✅ Theory MemberData includes TimeoutException.
// ✅ A dedicated test asserts that OperationCanceledException propagates unwrapped.

public partial class StudentServiceTests
{
    // --- tsc-csharp-cp-015: TimeoutException in MemberData ---

    public static TheoryData<Exception> DependencyExceptions =>
        new TheoryData<Exception>
        {
            new HttpRequestException(),
            new SqlException(),
            new TimeoutException()
        };

    [Theory]
    [MemberData(nameof(DependencyExceptions))]
    public async Task ShouldThrowDependencyExceptionOnRetrieveIfDependencyErrorOccursAsync(
        Exception dependencyException)
    {
        // given
        Guid someStudentId = Guid.NewGuid();

        var failedDependencyException =
            new FailedStudentDependencyException(
                message: "Failed student dependency error occurred.",
                innerException: dependencyException);

        var expectedStudentDependencyException =
            new StudentDependencyException(
                message: "Student dependency error occurred.",
                innerException: failedDependencyException);

        this.studentBrokerMock
            .Setup(broker => broker.SelectStudentByIdAsync(
                someStudentId,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(dependencyException);

        // when
        ValueTask<Student> retrieveStudentTask =
            this.studentService.RetrieveStudentByIdAsync(
                someStudentId);

        // then
        await Assert.ThrowsAsync<StudentDependencyException>(
            () => retrieveStudentTask.AsTask());
    }

    // --- tsc-csharp-cp-016: OperationCanceledException propagates unwrapped ---

    [Fact]
    public async Task ShouldThrowOperationCanceledExceptionOnRetrieveIfCanceledAsync()
    {
        // given
        Guid someStudentId = Guid.NewGuid();
        using var cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        this.studentBrokerMock
            .Setup(broker => broker.SelectStudentByIdAsync(
                someStudentId,
                cancellationToken))
            .ThrowsAsync(new OperationCanceledException());

        cancellationTokenSource.Cancel();

        // when
        ValueTask<Student> retrieveStudentTask =
            this.studentService.RetrieveStudentByIdAsync(
                someStudentId,
                cancellationToken);

        // then
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => retrieveStudentTask.AsTask());

        // OperationCanceledException must NOT be wrapped in StudentServiceException,
        // StudentDependencyException, or any other exception type.
    }
}
