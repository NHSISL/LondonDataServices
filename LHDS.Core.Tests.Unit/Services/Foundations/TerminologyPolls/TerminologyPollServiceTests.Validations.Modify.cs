using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollIsNullAndLogItAsync()
        {
            // given
            TerminologyPoll nullTerminologyPoll = null;
            var nullTerminologyPollException = new NullTerminologyPollException(message: "TerminologyPoll is null.");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: nullTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(nullTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidTerminologyPoll = new TerminologyPoll
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidTerminologyPollException = 
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.Id),
                values: "Id is required");

            //invalidTerminologyPollException.AddData(
            //    key: nameof(TerminologyPoll.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the TerminologyPoll model

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedDate),
                values: "Date is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.CreatedBy),
                values: "Text is required");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(TerminologyPoll.CreatedDate)}"
                });

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedBy),
                values: "Text is required");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            //then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll invalidTerminologyPoll = randomTerminologyPoll;

            var invalidTerminologyPollException = 
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: $"Date is the same as {nameof(TerminologyPoll.CreatedDate)}");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(invalidTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(invalidTerminologyPoll.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            randomTerminologyPoll.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidTerminologyPollException = 
                new InvalidTerminologyPollException(
                    message: "Invalid terminologyPoll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.UpdatedDate),
                values: "Date is not recent");

            var expectedTerminologyPollValidatonException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: invalidTerminologyPollException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(randomTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomModifyTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll nonExistTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll nullTerminologyPoll = null;

            var notFoundTerminologyPollException =
                new NotFoundTerminologyPollException(nonExistTerminologyPoll.Id);

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: notFoundTerminologyPollException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(nonExistTerminologyPoll.Id))
                .ReturnsAsync(nullTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollService.ModifyTerminologyPollAsync(nonExistTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(nonExistTerminologyPoll.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}