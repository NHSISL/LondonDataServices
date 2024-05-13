// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Downloads;
using LHDS.Core.Models.Orchestrations.EmisLandings;
using LHDS.Core.Models.Orchestrations.EmisLandings.Exceptions;
using LHDS.Core.Models.Processings.SubscriberCredentials;
using LHDS.Core.Services.Orchestrations.Downloads;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.EmisLandings
{
    public partial class EmisLandingOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfConfigurationIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            var somefileName = GetRandomMessage();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Guid inputSupplierId = Guid.NewGuid();
            LandingConfiguration invalidLandingConfiguration = null;

            var invalidDownloadOrchestrationService = new EmisLandingOrchestrationService(
                documentProcessingService: documentProcessingServiceMock.Object,
                downloadProcessingService: downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingService: ingestionTrackingProcessingServiceMock.Object,
                auditService: auditServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                hashBroker: hashBrokerMock.Object,
                landingConfiguration: invalidLandingConfiguration);

            var nullLandingConfigurationDownloadOrchestrationException =
                new NullLandingConfigurationEmisLandingOrchestrationException(
                    message: "Null landing configuration EMIS landing orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedEmisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: nullLandingConfigurationDownloadOrchestrationException);

            // when
            ValueTask<string> processTask = invalidDownloadOrchestrationService
                .ProcessFileAsync(
                    ftpFileName: inputFileName,
                    subscriberCredential: inputSubscriberCredential,
                    supplierId: inputSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfSubscriptionCredentialIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential inputSubscriberCredential = null;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Guid inputSupplierId = Guid.NewGuid();

            var nullSubscriberCredentialEmisLandingOrchestrationException =
                new NullSubscriberCredentialEmisLandingOrchestrationException(
                    message: "Null subscriber credential EMIS landing orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedEmisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: nullSubscriberCredentialEmisLandingOrchestrationException);

            // when
            ValueTask<string> processTask =
                this.emisLandingOrchestrationService
                    .ProcessFileAsync(
                        ftpFileName: inputFileName,
                        subscriberCredential: inputSubscriberCredential,
                        supplierId: inputSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfBlobContainersIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Guid inputSupplierId = Guid.NewGuid();
            BlobContainers invalidBlobContainers = null;

            var invalidDownloadOrchestrationService = new EmisLandingOrchestrationService(
                documentProcessingService: documentProcessingServiceMock.Object,
                downloadProcessingService: downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingService: ingestionTrackingProcessingServiceMock.Object,
                auditService: auditServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers: invalidBlobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                hashBroker: hashBrokerMock.Object,
                landingConfiguration: landingConfiguration);

            var nullBlobContainersEmisLandingOrchestrationException =
                new NullBlobContainersEmisLandingOrchestrationException(
                    message: "Null blob container EMIS landing orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedEmisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: nullBlobContainersEmisLandingOrchestrationException);

            // when
            ValueTask<string> DownloadTask = invalidDownloadOrchestrationService
                .ProcessFileAsync(
                    ftpFileName: inputFileName,
                    subscriberCredential: inputSubscriberCredential,
                    supplierId: inputSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(DownloadTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessFileIfFileNameIsNullAndLogItAsync(string invalidText)
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string inputFileName = invalidText;
            Guid invalidSupplierId = Guid.Empty;

            var invalidArgumentEmisLandingOrchestrationException =
                new InvalidArgumentEmisLandingOrchestrationException(
                    message: "Invalid EMIS landing orchestration argument(s), " +
                        "please correct the errors and try again.");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "FileName",
               values: "Text is required");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "SupplierId",
               values: "Id is required");

            var expectedDownloadOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentEmisLandingOrchestrationException);

            // when
            ValueTask<string> processTask =
                this.emisLandingOrchestrationService
                    .ProcessFileAsync(
                        ftpFileName: inputFileName,
                        subscriberCredential: inputSubscriberCredential,
                        supplierId: invalidSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
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
            this.hashBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotProcessNamedDocumentOnProcessFileIfDownloadIsNullAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Guid inputSupplierId = Guid.NewGuid();

            Download inputDownload = new Download
            {
                SubscriberCredential = inputSubscriberCredential,
                Document = new Document { FileName = inputFileName }
            };

            var notFoundEmisLandingOrchestrationException =
                new NotFoundEmisLandingOrchestrationException(
                message: $"Couldn't find download with file name: {inputFileName}.");

            var expectedEmisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: notFoundEmisLandingOrchestrationException);

            this.downloadProcessingServiceMock.Setup(service =>
                    service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))))
                        .Returns(null);

            // when
            ValueTask<string> processTask = this.emisLandingOrchestrationService
                .ProcessFileAsync(
                    ftpFileName: inputFileName,
                    subscriberCredential: inputSubscriberCredential,
                    supplierId: inputSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.downloadProcessingServiceMock.Verify(service =>
                service.RetrieveDownloadByFileNameAsync(It.Is(SameDownloadAs(inputDownload))),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
        }
    }
}
