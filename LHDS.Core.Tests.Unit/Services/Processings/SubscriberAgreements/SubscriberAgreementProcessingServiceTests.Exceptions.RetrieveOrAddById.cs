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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveOrAddIfErrorOccursAndLogItAsync(
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
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRetrieveOrAddTask =
                this.subscriberAgreementProcessingService.RetrieveOrAddSubscriberAgreementAsync(
                    inputSubscriberAgreement);

            SubscriberAgreementProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyValidationException>(
                    subscriberAgreementRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyValidationException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
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
        public async Task ShouldThrowDependencyExceptionOnRetrieveOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = someSubscriberAgreement;

            var expectedSubscriberAgreementProcessingDependencyException =
                new SubscriberAgreementProcessingDependencyException(
                    message: "Subscriber agreement processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRetrieveOrAddTask =
                this.subscriberAgreementProcessingService.RetrieveOrAddSubscriberAgreementAsync(
                    inputSubscriberAgreement);

            SubscriberAgreementProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyException>(
                    subscriberAgreementRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyException))),
                         Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveOrAddIfServiceErrorOccursAsync()
        {
            // given
            SubscriberAgreement someSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = someSubscriberAgreement;
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
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementRetrieveOrAddTask =
                this.subscriberAgreementProcessingService.RetrieveOrAddSubscriberAgreementAsync(
                    inputSubscriberAgreement);

            SubscriberAgreementProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingServiceException>(
                    subscriberAgreementRetrieveOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingServiveException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id),
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