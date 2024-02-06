using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberAgreementIsNullAndLogItAsync()
        {
            // given
            SubscriberAgreement nullSubscriberAgreement = null;

            var nullSubscriberAgreementException =
                new NullSubscriberAgreementException(message: "SubscriberAgreement is null.");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: nullSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(nullSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfSubscriberAgreementIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidSubscriberAgreement = new SubscriberAgreement
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidSubscriberAgreementException =
                new InvalidSubscriberAgreementException(
                    message: "Invalid subscriberAgreement. Please correct the errors and try again.");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.Id),
                values: "Id is required");

            //invalidSubscriberAgreementException.AddData(
            //    key: nameof(SubscriberAgreement.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the SubscriberAgreement model

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedDate),
                values: "Date is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.CreatedBy),
                values: "Text is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedDate),
                values: "Date is required");

            invalidSubscriberAgreementException.AddData(
                key: nameof(SubscriberAgreement.UpdatedBy),
                values: "Text is required");

            var expectedSubscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: invalidSubscriberAgreementException);

            // when
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementService.AddSubscriberAgreementAsync(invalidSubscriberAgreement);

            SubscriberAgreementValidationException actualSubscriberAgreementValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementValidationException>(
                    addSubscriberAgreementTask.AsTask);

            // then
            actualSubscriberAgreementValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}