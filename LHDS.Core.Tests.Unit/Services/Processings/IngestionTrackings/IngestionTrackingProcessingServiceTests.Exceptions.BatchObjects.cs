// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveObjectsInBatchIfErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            string someBatchReference = GetRandomString();

            var expectedIngestionTrackingProcessingDependencyValidationException =
                new IngestionTrackingProcessingDependencyValidationException(
                    message: "IngestionTracking processing dependency validation error occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveObjectsTask =
                this.ingestionTrackingProcessingService
                    .RetrieveObjectsInBatchByBatchReference(someBatchReference);

            IngestionTrackingProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingDependencyValidationException>(
                    retrieveObjectsTask.AsTask);

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
        public async Task ShouldThrowDependencyExceptionOnRetrieveObjectsInBatchIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            string someBatchReference = GetRandomString();

            var expectedIngestionTrackingProcessingDependencyException =
                new IngestionTrackingProcessingDependencyException(
                    message: "IngestionTracking processing dependency error occurred, please try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.ingestionTrackingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackings())
                    .Throws(dependencyException);

            // when
            ValueTask<List<string>> retrieveObjectsTask =
                this.ingestionTrackingProcessingService
                    .RetrieveObjectsInBatchByBatchReference(someBatchReference);

            IngestionTrackingProcessingDependencyException actualException =
                await Assert.ThrowsAsync<IngestionTrackingProcessingDependencyException>(
                    retrieveObjectsTask.AsTask);

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
    }
}
