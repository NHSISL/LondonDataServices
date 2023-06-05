// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Text;
using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task ShouldPickupFileAndSendToMeshAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            var randomString = GetRandomString();
            var inputString = randomString;
            var inputBytes = Encoding.ASCII.GetBytes(inputString);
            var fileName = GetRandomString();
            string batchReference = randomDate.ToString("yyyyMMddHHmmss");

            PdsAudit pdsAudit = new PdsAudit
            {
                Id = Guid.NewGuid(),
                CorrelationId = Guid.NewGuid(),
                FileName = fileName,
                Message = "Sent",
                CreatedDate = randomDate,
                UpdatedDate = randomDate,
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            this.pdsAuditServiceMock.Setup(service =>
                service.AddPdsAuditAsync(pdsAudit));

            string mexTo = this.pdsConfiguration.To;
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            byte[] fileContent = inputBytes;
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

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(
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

            //when
            await this.pdsOrchestrationService.PickupFileAndSendToMesh(inputBytes, fileName);

            //then

            this.pdsAuditServiceMock.Verify(service =>
               service.AddPdsAuditAsync(It.Is(SamePdsAuditAs(pdsAudit))),
                   Times.Once);

            this.meshServiceMock.Verify(service =>
              service.SendMessageAsync(
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

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();


        }
    }
}
