// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveByIdIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedSubscriberAgreementProcessingDependencyValidationException =
                new SubscriberAgreementProcessingDependencyValidationException(
                    message: "Subscriber agreement processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(someId))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRemoveByIdTask =
                this.subscriberAgreementProcessingService.RemoveSubscriberAgreementByIdAsync(someId);

            SubscriberAgreementProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyValidationException>(
                    subscriberAgreementRemoveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyValidationException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyValidationException))),
                         Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedSubscriberAgreementProcessingDependencyException =
                new SubscriberAgreementProcessingDependencyException(
                    message: "Subscriber agreement processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(someId))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRemoveByIdTask =
                this.subscriberAgreementProcessingService.RemoveSubscriberAgreementByIdAsync(someId);

            SubscriberAgreementProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyException>(
                    subscriberAgreementRemoveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyException))),
                         Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveByIdIfServiceErrorOccursAsync()
        {
            // given
            Guid someId = Guid.NewGuid();

            var serviceException = new Exception();

            var failedSubscriberAgreementProcessingServiceException =
                new FailedSubscriberAgreementProcessingServiceException(
                    message: "Failed subscriber agreement processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedSubscriberAgreementProcessingServiveException =
                new SubscriberAgreementProcessingServiceException(
                    message: "Subscriber agreement processing service error occurred, please contact support.",
                    innerException: failedSubscriberAgreementProcessingServiceException);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RemoveSubscriberAgreementByIdAsync(someId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRemoveByIdTask =
                this.subscriberAgreementProcessingService.RemoveSubscriberAgreementByIdAsync(someId);

            SubscriberAgreementProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingServiceException>(
                    subscriberAgreementRemoveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingServiveException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RemoveSubscriberAgreementByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingServiveException))),
                         Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
