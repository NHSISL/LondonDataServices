using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.Students;
using LHDS.Core.Models.Foundations.Students.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidStudentId = Guid.Empty;

            var invalidStudentException = 
                new InvalidStudentException(
                    message: "Invalid student. Please correct the errors and try again.");

            invalidStudentException.AddData(
                key: nameof(Student.Id),
                values: "Id is required");

            var expectedStudentValidationException =
                new StudentValidationException(
                    message: "Student validation errors occurred, please try again.",
                    innerException: invalidStudentException);

            // when
            ValueTask<Student> removeStudentByIdTask =
                this.studentService.RemoveStudentByIdAsync(invalidStudentId);

            StudentValidationException actualStudentValidationException =
                await Assert.ThrowsAsync<StudentValidationException>(
                    removeStudentByIdTask.AsTask);

            // then
            actualStudentValidationException.Should()
                .BeEquivalentTo(expectedStudentValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}