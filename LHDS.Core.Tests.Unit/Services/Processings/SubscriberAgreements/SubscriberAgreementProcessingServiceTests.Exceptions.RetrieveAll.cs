// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedSubscriberAgreementProcessingDependencyValidationException =
                new SubscriberAgreementProcessingDependencyValidationException(
                    message: "Subscriber agreement processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IQueryable<SubscriberAgreement>> subscriberAgreementRetrieveTask =
                subscriberAgreementProcessingService.RetrieveAllSubscriberAgreementsAsync();

            SubscriberAgreementProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyValidationException>(
                    testCode: subscriberAgreementRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyValidationException);

            subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyValidationException))),
                         Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedSubscriberAgreementProcessingDependencyException =
                new SubscriberAgreementProcessingDependencyException(
                    message: "Subscriber agreement processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            subscriberAgreementServiceMock.Setup(service =>
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IQueryable<SubscriberAgreement>> subscriberAgreementRetrieveTask =
                subscriberAgreementProcessingService.RetrieveAllSubscriberAgreementsAsync();

            SubscriberAgreementProcessingDependencyException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingDependencyException>(
                    testCode: subscriberAgreementRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingDependencyException);

            subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingDependencyException))),
                         Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
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
                service.RetrieveAllSubscriberAgreementsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IQueryable<SubscriberAgreement>> subscriberAgreementRetrieveTask =
                subscriberAgreementProcessingService.RetrieveAllSubscriberAgreementsAsync();

            SubscriberAgreementProcessingServiceException actualException =
                await Assert.ThrowsAsync<SubscriberAgreementProcessingServiceException>(
                    testCode: subscriberAgreementRetrieveTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedSubscriberAgreementProcessingServiveException);

            subscriberAgreementServiceMock.Verify(service =>
                service.RetrieveAllSubscriberAgreementsAsync(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedSubscriberAgreementProcessingServiveException))),
                         Times.Once);

            subscriberAgreementServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}