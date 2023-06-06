// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
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
        public async Task ShouldRetreiveMessagesFromMeshAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            var randomString = GetRandomString();
            var inputString = randomString;
            var inputBytes = Encoding.ASCII.GetBytes(inputString);
            var fileName = GetRandomString();
            string batchReference = randomDate.ToString("yyyyMMddHHmmss");
            getrandommessage

            List<PdsAudit> pdsAuditsList= new List<PdsAudit>();

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync()
                    .

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

            this.meshServiceMock.Verify(service =>
              service.RetrieveMessageIdsFromInboxAsync(),
                        Times.Once);

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();


        }
    }
}
