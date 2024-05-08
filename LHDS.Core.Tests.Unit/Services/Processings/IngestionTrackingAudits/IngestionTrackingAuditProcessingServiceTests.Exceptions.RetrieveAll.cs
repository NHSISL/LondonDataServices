// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using FluentAssertions;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedIngestionTrackingAuditProcessingDependencyValidationException =
                new IngestionTrackingAuditProcessingDependencyValidationException(
                    message: "IngestionTrackingAudit processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingAudits())
                    .Throws(dependencyValidationException);

            // when
            Action ingestionTrackingAuditRetrieveAllAction = () =>
                ingestionTrackingAuditProcessingService.RetrieveAllIngestionTrackingAudits();

            IngestionTrackingAuditProcessingDependencyValidationException actualException =
                Assert.Throws<IngestionTrackingAuditProcessingDependencyValidationException>(
                    ingestionTrackingAuditRetrieveAllAction);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedIngestionTrackingAuditProcessingDependencyValidationException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingAudits(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingDependencyValidationException))),
                         Times.Once);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedIngestionTrackingAuditProcessingDependencyException =
                new IngestionTrackingAuditProcessingDependencyException(
                    message: "IngestionTrackingAudit processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingAudits())
                    .Throws(dependencyException);

            // when
            Action ingestionTrackingAuditRetrieveAllAction = () =>
                ingestionTrackingAuditProcessingService.RetrieveAllIngestionTrackingAudits();

            IngestionTrackingAuditProcessingDependencyException actualException =
                Assert.Throws<IngestionTrackingAuditProcessingDependencyException>(ingestionTrackingAuditRetrieveAllAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingDependencyException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingAudits(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingDependencyException))),
                         Times.Once);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingAuditProcessingServiceException =
                new FailedIngestionTrackingAuditProcessingServiceException(
                    message: "Failed IngestionTrackingAudit processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingAuditProcessingServiveException =
                new IngestionTrackingAuditProcessingServiceException(
                    message: "IngestionTrackingAudit processing service error occurred, please contact support.",
                    innerException: failedIngestionTrackingAuditProcessingServiceException);

            ingestionTrackingAuditServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingAudits())
                    .Throws(serviceException);

            // when
            Action ingestionTrackingAuditRetrieveAllAction = () =>
                ingestionTrackingAuditProcessingService.RetrieveAllIngestionTrackingAudits();

            IngestionTrackingAuditProcessingServiceException actualException =
                Assert.Throws<IngestionTrackingAuditProcessingServiceException>(
                    ingestionTrackingAuditRetrieveAllAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingAuditProcessingServiveException);

            ingestionTrackingAuditServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingAudits(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingAuditProcessingServiveException))),
                         Times.Once);

            ingestionTrackingAuditServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
