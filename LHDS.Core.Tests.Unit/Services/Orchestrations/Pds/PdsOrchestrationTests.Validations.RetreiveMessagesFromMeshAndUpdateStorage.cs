// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Brokers.Storages.Blobs;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using LHDS.Core.Services.Orchestrations.Pds;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionIfBlobContainersIsNullOnRetrieveAndLogItAsync()
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
            ValueTask<List<PdsAudit>> PickupFileAndSendToMeshTask =
                invalidPdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

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
    }
}
