// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Audits;
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
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            Audit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit inputIngestionTrackingAudit = someIngestionTrackingAudit;

            var expectedIngestionTrackingAuditProcessingDependencyValidationException =
                new IngestionTrackingAuditProcessingDependencyValidationException(
                    message: "IngestionTrackingAudit processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<Audit> ingestionTrackingAuditAddTask =
                this.ingestionTrackingAuditProcessingService.AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            IngestionTrackingAuditProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingDependencyValidationException>(
                    ingestionTrackingAuditAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(
                expectedIngestionTrackingAuditProcessingDependencyValidationException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingDependencyValidationException))),
                         Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Audit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit inputIngestionTrackingAudit = someIngestionTrackingAudit;

            var expectedIngestionTrackingAuditProcessingDependencyException =
                new IngestionTrackingAuditProcessingDependencyException(
                    message: "IngestionTrackingAudit processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit))
                    .Throws(dependencyException);

            // when
            ValueTask<Audit> ingestionTrackingAuditAddTask =
                this.ingestionTrackingAuditProcessingService
                    .AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            IngestionTrackingAuditProcessingDependencyException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingDependencyException>(
                    ingestionTrackingAuditAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingDependencyException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingDependencyException))),
                         Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAsync()
        {
            // given
            Audit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            Audit inputIngestionTrackingAudit = someIngestionTrackingAudit;

            var serviceException = new Exception();

            var failedIngestionTrackingAuditProcessingServiceException =
                new FailedIngestionTrackingAuditProcessingServiceException(
                    message: "Failed IngestionTrackingAudit processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingAuditProcessingServiveException =
                new IngestionTrackingAuditProcessingServiceException(
                    message: "IngestionTrackingAudit processing service error occurred, contact support.",
                    innerException: failedIngestionTrackingAuditProcessingServiceException);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit))
                    .Throws(serviceException);

            // when
            ValueTask<Audit> addIngestionTrackingTask =
                this.ingestionTrackingAuditProcessingService
                    .AddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            IngestionTrackingAuditProcessingServiceException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingServiceException>(addIngestionTrackingTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingServiveException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.AddAuditAsync(inputIngestionTrackingAudit),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingServiveException))),
                         Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
