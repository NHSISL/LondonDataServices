// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using LHDS.Core.Services.Orchestrations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfConfigurationIsNullAndLogItAsync()
        {
            // Given
            OptOutConfiguration invalidOptOutConfiguration = null;

            var invalidOptOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: this.optOutProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                meshProcessingService: this.meshProcessingServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                blobContainers: this.blobContainers,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                optOutConfiguration: invalidOptOutConfiguration,
                meshConfiguration: this.meshConfiguration);

            var nullConfigOptOutOrchestrationException =
                new NullConfigOptOutOrchestrationException(
                    message: "Null configuration opt out orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: nullConfigOptOutOrchestrationException);

            // When
            ValueTask<MeshMessage> pushExpOptOutsToMeshIfExpiredTask =
                invalidOptOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationValidationException>(pushExpOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionIfBlobContainersIsNullAndLogItAsync()
        {
            // Given
            BlobContainers invalidBlobContainers = null;

            var invalidOptOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: this.optOutProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                meshProcessingService: this.meshProcessingServiceMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                blobContainers: invalidBlobContainers,
                loggingBroker: this.loggingBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                optOutConfiguration: this.optOutConfiguration,
                meshConfiguration: this.meshConfiguration);

            var nullBlobContainersOptOutOrchestrationException =
                new NullBlobContainersOptOutOrchestrationException(
                    message: "Null blob container opt out orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedOptOutOrchestrationValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: nullBlobContainersOptOutOrchestrationException);

            // When
            ValueTask<MeshMessage> pushExpOptOutsToMeshIfExpiredTask =
                invalidOptOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationValidationException>(
                    pushExpOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutOrchestrationValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionIfConfigurationSettingsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // Given
            var invalidOptOutConfiguration = new OptOutConfiguration
            {
                ExpiredAfterDays = 2,
                InputFolder = invalidText,
                OptOutFileHasHeader = false,
                OutputFolder = invalidText,
                OptOutFileRequireTrailingComma = true,
            };

            BlobContainers blobContainers = new BlobContainers
            {
                EmisLanding = "emislanding",
                Versioner = "versioner",
                Ingress = "ingress",
                OptOut = "optout",
                Pds = "pds",
            };

            var invalidOptOutOrchestrationService = new OptOutOrchestrationService(
                optOutProcessingService: this.optOutProcessingServiceMock.Object,
                documentProcessingService: this.documentProcessingServiceMock.Object,
                meshProcessingService: this.meshProcessingServiceMock.Object,
                blobContainers,
                loggingBroker: this.loggingBrokerMock.Object,
                csvHelperBroker: this.csvHelperBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                optOutConfiguration: invalidOptOutConfiguration,
                meshConfiguration: this.meshConfiguration);

            var invalidConfigOptOutOrchestrationException =
                new InvalidConfigOptOutOrchestrationException(
                    message: "Invalid Configuration orchestration error, please correct the errors and try again.");

            invalidConfigOptOutOrchestrationException.AddData(
                key: nameof(OptOutConfiguration.OutputFolder),
                values: "Text is required");

            invalidConfigOptOutOrchestrationException.AddData(
               key: nameof(OptOutConfiguration.ExpiredAfterDays),
               values: "Value is required");

            var expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: invalidConfigOptOutOrchestrationException);

            // When
            ValueTask<MeshMessage> pushExpOptOutsToMeshIfExpiredTask =
                invalidOptOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationValidationException>(
                    pushExpOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPushExpiredOptOutsToMeshIfExpiredOrchestrationOptOutFileValidationException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
