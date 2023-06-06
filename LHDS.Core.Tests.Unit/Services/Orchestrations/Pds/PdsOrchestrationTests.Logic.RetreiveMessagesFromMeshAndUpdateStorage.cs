// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;
using static System.Net.Mime.MediaTypeNames;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task ShouldRetreiveMessagesForMatchingPdsWorkflowIdFromMeshAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            int randomNumber = GetRandomNumber();
            List<string> randomMessageIds = GetRandomStrings(randomNumber);
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            List<MeshMessage> retrievedMessages = GetRandomMessages(randomMessageIds, mexWorkflowId);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(randomMessageIds);

            List<PdsAudit> pdsAuditsList= new List<PdsAudit>();

            foreach (var message in retrievedMessages)
            {

                this.meshServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId))
                        .ReturnsAsync(message);

                Document document = new Document
                {
                    FileName = message.Headers["Mex-FileName"].FirstOrDefault().ToString(),
                    DocumentData = message.FileContent,
                };

                this.documentServiceMock.Setup(service =>
                    service.AddDocumentAsync(document));

                Guid correlationId = Guid.Parse(message.Headers["Mex-LocalID"].FirstOrDefault());
                string fileName = message.Headers["Mex-FileName"].FirstOrDefault();

                PdsAudit pdsAudit = new PdsAudit
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = correlationId,
                    FileName = fileName,
                    Message =  $"Received message from mesh with id {message.MessageId}",
                    CreatedDate = randomDate,
                    UpdatedDate = randomDate,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                this.pdsAuditServiceMock.Setup(service =>
                    service.AddPdsAuditAsync(pdsAudit))
                        .ReturnsAsync(pdsAudit);

                pdsAuditsList.Add(pdsAudit);
            };

            List<PdsAudit> expectedPdsAudits = pdsAuditsList.DeepClone();

            //when
            List<PdsAudit> actualPdsAudits =
                await this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            //then
            actualPdsAudits.Should().BeEquivalentTo(expectedPdsAudits);

            this.meshServiceMock.Verify(service =>
              service.RetrieveMessageIdsFromInboxAsync(),
                        Times.Once);

            foreach (var message in retrievedMessages)
            {

                this.meshServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId))
                        .ReturnsAsync(message);

                Document document = new Document
                {
                    FileName = message.Headers["Mex-FileName"].FirstOrDefault().ToString(),
                    DocumentData = message.FileContent,
                };

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(document),
                        Times.Once);

                Guid correlationId = Guid.Parse(message.Headers["Mex-LocalID"].FirstOrDefault());
                string fileName = message.Headers["Mex-FileName"].FirstOrDefault();

                PdsAudit pdsAudit = new PdsAudit
                {
                    Id = Guid.NewGuid(),
                    CorrelationId = correlationId,
                    FileName = fileName,
                    Message = $"Received message from mesh with id {message.MessageId}",
                    CreatedDate = randomDate,
                    UpdatedDate = randomDate,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                this.pdsAuditServiceMock.Verify(service =>
                    service.AddPdsAuditAsync(pdsAudit),
                        Times.Once);

                pdsAuditsList.Add(pdsAudit);
            };

            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
        }
    }
}
