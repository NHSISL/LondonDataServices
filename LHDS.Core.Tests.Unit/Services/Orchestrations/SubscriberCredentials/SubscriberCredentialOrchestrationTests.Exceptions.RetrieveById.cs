// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.SubscriberCredentials
{
    public partial class SubscriberCredentialOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationOnRetrieveSubscriberCredentialByIdIfDependencyValidationOccursAndLogItAsync(
                Xeption dependencyValidationException)
        {
            // given
            Guid randomId = Guid.NewGuid();

            var expectedDependencyException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberCredential> retreieveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RetrieveSubscriberCredentialByIdAsync(randomId);

            SubscriberCredentialOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationDependencyValidationException>(
                    retreieveSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveSubscriberCredentialByIdIfDependencyErrorOccursAndLogItAsync(
                Xeption dependencyException)
        {
            // given
            Guid randomId = Guid.NewGuid();

            var expectedDependencyException =
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<SubscriberCredential> retreieveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RetrieveSubscriberCredentialByIdAsync(randomId);

            SubscriberCredentialDependencyOrchestrationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialDependencyOrchestrationException>(
                    retreieveSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnRetrieveSubscriberCredentialByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid randomId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedSubscriberCredentialOrchestrationServiceException =
                new FailedSubscriberCredentialOrchestrationServiceException(
                    message: "Failed subscriber credential orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDependencyException =
                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, " +
                        "fix the errors and try again.",
                    innerException: failedSubscriberCredentialOrchestrationServiceException);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberCredential> retreieveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RetrieveSubscriberCredentialByIdAsync(randomId);

            SubscriberCredentialOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationServiceException>(
                    retreieveSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveSubscriberAgreementByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}