// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.IngestionTrackingAudits;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.IngestionTrackingAudits
{
    public partial class IngestionTrackingAuditProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveByIdIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedIngestionTrackingAuditProcessingDependencyValidationException =
                new IngestionTrackingAuditProcessingDependencyValidationException(
                    message: "IngestionTrackingAudit processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IngestionTrackingAudit> ingestionTrackingAuditRetrieveByIdTask =
                this.ingestionTrackingAuditProcessingService.RetrieveIngestionTrackingAuditByIdAsync(someId);

            IngestionTrackingAuditProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingDependencyValidationException>(
                    ingestionTrackingAuditRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingDependencyValidationException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingDependencyValidationException))),
                         Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Guid someId = Guid.NewGuid();

            var expectedIngestionTrackingAuditProcessingDependencyException =
                new IngestionTrackingAuditProcessingDependencyException(
                    message: "IngestionTrackingAudit processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IngestionTrackingAudit> ingestionTrackingAuditRetrieveByIdTask =
                this.ingestionTrackingAuditProcessingService.RetrieveIngestionTrackingAuditByIdAsync(someId);

            IngestionTrackingAuditProcessingDependencyException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingDependencyException>(
                    ingestionTrackingAuditRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingDependencyException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingDependencyException))),
                         Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAsync()
        {
            // given
            Guid someId = Guid.NewGuid();

            var serviceException = new Exception();

            var failedIngestionTrackingAuditProcessingServiceException =
                new FailedIngestionTrackingAuditProcessingServiceException(
                    message: "Failed IngestionTrackingAudit processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingAuditProcessingServiveException =
                new IngestionTrackingAuditProcessingServiceException(
                    message: "IngestionTrackingAudit processing service error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditProcessingServiceException);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTrackingAudit> ingestionTrackingAuditRetrieveByIdTask =
                this.ingestionTrackingAuditProcessingService.RetrieveIngestionTrackingAuditByIdAsync(someId);

            IngestionTrackingAuditProcessingServiceException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingServiceException>(
                    ingestionTrackingAuditRetrieveByIdTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingServiveException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingServiveException))),
                         Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
