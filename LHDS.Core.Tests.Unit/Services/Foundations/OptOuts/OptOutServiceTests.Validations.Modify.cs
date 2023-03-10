using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.OptOuts;
using LHDS.Core.Models.OptOuts.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.OptOuts
{
    public partial class OptOutServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsNullAndLogItAsync()
        {
            // given
            OptOut nullOptOut = null;
            var nullOptOutException = new NullOptOutException();

            var expectedOptOutValidationException =
                new OptOutValidationException(nullOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(nullOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfOptOutIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidOptOut = new OptOut
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidOptOutException = new InvalidOptOutException();

            invalidOptOutException.AddData(
                key: nameof(OptOut.Id),
                values: "Id is required");

            //invalidOptOutException.AddData(
            //    key: nameof(OptOut.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the OptOut model

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedDate),
                values: "Date is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.CreatedByUserId),
                values: "Id is required");

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(OptOut.CreatedDate)}"
                });

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedByUserId),
                values: "Id is required");

            var expectedOptOutValidationException =
                new OptOutValidationException(invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            //then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateOptOutAsync(It.IsAny<OptOut>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            OptOut invalidOptOut = randomOptOut;
            var invalidOptOutException = new InvalidOptOutException();

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: $"Date is the same as {nameof(OptOut.CreatedDate)}");

            var expectedOptOutValidationException =
                new OptOutValidationException(invalidOptOutException);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(invalidOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should()
                .BeEquivalentTo(expectedOptOutValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(invalidOptOut.Id),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            OptOut randomOptOut = CreateRandomOptOut(randomDateTimeOffset);
            randomOptOut.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidOptOutException =
                new InvalidOptOutException();

            invalidOptOutException.AddData(
                key: nameof(OptOut.UpdatedDate),
                values: "Date is not recent");

            var expectedOptOutValidatonException =
                new OptOutValidationException(invalidOptOutException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<OptOut> modifyOptOutTask =
                this.optOutService.ModifyOptOutAsync(randomOptOut);

            OptOutValidationException actualOptOutValidationException =
                await Assert.ThrowsAsync<OptOutValidationException>(
                    modifyOptOutTask.AsTask);

            // then
            actualOptOutValidationException.Should().BeEquivalentTo(expectedOptOutValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectOptOutByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}