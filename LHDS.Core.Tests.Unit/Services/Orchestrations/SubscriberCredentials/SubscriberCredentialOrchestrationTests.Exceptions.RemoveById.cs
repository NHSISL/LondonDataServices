// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.SubscriberCredentials.Exceptions;
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
            ShouldThrowDependencyValidationOnRemoveSubscriberCredentialIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid randomId = Guid.NewGuid();

            var expectedDependencyException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.secureDataProcessingServiceMock.Setup(service =>
                service.RemoveSecureDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask renoveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RemoveSubscriberCredentialByIdAsync(randomId);

            SubscriberCredentialOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationDependencyValidationException>(
                    renoveSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.secureDataProcessingServiceMock.Verify(service =>
             service.RemoveSecureDataByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
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
            ShouldThrowDependencyExceptionOnRemoveSubscriberCredentialIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid randomId = Guid.NewGuid();

            var expectedDependencyException =
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.secureDataProcessingServiceMock.Setup(service =>
                service.RemoveSecureDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask renoveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RemoveSubscriberCredentialByIdAsync(randomId);

            SubscriberCredentialDependencyOrchestrationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialDependencyOrchestrationException>(
                    renoveSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.secureDataProcessingServiceMock.Verify(service =>
             service.RemoveSecureDataByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnRemoveSubscriberCredentialIfServiceErrorOccursAndLogItAsync()
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

            this.secureDataProcessingServiceMock.Setup(service =>
                service.RemoveSecureDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask renoveSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.RemoveSubscriberCredentialByIdAsync(randomId);

            SubscriberCredentialOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationServiceException>(
                    renoveSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.secureDataProcessingServiceMock.Verify(service =>
             service.RemoveSecureDataByIdAsync(It.IsAny<Guid>()),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}