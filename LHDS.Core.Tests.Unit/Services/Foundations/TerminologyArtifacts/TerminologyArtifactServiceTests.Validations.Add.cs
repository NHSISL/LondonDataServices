using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfTerminologyArtifactIsNullAndLogItAsync()
        {
            // given
            TerminologyArtifact nullTerminologyArtifact = null;

            var nullTerminologyArtifactException =
                new NullTerminologyArtifactException(message: "TerminologyArtifact is null.");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: nullTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.AddTerminologyArtifactAsync(nullTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfTerminologyArtifactIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidTerminologyArtifact = new TerminologyArtifact
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidTerminologyArtifactException =
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.Id),
                values: "Id is required");

            //invalidTerminologyArtifactException.AddData(
            //    key: nameof(TerminologyArtifact.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the TerminologyArtifact model

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedDate),
                values: "Date is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.CreatedBy),
                values: "Text is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values: "Date is required");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedBy),
                values: "Text is required");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.AddTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyArtifact randomTerminologyArtifact = CreateRandomTerminologyArtifact(randomDateTimeOffset);
            TerminologyArtifact invalidTerminologyArtifact = randomTerminologyArtifact;

            invalidTerminologyArtifact.UpdatedDate =
                invalidTerminologyArtifact.CreatedDate.AddDays(randomNumber);

            var invalidTerminologyArtifactException = 
                new InvalidTerminologyArtifactException(
                    message: "Invalid terminologyArtifact. Please correct the errors and try again.");

            invalidTerminologyArtifactException.AddData(
                key: nameof(TerminologyArtifact.UpdatedDate),
                values: $"Date is not the same as {nameof(TerminologyArtifact.CreatedDate)}");

            var expectedTerminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: invalidTerminologyArtifactException);

            // when
            ValueTask<TerminologyArtifact> addTerminologyArtifactTask =
                this.terminologyArtifactService.AddTerminologyArtifactAsync(invalidTerminologyArtifact);

            TerminologyArtifactValidationException actualTerminologyArtifactValidationException =
                await Assert.ThrowsAsync<TerminologyArtifactValidationException>(
                    addTerminologyArtifactTask.AsTask);

            // then
            actualTerminologyArtifactValidationException.Should()
                .BeEquivalentTo(expectedTerminologyArtifactValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyArtifactValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyArtifactAsync(It.IsAny<TerminologyArtifact>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}