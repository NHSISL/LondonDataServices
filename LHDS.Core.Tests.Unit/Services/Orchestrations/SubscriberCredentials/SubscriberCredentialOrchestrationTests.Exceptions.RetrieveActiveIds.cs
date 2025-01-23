// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
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
            ShouldThrowDependencyValidationOnRetrieveActiveIdsIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedDependencyValidationException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<List<Guid>> retrieveActiveSubscriberCredentialIdsTask =
                this.subscriberCredentialOrchestration.RetrieveAllActiveSubscriberCredentialIdsAsync();

            SubscriberCredentialOrchestrationDependencyValidationException actualDependencyValidationException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationDependencyValidationException>(
                    retrieveActiveSubscriberCredentialIdsTask.AsTask);

            // then
            actualDependencyValidationException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreementsAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedDependencyValidationException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(SubscriberCredentialOrchestrationDependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveActiveIdslIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedDependencyException =
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<List<Guid>> retrieveActiveSubscriberCredentialIdsTask =
                this.subscriberCredentialOrchestration.RetrieveAllActiveSubscriberCredentialIdsAsync();

            SubscriberCredentialDependencyOrchestrationException actualDepenedencyException =
                await Assert.ThrowsAsync<SubscriberCredentialDependencyOrchestrationException>(
                    retrieveActiveSubscriberCredentialIdsTask.AsTask);

            // then
            actualDepenedencyException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreementsAsync(),
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
            ShouldThrowServiceExceptionOnRetrieveActiveIdsIfServiceErrorOccursAndLogItAsync()
        {
            //given
            var serviceException = new Exception();

            var failedSubscriberCredentialOrchestrationServiceException =
                new FailedSubscriberCredentialOrchestrationServiceException(
                    message: "Failed subscriber credential orchestration service error occurred, " +
                        "please contact support.",
                    innerException: serviceException);

            var expectedSerivceException =
                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, " +
                        "fix the errors and try again.",
                    innerException: failedSubscriberCredentialOrchestrationServiceException);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
               service.RetrieveAllSubscriberAgreementsAsync())
                   .ThrowsAsync(serviceException);

            // when
            ValueTask<List<Guid>> retrieveActiveSubscriberCredentialIdsTask =
                this.subscriberCredentialOrchestration.RetrieveAllActiveSubscriberCredentialIdsAsync();

            SubscriberCredentialOrchestrationServiceException actualServiceException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationServiceException>(
                    retrieveActiveSubscriberCredentialIdsTask.AsTask);

            // then
            actualServiceException.Should()
                 .BeEquivalentTo(expectedSerivceException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreementsAsync(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedSerivceException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}