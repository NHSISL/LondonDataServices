// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
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
            ShouldThrowDependencyValidationOnModifyOrAddSubscriberCredentialIfDependencyValidationOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomId = Guid.NewGuid();

            dynamic randomDynamic = CreateRandomDynamicSubscriberAgreementCredential(
                date: randomDateTimeOffset,
                id: randomId);

            SubscriberCredential inputSubscriberCredential = CreateSubscriberCredentialFromDynamic(randomDynamic);

            var expectedDependencyException =
                new SubscriberCredentialOrchestrationDependencyValidationException(
                    message: "Subscriber credential orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberCredential> modifyOrAddSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredentialOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationDependencyValidationException>(
                    modifyOrAddSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
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
            ShouldThrowDependencyExceptionOnModifyOrAddSubscriberCredentialIfDependencyErrorOccursAndLogItAsync(
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
                service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<SubscriberCredential> modifyOrAddSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredentialDependencyOrchestrationException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialDependencyOrchestrationException>(
                    modifyOrAddSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
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
            ShouldThrowServiceExceptionOnModifyOrAddSubscriberCredentialIfServiceErrorOccursAndLogItAsync()
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
                    message: "Failed subscriber credential orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedDependencyException =
                new SubscriberCredentialOrchestrationServiceException(
                    message: "Subscriber credential orchestration service error occurred, " +
                        "fix the errors and try again.",
                    innerException: failedSubscriberCredentialOrchestrationServiceException);

            this.subscriberAgreementProcessingServiceMock.Setup(service =>
                service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberCredential> modifyOrAddSubscriberCredentialTask =
                this.subscriberCredentialOrchestration.ModifyOrAddSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredentialOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<SubscriberCredentialOrchestrationServiceException>(
                    modifyOrAddSubscriberCredentialTask.AsTask);

            // then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.subscriberAgreementProcessingServiceMock.Verify(service =>
             service.ModifyOrAddSubscriberAgreementAsync(It.IsAny<SubscriberAgreement>()),
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