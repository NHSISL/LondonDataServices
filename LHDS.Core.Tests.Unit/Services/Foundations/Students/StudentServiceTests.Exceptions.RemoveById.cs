using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using LHDS.Core.Models.Foundations.Students;
using LHDS.Core.Models.Foundations.Students.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            SqlException sqlException = GetSqlException();

            var failedStudentStorageException =
                new FailedStudentStorageException(
                    message: "Failed student storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedStudentDependencyException =
                new StudentDependencyException(
                    message: "Student dependency error occurred, contact support.",
                    innerException: failedStudentStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(randomStudent.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.RemoveStudentByIdAsync(randomStudent.Id);

            StudentDependencyException actualStudentDependencyException =
                await Assert.ThrowsAsync<StudentDependencyException>(
                    addStudentTask.AsTask);

            // then
            actualStudentDependencyException.Should()
                .BeEquivalentTo(expectedStudentDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(randomStudent.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}