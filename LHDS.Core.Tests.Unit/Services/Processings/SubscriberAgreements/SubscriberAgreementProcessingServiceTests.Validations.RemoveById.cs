// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionsOnRemoveIfSubscriberAgreementProcessingIsNullAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidArgumentSubscriberAgreementProcessingException =
                new InvalidArgumentSubscriberAgreementProcessingException(
                    message: "Invalid argument(s). Please correct the errors and try again.");

            invalidArgumentSubscriberAgreementProcessingException.AddData(
                key: "Id",
                values: "Id is required");

            var expectedSubscriberAgreementProcessingValidationException =
                new SubscriberAgreementProcessingValidationException(
                    message: "Subscriber agreement processing validation error occurred, please try again.",
                    innerException: invalidArgumentSubscriberAgreementProcessingException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRemoveByIdTask =
                this.subscriberAgreementProcessingService.RemoveSubscriberAgreementByIdAsync(invalidId);

            SubscriberAgreementProcessingValidationException actualSubscriberAgreementProcessingValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingValidationException>(
                    subscriberAgreementRemoveByIdTask.AsTask);

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
