// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedSubscriberAgreementProcessingDependencyValidationException =
                new SubscriberAgreementProcessingDependencyValidationException(
                    message: "Subscriber agreement processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Throws(dependencyValidationException);

            // when
            Action subscriberAgreementRetrieveAction = () =>
                subscriberAgreementProcessingService.RetrieveAllSubscriberAgreements();

            SubscriberAgreementProcessingDependencyValidationException actualException =
                Assert.Throws<SubscriberAgreementProcessingDependencyValidationException>(
                    subscriberAgreementRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyValidationException);

            subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreements(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyValidationException))),
                         Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedSubscriberAgreementProcessingDependencyException =
                new SubscriberAgreementProcessingDependencyException(
                    message: "Subscriber agreement processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Throws(dependencyException);

            // when
            Action subscriberAgreementRetrieveAction = () =>
                subscriberAgreementProcessingService.RetrieveAllSubscriberAgreements();

            SubscriberAgreementProcessingDependencyException actualException =
                Assert.Throws<SubscriberAgreementProcessingDependencyException>(subscriberAgreementRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyException);

            subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreements(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyException))),
                         Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedSubscriberAgreementProcessingServiceException =
                new FailedSubscriberAgreementProcessingServiceException(
                    message: "Failed subscriber agreement processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberAgreementProcessingServiveException =
                new SubscriberAgreementProcessingServiceException(
                    message: "Subscriber agreement processing service error occurred, please contact support.",
                    innerException: failedSubscriberAgreementProcessingServiceException);

            subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Throws(serviceException);

            // when
            Action subscriberAgreementRetrieveAction = () =>
                subscriberAgreementProcessingService.RetrieveAllSubscriberAgreements();

            SubscriberAgreementProcessingServiceException actualException =
                Assert.Throws<SubscriberAgreementProcessingServiceException>(subscriberAgreementRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingServiveException);

            subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreements(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingServiveException))),
                         Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}