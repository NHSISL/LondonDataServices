// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Orchestrations.TppLandings.Exceptions;
using LHDS.Core.Services.Orchestrations.Tpp;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.TppLandings
{
    public partial class TppLandingOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(TppDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnReProcessIfDependencyValidationOccursAndLogItAsync(
             Xeption dependancyValidationException)
        {
            // given
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            var expectedDependencyException =
                new TppLandingOrchestrationDependencyValidationException(

                    message: "TPP landing orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",

                    dependancyValidationException.InnerException as Xeption);

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ThrowsAsync(dependancyValidationException);

            // when
            ValueTask processTask = tppOrchestrationServiceMock.Object
                .ReProcessAsync(supplierId: randomSupplierId);

            TppLandingOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationDependencyValidationException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                       Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(TppDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnReProcessIfDependencyExceptionOccursAndLogItAsync(
          Xeption dependancyException)
        {
            // given
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;
            Stream randomStream = new MemoryStream(CreateRandomData());
            Stream inputStream = randomStream;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            var expectedDependencyException =
                new TppLandingOrchestrationDependencyException(
                    message: "TPP landing orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask processTask = this.tppOrchestrationService
                .ReProcessAsync(supplierId: randomSupplierId);

            TppLandingOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationDependencyException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedDependencyException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnReProcessIfServiceErrorOccursAndLogItAsync()
        {
            //Given
            Guid randomSupplierId = Guid.NewGuid();
            Guid inputSupplierId = randomSupplierId;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            var serviceException = new Exception();

            var failedTppOrchestrationServiceException =
                new FailedTppLandingOrchestrationServiceException(
                    message: "Failed TPP landing orchestration service error occurred, please contact support.",
                    serviceException);

            var expectedTppOrchestrationServiceException =
                new TppLandingOrchestrationServiceException(
                    message: "TPP landing orchestration service error occurred, please contact support.",
                    failedTppOrchestrationServiceException);

            var tppOrchestrationServiceMock = new Mock<TppLandingOrchestrationService>(
                documentProcessingServiceMock.Object,
                ingestionTrackingProcessingServiceMock.Object,
                ingestionTrackingProcessingAuditServiceMock.Object,
                dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBrokerMock.Object,
                dateTimeBrokerMock.Object,
                identifierBrokerMock.Object,
                hashBrokerMock.Object,
                fileBrokerMock.Object,
                landingConfiguration)
            {
                CallBase = true
            };

            this.ingestionTrackingProcessingServiceMock.Setup(service =>
                service.RetrieveAllIngestionTrackingsAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask processTask = this.tppOrchestrationService
                .ReProcessAsync(supplierId: randomSupplierId);

            TppLandingOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<TppLandingOrchestrationServiceException>(processTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedTppOrchestrationServiceException);

            this.ingestionTrackingProcessingServiceMock.Verify(service =>
                service.RetrieveAllIngestionTrackingsAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTppOrchestrationServiceException))),
                        Times.Once);

            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingAuditServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}