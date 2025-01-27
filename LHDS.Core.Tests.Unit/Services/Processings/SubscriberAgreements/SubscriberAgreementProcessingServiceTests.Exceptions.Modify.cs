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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfErrorOccursAndLogItAsync(
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
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementModifyTask =
                this.subscriberAgreementProcessingService.ModifySubscriberAgreementAsync(inputSubscriberAgreement);

            SubscriberAgreementProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyValidationException>(
                    subscriberAgreementModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyValidationException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement),
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
        public async Task ShouldThrowDependencyExceptionOnModifyIfDependencyErrorOccursAndLogItAsync(
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
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementModifyTask =
                this.subscriberAgreementProcessingService.ModifySubscriberAgreementAsync(inputSubscriberAgreement);

            SubscriberAgreementProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyException>(
                    subscriberAgreementModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyException))),
                         Times.Once);

            this.subscriberAgreementServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAsync()
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
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SubscriberAgreement> subscriberAgreementModifyTask =
                this.subscriberAgreementProcessingService.ModifySubscriberAgreementAsync(inputSubscriberAgreement);

            SubscriberAgreementProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingServiceException>(
                    subscriberAgreementModifyTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingServiveException);

            this.subscriberAgreementServiceMock.Verify(service =>
                service.ModifySubscriberAgreementAsync(inputSubscriberAgreement),
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