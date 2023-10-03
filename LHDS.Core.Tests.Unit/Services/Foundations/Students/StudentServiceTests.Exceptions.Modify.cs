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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Student> modifyStudentTask =
                this.studentService.ModifyStudentAsync(randomStudent);

            StudentDependencyException actualStudentDependencyException =
                await Assert.ThrowsAsync<StudentDependencyException>(
                    modifyStudentTask.AsTask);

            // then
            actualStudentDependencyException.Should()
                .BeEquivalentTo(expectedStudentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(randomStudent.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(randomStudent),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}