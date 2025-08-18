// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task
            ShouldThrowValidationExceptionsOnRetrieveOrAddIfSubscriberAgreementProcessingIsNullAndLogItAsync()
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
            ValueTask<SubscriberAgreement> addSubscriberAgreementTask =
                this.subscriberAgreementProcessingService
                    .RetrieveOrAddSubscriberAgreementAsync(nullSubscriberAgreement);

            SubscriberAgreementProcessingValidationException actualSubscriberAgreementProcessingValidationException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingValidationException>(
                    addSubscriberAgreementTask.AsTask);

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