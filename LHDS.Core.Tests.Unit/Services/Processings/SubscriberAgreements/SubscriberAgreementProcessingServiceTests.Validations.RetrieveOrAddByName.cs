// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Securities;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions;
using LHDS.Core.Services.Foundations.SubscriberAgreements;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingServiceTests
    {
        [Fact]
        public async Task 
            ShouldThrowValidationExceptionsOnRetrieveOrAddByNameIfSubscriberAgreementProcessingIsNullAndLogItAsync()
        {
            // given
            SubscriberAgreement nullSubscriberAgreement = null;

            var nullSubscriberAgreementProcessingException =
                new NullSubscriberAgreementProcessingException(message: "Subscriber agreement is null.");

            var expectedSubscriberAgreementProcessingValidationException =
                new SubscriberAgreementProcessingValidationException(
                    message: "Subscriber agreement processing validation error occurred, please try again.",
                    innerException: nullSubscriberAgreementProcessingException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementModifyTask =
                this.subscriberAgreementProcessingService.RetrieveOrAddSubscriberAgreementByNameAsync(
                    nullSubscriberAgreement);

            SubscriberAgreementProcessingValidationException actualSubscriberAgreementProcessingValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingValidationException>(
                    subscriberAgreementModifyTask.AsTask);

            //then
            actualSubscriberAgreementProcessingValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementProcessingValidationException))),
                        Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task 
            ShouldThrowValidationExceptionOnRetrieveOrAddIfSubscriberAgreementProcessingIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given 
            var invalidSubscriberAgreement = new SubscriberAgreement
            {
                SupplierSharingAgreementShortName = invalidText,
            };

            var invalidArgumentSubscriberAgreementProcessingException =
                new InvalidArgumentSubscriberAgreementProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentSubscriberAgreementProcessingException.AddData(
                key: nameof(SubscriberAgreement.SupplierSharingAgreementShortName),
                values: "Text is required");

            var expectedSubscriberAgreementProcessingValidationException =
                 new SubscriberAgreementProcessingValidationException(
                     message: "Subscriber agreement processing validation error occurred, please try again.",
                     innerException: invalidArgumentSubscriberAgreementProcessingException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementModifyTask =
                this.subscriberAgreementProcessingService.RetrieveOrAddSubscriberAgreementByNameAsync(
                    invalidSubscriberAgreement);

            SubscriberAgreementProcessingValidationException actualSubscriberAgreementProcessingValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingValidationException>(
                    subscriberAgreementModifyTask.AsTask);

            //then
            actualSubscriberAgreementProcessingValidationException.Should()
                .BeEquivalentTo(expectedSubscriberAgreementProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedSubscriberAgreementProcessingValidationException))),
                        Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}