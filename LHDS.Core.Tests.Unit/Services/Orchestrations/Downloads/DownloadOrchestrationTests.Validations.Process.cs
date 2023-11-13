// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Orchestrations.Downloads;
using LHDS.Core.Models.Orchestrations.Downloads.Exceptions;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Downloads
{
    public partial class DownloadOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfConfigurationIsNullAndLogItAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            LandingConfiguration invalidLandingConfiguration = null;

            var invalidDownloadOrchestrationService = new DownloadOrchestrationService(
                documentProcessingService: documentProcessingServiceMock.Object,
                downloadProcessingService: downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingService: ingestionTrackingProcessingServiceMock.Object,
                auditService: auditServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                landingConfiguration: invalidLandingConfiguration);

            var nullLandingConfigurationDownloadOrchestrationException =
                new NullLandingConfigurationDownloadOrchestrationException(
                    message: "Null landing configuration download orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedDownloadOrchestrationValidationException =
                new DownloadOrchestrationValidationException(
                    message: "Download orchestration validation errors occurred, please try again.",
                    innerException: nullLandingConfigurationDownloadOrchestrationException);

            // when
            ValueTask<string> DownloadTask = invalidDownloadOrchestrationService.ProcessAsync(inputFileName);

            DownloadOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDownloadOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionIfBlobContainersIsNullAndLogItAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            BlobContainers invalidBlobContainers = null;

            var invalidDownloadOrchestrationService = new DownloadOrchestrationService(
                documentProcessingService: documentProcessingServiceMock.Object,
                downloadProcessingService: downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingService: ingestionTrackingProcessingServiceMock.Object,
                auditService: auditServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers: invalidBlobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                landingConfiguration: landingConfiguration);

            var nullBlobContainersDownloadOrchestrationException =
                new NullBlobContainersDownloadOrchestrationException(
                    message: "Null blob container download orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedDownloadOrchestrationValidationException =
                new DownloadOrchestrationValidationException(
                    message: "Download orchestration validation errors occurred, please try again.",
                    innerException: nullBlobContainersDownloadOrchestrationException);

            // when
            ValueTask<string> DownloadTask = invalidDownloadOrchestrationService.ProcessAsync(inputFileName);

            DownloadOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDownloadOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnDownloadIfFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            var invalidArgumentDownloadOrchestrationException =
                new InvalidArgumentDownloadOrchestrationException(
                    message: "Invalid download orchestration argument(s), please correct the errors and try again.");

            invalidArgumentDownloadOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            var expectedDownloadOrchestrationValidationException =
                new DownloadOrchestrationValidationException(
                    message: "Download orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentDownloadOrchestrationException);

            // when
            ValueTask<string> DownloadTask =
                this.downloadOrchestrationService.ProcessAsync(invalidText);

            DownloadOrchestrationValidationException actualDownloadOrchestrationValidationException =
                await Assert.ThrowsAsync<DownloadOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            actualDownloadOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedDownloadOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessNamedDocumentsIfDownloadIsNullAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;

            var notFoundDownloadOrchestrationException =
                new NotFoundDownloadOrchestrationException(
                message: $"Couldn't find download with file name: {inputFileName}.");

            var expectedDownloadOrchestrationValidationException =
                new DownloadOrchestrationValidationException(
                    message: "Download orchestration validation errors occurred, please try again.",
                    innerException: notFoundDownloadOrchestrationException);

            this.downloadProcessingServiceMock.Setup(service =>
                  service.RetrieveDownloadByFileNameAsync(inputFileName))
                      .Returns(null);

            // when
            ValueTask<string> DownloadTask = this.downloadOrchestrationService.ProcessAsync(inputFileName);

            DownloadOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<DownloadOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(inputFileName),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDownloadOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
