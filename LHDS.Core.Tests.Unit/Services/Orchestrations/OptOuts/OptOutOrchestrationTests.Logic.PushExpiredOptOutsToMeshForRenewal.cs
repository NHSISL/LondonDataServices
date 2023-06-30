// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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
            bool withHeader = false;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            List<OptOut> randomOptOuts = CreateRandomOptOutsList();
            List<OptOut> outputOptOuts = randomOptOuts;

            List<OptOutIdentifier> mappedOptOuts = outputOptOuts
                .Select(optout => new OptOutIdentifier
                {
                    NhsNumber = optout.NhsNumber,
                    UniqueReference = optout.UniqueReference,
                    Status = optout.Status
                }).ToList();

            var processedOutputString = GetRandomString();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.optOutProcessingServiceMock.Setup(processing =>
                processing.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays))
                    .ReturnsAsync(outputOptOuts);

            string batchReference = randomDate.ToString("yyyyMMddHHmmss");

            this.csvMapperProcessingServiceMock.Setup(processings =>
                processings.MapObjectToCsvAsync(
                    It.Is(SameOptOutIdentifierListAs(mappedOptOuts)), withHeader, shouldAddTrailingComma))
                        .ReturnsAsync(processedOutputString);

            string mexTo = this.optOutConfiguration.To;
            string mexWorkflowId = this.optOutConfiguration.WorkflowId;
            byte[] fileContent = Encoding.UTF8.GetBytes(processedOutputString);
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

            foreach (var optOut in outputOptOuts)
            {
                optOut.LastSentToMesh = currentDateTime;
                optOut.UpdatedDate = currentDateTime;
                optOut.BatchReference = batchReference;

                this.optOutProcessingServiceMock.Setup(processing =>
                    processing.AddOrModifyOptOutAsync(optOut))
                        .ReturnsAsync(optOut);
            }

            // When
            await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            // Then
            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays),
                    Times.Once);

            this.csvMapperProcessingServiceMock.Verify(processings =>
                processings.MapObjectToCsvAsync(It.IsAny<List<OptOutIdentifier>>(), withHeader, shouldAddTrailingComma),
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

            foreach (var optOut in outputOptOuts)
            {
                optOut.LastSentToMesh = currentDateTime;
                optOut.UpdatedDate = currentDateTime;
                optOut.BatchReference = batchReference;

                this.optOutProcessingServiceMock.Verify(processing =>
                    processing.AddOrModifyOptOutAsync(optOut),
                        Times.Once());
            }

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldNotPushExpiredOptOutsToMeshForRenewalStatusIfNoExpiredRecordsAsync()
        {
            // Given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            DateTimeOffset currentDateTime = randomDate;
            bool withHeader = false;
            bool shouldAddTrailingComma = optOutConfiguration.OptOutFileRequireTrailingComma;
            List<OptOut> randomOptOuts = new List<OptOut>();
            List<OptOut> outputOptOuts = randomOptOuts;
            MeshMessage expectedMessage = null;

            var processedOutputString = GetRandomString();

            this.optOutProcessingServiceMock.Setup(processing =>
                processing.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays))
                    .ReturnsAsync(outputOptOuts);

            // When
            MeshMessage actualMessage = await this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            // Then
            actualMessage.Should().BeEquivalentTo(expectedMessage);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(optOutConfiguration.ExpiredAfterDays),
                    Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}
