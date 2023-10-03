using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
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

            StudentDependencyValidationException expectedStudentDependencyValidationException =
                new StudentDependencyValidationException(
                    message: "Student dependency validation occurred, please try again.",
                    innerException: invalidStudentReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Student> modifyStudentTask =
                this.studentService.ModifyStudentAsync(someStudent);

            StudentDependencyValidationException actualStudentDependencyValidationException =
                await Assert.ThrowsAsync<StudentDependencyValidationException>(
                    modifyStudentTask.AsTask);

            // then
            actualStudentDependencyValidationException.Should()
                .BeEquivalentTo(expectedStudentDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(someStudent.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(someStudent),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            var databaseUpdateException = new DbUpdateException();

            var failedStudentStorageException =
                new FailedStudentStorageException(
                    message: "Failed student storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedStudentDependencyException =
                new StudentDependencyException(
                    message: "Student dependency error occurred, contact support.",
                    innerException: failedStudentStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(randomStudent),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedStudentException =
                new LockedStudentException(
                    message: "Locked student record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedStudentDependencyValidationException =
                new StudentDependencyValidationException(
                    message: "Student dependency validation occurred, please try again.",
                    innerException: lockedStudentException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Student> modifyStudentTask =
                this.studentService.ModifyStudentAsync(randomStudent);

            StudentDependencyValidationException actualStudentDependencyValidationException =
                await Assert.ThrowsAsync<StudentDependencyValidationException>(
                    modifyStudentTask.AsTask);

            // then
            actualStudentDependencyValidationException.Should()
                .BeEquivalentTo(expectedStudentDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(randomStudent.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(randomStudent),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            var serviceException = new Exception();

            var failedStudentServiceException =
                new FailedStudentServiceException(
                    message: "Failed student service occurred, please contact support", 
                    innerException: serviceException);

            var expectedStudentServiceException =
                new StudentServiceException(
                    message: "Student service error occurred, contact support.",
                    innerException: failedStudentServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Student> modifyStudentTask =
                this.studentService.ModifyStudentAsync(randomStudent);

            StudentServiceException actualStudentServiceException =
                await Assert.ThrowsAsync<StudentServiceException>(
                    modifyStudentTask.AsTask);

            // then
            actualStudentServiceException.Should()
                .BeEquivalentTo(expectedStudentServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(randomStudent.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentServiceException))),
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