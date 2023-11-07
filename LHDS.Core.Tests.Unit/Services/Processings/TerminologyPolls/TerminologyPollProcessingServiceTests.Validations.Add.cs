// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfTerminologyPollIsNullAndLogItAsync()
        {
            // given
            TerminologyPoll nullTerminologyPoll = null;

            var nullTerminologyPollException =
                new NullTerminologyPollException(message: "Terminology poll is null.");

            var expectedTerminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: nullTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(nullTerminologyPoll);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(() =>
                    addTerminologyPollTask.AsTask());

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfTerminologyPollIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidTerminologyPoll = new TerminologyPoll
            {
                ResourceType = invalidText,
            };

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminology poll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.Id),
                values: "Id is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.ResourceType),
                values: "Text is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedDate),
                values: "Date is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedBy),
                values: "Text is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: "Date is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedBy),
                values: "Text is required");

            var expectedTerminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(() =>
                    addTerminologyPollTask.AsTask());

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;

            invalidTerminologyPoll.UpdatedDate =
                invalidTerminologyPoll.CreatedDate.AddDays(randomNumber);

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminology poll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: $"Date is not the same as {nameof(TerminologyPoll.CreatedDate)}");

            var expectedTerminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(() =>
                    addTerminologyPollTask.AsTask());

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedByAndUpdatedByIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;
            invalidTerminologyPoll.UpdatedBy = Guid.NewGuid().ToString();

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminology poll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedBy),
                values: $"Text is not the same as {nameof(TerminologyPoll.CreatedBy)}");

            var expectedTerminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(() =>
                    addTerminologyPollTask.AsTask());

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTimeOffset.AddMinutes(minutesBeforeOrAfter);

            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(invalidDateTime);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminology poll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedDate),
                values: "Date is not recent");

            var expectedTerminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollProcessingService.AddTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(() =>
                    addTerminologyPollTask.AsTask());

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
        }
    }
}