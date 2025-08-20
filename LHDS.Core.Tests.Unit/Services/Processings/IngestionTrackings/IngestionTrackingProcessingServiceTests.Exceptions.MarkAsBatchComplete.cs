// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.IngestionTrackings.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackings
{
    public partial class IngestionTrackingProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnMarkAsBatchCompleteIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();
            bool someIsBatchComplete = true;

            var expectedIngestionTrackingProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask markAsBatchCompleteAsyncTask =
                this.ingestionTrackingProcessingService.MarkAsBatchCompleteAsync(someId, someIsBatchComplete);

            IngestionTrackingProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingDependencyValidationException>(
                    markAsBatchCompleteAsyncTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyValidationException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyValidationException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnMarkAsBatchCompleteIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();
            bool someIsBatchComplete = true;

            var expectedIngestionTrackingProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask markAsBatchCompleteAsyncTask =
                this.ingestionTrackingProcessingService.MarkAsBatchCompleteAsync(someId, someIsBatchComplete);

            IngestionTrackingProcessingDependencyException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingDependencyException>(
                    markAsBatchCompleteAsyncTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnMarkAsBatchCompleteIfServiceErrorOccursAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            bool someIsBatchComplete = true;
            var serviceException = new Exception();

            var failedIngestionTrackingProcessingServiceException =
                new FailedIngestionTrackingProcessingServiceException(
                    message: "Failed IngestionTracking processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingProcessingServiveException =
                new IngestionTrackingProcessingServiceException(
                    message: "IngestionTracking processing service error occurred, please contact support.",
                    innerException: failedIngestionTrackingProcessingServiceException);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask markAsBatchCompleteAsyncTask =
                this.ingestionTrackingProcessingService.MarkAsBatchCompleteAsync(someId, someIsBatchComplete);

            IngestionTrackingProcessingServiceException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingServiceException>(
                    markAsBatchCompleteAsyncTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingServiveException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingServiveException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
