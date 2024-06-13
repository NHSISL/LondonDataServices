// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public void ShouldThrowDependencyValidationExceptionOnRetrieveAllIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            var expectedIngestionTrackingProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependencyValidationException);

            // when
            Action ingestionTrackingRetrieveAction = () =>
                ingestionTrackingProcessingService.RetrieveAllIngestionTrackings();

            IngestionTrackingProcessingDependencyValidationException actualException =
                Assert.Throws<IngestionTrackingProcessingDependencyValidationException>(
                    ingestionTrackingRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyValidationException);

            ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyValidationException))),
                         Times.Once);

            ingestionTrackingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public void ShouldThrowDependencyExceptionOnRetrieveAllIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            var expectedIngestionTrackingProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependencyException);

            // when
            Action ingestionTrackingRetrieveAction = () =>
                ingestionTrackingProcessingService.RetrieveAllIngestionTrackings();

            IngestionTrackingProcessingDependencyException actualException =
                Assert.Throws<IngestionTrackingProcessingDependencyException>(ingestionTrackingRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingDependencyException);

            ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingDependencyException))),
                         Times.Once);

            ingestionTrackingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAsync()
        {
            // given
            var serviceException = new Exception();

            var failedIngestionTrackingProcessingServiceException =
                new FailedIngestionTrackingProcessingServiceException(
                    message: "Failed IngestionTracking processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedIngestionTrackingProcessingServiveException =
                new IngestionTrackingProcessingServiceException(
                    message: "IngestionTracking processing service error occurred, please contact support.",
                    innerException: failedIngestionTrackingProcessingServiceException);

            ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(serviceException);

            // when
            Action ingestionTrackingRetrieveAction = () =>
                ingestionTrackingProcessingService.RetrieveAllIngestionTrackings();

            IngestionTrackingProcessingServiceException actualException =
                Assert.Throws<IngestionTrackingProcessingServiceException>(ingestionTrackingRetrieveAction);

            // then
            actualException.Should().BeEquivalentTo(expectedIngestionTrackingProcessingServiveException);

            ingestionTrackingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackings(),
                    Times.Once);

            loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedIngestionTrackingProcessingServiveException))),
                         Times.Once);

            ingestionTrackingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
