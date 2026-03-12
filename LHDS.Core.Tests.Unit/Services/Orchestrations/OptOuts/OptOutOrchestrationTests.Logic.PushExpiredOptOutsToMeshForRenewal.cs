// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.ElementModel.Types;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldPushExpiredOptOutsToMeshForRenewalStatusAsync()
        {
            // Given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTime = randomDate;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            List<string> randomOptOuts = CreateRandomIdentifierList();
            List<string> outputOptOuts = randomOptOuts;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTime);

            this.optOutProcessingServiceMock.Setup(processing =>
                processing.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays))
                    .ReturnsAsync(outputOptOuts); 

            StringBuilder processedOutputString = new StringBuilder();

            foreach (var identifier in outputOptOuts)
            {
                processedOutputString.AppendLine($"{identifier},");
            }

            string csvFileContent = processedOutputString.ToString();
            string batchReference = randomDate.ToString("yyyyMMddHHmmss");
            string mexTo = this.optOutConfiguration.To;
            string mexWorkflowId = this.optOutConfiguration.WorkflowId;
            byte[] fileContent = Encoding.UTF8.GetBytes(csvFileContent);
            string mexSubject = string.Empty;
            string mexLocalId = batchReference;
            string mexFileName = $"{batchReference}.txt";
            string mexContentChecksum = string.Empty;
            string contentType = "text/plain";
            string contentEncoding = string.Empty;
            string accept = "application/json";

            MeshMessage outputMessage = ComposeMessage.CreateMeshMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding,
                accept);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept))
                        .ReturnsAsync(outputMessage);

            foreach (var identifier in outputOptOuts)
            {
                OptOut modifiedOptOut = new OptOut
                {
                    NhsNumber = identifier,
                    LastSentToMesh = currentDateTime,
                    UpdatedDate = currentDateTime,
                    BatchReference = batchReference
                };

                this.optOutProcessingServiceMock.Setup(processing =>
                    processing.AddOrModifyOptOutAsync(modifiedOptOut))
                        .ReturnsAsync(modifiedOptOut);
            }

            // When
            await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            // Then
            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays),
                    Times.Once);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept),
                        Times.Once);

            foreach (var identifier in outputOptOuts)
            {
                OptOut modifiedOptOut = new OptOut
                {
                    NhsNumber = identifier,
                    LastSentToMesh = currentDateTime,
                    UpdatedDate = currentDateTime,
                    BatchReference = batchReference
                };

                this.optOutProcessingServiceMock.Verify(processing =>
                    processing.AddOrModifyOptOutAsync(
                        It.Is<OptOut>(o =>
                            o.NhsNumber == identifier &&
                            o.LastSentToMesh == currentDateTime &&
                            o.UpdatedDate == currentDateTime &&
                            o.BatchReference == batchReference)),
                    Times.Once);
            }

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(outputOptOuts.Count + 1));

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotPushExpiredOptOutsToMeshForRenewalStatusIfNoExpiredRecordsAsync()
        {
            // Given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTime = randomDate;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            List<string> randomOptOuts = new List<string>();
            List<string> outputOptOuts = randomOptOuts;
            MeshMessage expectedMessage = null;

            var processedOutputString = GetRandomString();

            this.optOutProcessingServiceMock.Setup(processing =>
                processing.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays))
                    .ReturnsAsync(outputOptOuts);

            // When
            MeshMessage? actualMessage =
                await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            // Then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays),
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
