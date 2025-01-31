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
        public async Task ShouldThrowDependencyValidationExceptionOnModifyOrAddIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = someIngestionTrackingAudit;

            var expectedIngestionTrackingAuditProcessingDependencyValidationException =
                new IngestionTrackingAuditProcessingDependencyValidationException(
                    message: "IngestionTrackingAudit processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ThrowsAsync(dependencyValidationException);

            // when
            ValueTask<IngestionTrackingAudit> ingestionTrackingAuditModifyOrAddTask =
                this.ingestionTrackingAuditProcessingService
                    .ModifyOrAddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            IngestionTrackingAuditProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingDependencyValidationException>(
                    ingestionTrackingAuditModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingDependencyValidationException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
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
        public async Task ShouldThrowDependencyExceptionOnModifyOrAddIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = someIngestionTrackingAudit;

            var expectedIngestionTrackingAuditProcessingDependencyException =
                new IngestionTrackingAuditProcessingDependencyException(
                    message: "IngestionTrackingAudit processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ThrowsAsync(dependencyException);

            // when
            ValueTask<IngestionTrackingAudit> ingestionTrackingAuditModifyOrAddTask =
                this.ingestionTrackingAuditProcessingService
                    .ModifyOrAddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            IngestionTrackingAuditProcessingDependencyException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingDependencyException>(
                    ingestionTrackingAuditModifyOrAddTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingDependencyException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingDependencyException))),
                         Times.Once);

            this.ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyOrAddIfServiceErrorOccursAsync()
        {
            // given
            IngestionTrackingAudit someIngestionTrackingAudit = CreateRandomIngestionTrackingAudit();
            IngestionTrackingAudit inputIngestionTrackingAudit = someIngestionTrackingAudit;

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
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IngestionTrackingAudit> modifyOrAddIngestionTrackingAuditTask =
                this.ingestionTrackingAuditProcessingService
                    .ModifyOrAddIngestionTrackingAuditAsync(inputIngestionTrackingAudit);

            IngestionTrackingAuditProcessingServiceException actualException =
                await Assert.ThrowsAsync<IngestionTrackingAuditProcessingServiceException>(
                    modifyOrAddIngestionTrackingAuditTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingServiveException);

            this.ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveIngestionTrackingAuditByIdAsync(inputIngestionTrackingAudit.Id),
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
