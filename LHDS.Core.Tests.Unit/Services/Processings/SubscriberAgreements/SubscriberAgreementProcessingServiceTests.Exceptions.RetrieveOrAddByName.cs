// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Processings.SubscriberAgreements.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.SubscriberAgreements
{
    public partial class SubscriberAgreementProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrAddNameIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = someSubscriberAgreement;

            var expectedSubscriberAgreementProcessingDependencyValidationException =
                new SubscriberAgreementProcessingDependencyValidationException(
                    message: "Subscriber agreement processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRetrieveOrAddTask =
                this.subscriberAgreementProcessingService.RetrieveOrAddSubscriberAgreementByNameAsync(
                    inputSubscriberAgreement);

            SubscriberAgreementProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyValidationException>(
                    subscriberAgreementRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyValidationException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyValidationException))),
                         Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}