// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using LHDS.Core.Services.Orchestrations.Pds;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfConfigurationIsNullOnPickupAndLogItAsync()
        {
            // Given
            byte[] somePdsFile = Encoding.UTF8.GetBytes(GetRandomString());
            string someFileName = GetRandomString();
            PdsConfiguration invalidPdsConfiguration = null;

            var invalidPdsOrchestrationService = new PdsOrchestrationService(
                pdsAuditService: pdsAuditServiceMock.Object,
                documentService: documentServiceMock.Object,
                meshService: meshServiceMock.Object,
                blobContainers: blobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                pdsConfiguration: invalidPdsConfiguration);

            var nullConfigPdsOrchestrationException =
                new NullConfigPdsOrchestrationException(
                    message: "Null configuration PDS orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedPdsOrchestrationValidationException =
                new PdsOrchestrationValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException: nullConfigPdsOrchestrationException);

            // When
            ValueTask<PdsAudit> PickupFileAndSendToMeshTask =
                invalidPdsOrchestrationService.PickupFileAndSendToMesh(pdsFile: somePdsFile, fileName: someFileName);

            PdsOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationValidationException>(
                    PickupFileAndSendToMeshTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionIfBlobContainersIsNullonPickupAndLogItAsync()
        {
            // Given
            byte[] somePdsFile = Encoding.UTF8.GetBytes(GetRandomString());
            string someFileName = GetRandomString();
            BlobContainers invalidBlobContainers = null;

            var invalidPdsOrchestrationService = new PdsOrchestrationService(
                pdsAuditService: pdsAuditServiceMock.Object,
                documentService: documentServiceMock.Object,
                meshService: meshServiceMock.Object,
                blobContainers: invalidBlobContainers,
                loggingBroker: loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object,
                identifierBroker: identifierBrokerMock.Object,
                pdsConfiguration: pdsConfiguration);

            var nullBlobContainersPdsOrchestrationException =
                new NullBlobContainersPdsOrchestrationException(
                    message: "Null blob container PDS orchestration exception, " +
                        "please correct the errors and try again.");

            var expectedPdsOrchestrationValidationException =
                new PdsOrchestrationValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException: nullBlobContainersPdsOrchestrationException);

            // When
            ValueTask<PdsAudit> PickupFileAndSendToMeshTask =
                invalidPdsOrchestrationService.PickupFileAndSendToMesh(pdsFile: somePdsFile, fileName: someFileName);

            PdsOrchestrationValidationException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationValidationException>(
                    PickupFileAndSendToMeshTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsOrchestrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnPickupFileAndSendIfArgsAreInvalidAndLogItAsync(
            string invalidText)
        {
            byte[] pdsFile = null;
            string fileName = invalidText;

            var invalidArgumentPdsException =
               new InvalidArgumentPdsException(
                   message: "Invalid PDS argument(s), please correct the errors and try again.");

            invalidArgumentPdsException.AddData(
                key: "pdsFile",
                values: "Data is required");

            invalidArgumentPdsException.AddData(
                key: "fileName",
                values: "Text is required");

            var expectedPdsValidationException =
                new PdsOrchestrationValidationException(
                    message: "PDS orchestration validation errors occurred, please try again.",
                    innerException: invalidArgumentPdsException);

            // when
            ValueTask<PdsAudit> PickupFileAndSendToMeshTask =
                this.pdsOrchestrationService.PickupFileAndSendToMesh(pdsFile, fileName);

            PdsOrchestrationValidationException actualPdsValidationException =
                await Assert.ThrowsAsync<PdsOrchestrationValidationException>(PickupFileAndSendToMeshTask.AsTask);

            // then
            actualPdsValidationException.Should()
                .BeEquivalentTo(expectedPdsValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedPdsValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
