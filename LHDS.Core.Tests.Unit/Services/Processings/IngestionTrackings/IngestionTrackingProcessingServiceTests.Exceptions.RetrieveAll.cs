// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.IngestionTrackingAudits.Exceptions;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedIngestionTrackingProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependencyValidationException);

            // when
            Action ingestionTrackingRetrieveAction = () =>
                this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings();

            IngestionTrackingProcessingDependencyValidationException actualException =
                Assert.Throws<IngestionTrackingProcessingDependencyValidationException>(
                    ingestionTrackingRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyValidationException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyValidationException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedIngestionTrackingProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependencyException);

            // when
            Action ingestionTrackingRetrieveAction = () =>
                this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings();

            IngestionTrackingProcessingDependencyException actualException =
                Assert.Throws<IngestionTrackingProcessingDependencyException>(ingestionTrackingRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingProcessingServiceException =
                new FailedIngestionTrackingProcessingServiceException(
                    message: "Failed IngestionTracking processing service error occurred, contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingProcessingServiveException =
                new IngestionTrackingProcessingServiceException(
                    message: "IngestionTracking processing service error occurred, contact support.",
                    innerException: failedIngestionTrackingProcessingServiceException);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(serviceException);

            // when
            Action ingestionTrackingRetrieveAction = () =>
                this.ingestionTrackingProcessingService.RetrieveAllIngestionTrackings();

            IngestionTrackingProcessingServiceException actualException =
                Assert.Throws<IngestionTrackingProcessingServiceException>(ingestionTrackingRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingServiveException);

            this.ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingServiveException))),
                         Times.Once);

            this.ingestionTrackingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
