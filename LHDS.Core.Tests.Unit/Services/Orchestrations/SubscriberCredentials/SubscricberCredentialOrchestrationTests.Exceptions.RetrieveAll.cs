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
            ShouldThrowDependencyValidationOnRetrieveAllSubscriberCredentialIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);

            var expectedDependencyValidationException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Throws(dependencyValidationException);

            // when
            Action retrieveAllSubscriberCredentialsAction = () =>
                this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentials();

            SubscriberCredentialOrchestrationDependencyValidationException actualDependencyValidationException =
                Assert.Throws<SubscriberCredentialOrchestrationDependencyValidationException>(
                    retrieveAllSubscriberCredentialsAction);

            // then
            actualDependencyValidationException.Should()
                 .BeEquivalentTo(expectedDependencyValidationException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreements(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
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
            ShouldThrowDependencyExceptionOnretrieveAllSubscriberCredentialIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);

            var expectedDependencyException =
                new SubscriberCredentialDependencyOrchestrationException(
                    message: "Subscriber credential orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreements())
                    .Throws(dependencyException);

            // when
            Action retrieveAllSubscriberCredentialsAction = () =>
                this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentials();

            SubscriberCredentialDependencyOrchestrationException actualDependencyVException =
                Assert.Throws<SubscriberCredentialDependencyOrchestrationException>(
                    retrieveAllSubscriberCredentialsAction);

            // then
            actualDependencyVException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreements(),
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
            ShouldThrowServiceExceptionOnRetrieveAllSubscriberCredentialIfServiceErrorOccursAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);
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
               service.RetrieveAllSubscriberAgreements())
                   .Throws(serviceException);

            // when
            Action retrieveAllSubscriberCredentialsAction = () =>
                this.subscriberCredentialOrchestration.RetrieveAllSubscriberCredentials();

            SubscriberCredentialOrchestrationServiceException actualServiceException =
                Assert.Throws<SubscriberCredentialOrchestrationServiceException>(
                    retrieveAllSubscriberCredentialsAction);

            // then
            actualServiceException.Should()
                 .BeEquivalentTo(expectedSerivceException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.RetrieveAllSubscriberAgreements(),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedSerivceException))),
                       Times.Once);

            this.subscriberAgreementProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.secureDataProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}