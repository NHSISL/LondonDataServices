using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();
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
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(someStudent);

            StudentDependencyException actualStudentDependencyException =
                await Assert.ThrowsAsync<StudentDependencyException>(
                    addStudentTask.AsTask);

            // then
            actualStudentDependencyException.Should()
                .BeEquivalentTo(expectedStudentDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfStudentAlreadyExsitsAndLogItAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student alreadyExistsStudent = randomStudent;
            string randomMessage = GetRandomString();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsStudentException =
                new AlreadyExistsStudentException(
                    message: "Student with the same Id already exists.",
                    innerException: duplicateKeyException);

            var expectedStudentDependencyValidationException =
                new StudentDependencyValidationException(
                    message: "Student dependency validation occurred, please try again.",
                    innerException: alreadyExistsStudentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(alreadyExistsStudent);

            // then
            StudentDependencyValidationException actualStudentDependencyValidationException =
                await Assert.ThrowsAsync<StudentDependencyValidationException>(
                    addStudentTask.AsTask);

            actualStudentDependencyValidationException.Should()
                .BeEquivalentTo(expectedStudentDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentDependencyValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidStudentReferenceException =
                new InvalidStudentReferenceException(
                    message: "Invalid student reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            var expectedStudentValidationException =
                new StudentDependencyValidationException(
                    message: "Student dependency validation occurred, please try again.",
                    innerException: invalidStudentReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Student> addStudentTask =
                this.studentService.AddStudentAsync(someStudent);

            // then
            StudentDependencyValidationException actualStudentDependencyValidationException =
                await Assert.ThrowsAsync<StudentDependencyValidationException>(
                    addStudentTask.AsTask);

            actualStudentDependencyValidationException.Should().BeEquivalentTo(expectedStudentValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(someStudent),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}