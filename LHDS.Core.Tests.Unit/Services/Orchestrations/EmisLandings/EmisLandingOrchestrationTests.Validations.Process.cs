// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
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
        public async Task ShouldThrowValidationExceptionOnProcessIfConfigurationIsNullAndLogItAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            var somefileName = GetRandomString();
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Guid inputSupplierId = Guid.NewGuid();
            LandingConfiguration invalidLandingConfiguration = null;

            var invalidDownloadOrchestrationService = new EmisLandingOrchestrationService(
                documentProcessingService: documentProcessingServiceMock.Object,
                downloadProcessingService: downloadProcessingServiceMock.Object,
                ingestionTrackingProcessingService: ingestionTrackingProcessingServiceMock.Object,
                auditService: ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                hashBroker: hashBrokerMock.Object,
                fileBroker: fileBrokerMock.Object,
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
            ValueTask<List<string>> processTask = invalidDownloadOrchestrationService
                .ProcessAsync(subscriberCredential: inputSubscriberCredential, supplierId: inputSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessIfBlobContainersIsNullAndLogItAsync()
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
                auditService: ingestionTrackingAuditProcessingServiceMock.Object,
                dataSetSpecificationProcessingService: dataSetSpecificationProcessingServiceMock.Object,
                blobContainers: invalidBlobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                hashBroker: hashBrokerMock.Object,
                fileBroker: fileBrokerMock.Object,
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
            ValueTask<List<string>> processTask = invalidDownloadOrchestrationService
                .ProcessAsync(subscriberCredential: inputSubscriberCredential, supplierId: inputSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.hashBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessIfSupplierIdIsInvalidAndLogItAsync()
        {
            // given
            SubscriberCredential invalidSubscriberCredential = null;
            string randomFileName = GetRandomString();
            string inputFileName = randomFileName;
            Guid invalidSupplierId = Guid.Empty;

            var invalidArgumentEmisLandingOrchestrationException =
                new InvalidArgumentEmisLandingOrchestrationException(
                    message: "Invalid EMIS landing orchestration argument(s), please correct the errors and try again.");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "SubscriberCredential",
               values: "SubscriberCredential is required");

            invalidArgumentEmisLandingOrchestrationException.AddData(
               key: "SupplierId",
               values: "Id is required");

            var expectedEmisLandingOrchestrationValidationException =
                new EmisLandingOrchestrationValidationException(
                    message: "EMIS landing orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentEmisLandingOrchestrationException);

            // when
            ValueTask<List<string>> processTask =
                this.emisLandingOrchestrationService
                    .ProcessAsync(subscriberCredential: invalidSubscriberCredential, supplierId: invalidSupplierId);

            EmisLandingOrchestrationValidationException actualEmisLandingOrchestrationValidationException =
                await Assert.ThrowsAsync<EmisLandingOrchestrationValidationException>(processTask.AsTask);

            // then
            actualEmisLandingOrchestrationValidationException.Should()
                .BeEquivalentTo(expectedEmisLandingOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedEmisLandingOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.downloadProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ingestionTrackingProcessingServiceMock.VerifyNoOtherCalls();
            this.dataSetSpecificationProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.ingestionTrackingAuditProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
