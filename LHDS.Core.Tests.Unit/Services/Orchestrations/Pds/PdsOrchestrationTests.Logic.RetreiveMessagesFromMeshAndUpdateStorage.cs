// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Documents;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

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
            Guid identifier = Guid.NewGuid();

            this.dateTimeBrokerMock.Setup(service =>
                service.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.identifierBrokerMock.Setup(service =>
                service.GetIdentifier())
                    .Returns(identifier);

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
                    Id = identifier,
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

            this.dateTimeBrokerMock.Verify(service =>
                service.GetCurrentDateTimeOffset(),
                    Times.Exactly(retrievedMessages.Count));

            this.identifierBrokerMock.Verify(service =>
                service.GetIdentifier(),
                    Times.Exactly(retrievedMessages.Count));

            this.meshServiceMock.Verify(service =>
              service.RetrieveMessageIdsFromInboxAsync(),
                        Times.Once);

            foreach (var message in retrievedMessages)
            {

                this.meshServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId),
                        Times.Once);

                Document document = new Document
                {
                    FileName = message.Headers["Mex-FileName"].FirstOrDefault().ToString(),
                    DocumentData = message.FileContent,
                };

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Once);

                Guid correlationId = Guid.Parse(message.Headers["Mex-LocalID"].FirstOrDefault());
                string fileName = message.Headers["Mex-FileName"].FirstOrDefault();

                PdsAudit pdsAudit = new PdsAudit
                {
                    Id = identifier,
                    CorrelationId = correlationId,
                    FileName = fileName,
                    Message = $"Received message from mesh with id {message.MessageId}",
                    CreatedDate = randomDate,
                    UpdatedDate = randomDate,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                this.pdsAuditServiceMock.Verify(service =>
                    service.AddPdsAuditAsync(It.Is(SamePdsAuditAs(pdsAudit))),
                        Times.Once);

                pdsAuditsList.Add(pdsAudit);
            };

            this.meshServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetreiveMessagesForNonMatchingPdsWorkflowIdFromMeshAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            int randomNumber = GetRandomNumber();
            List<string> randomMessageIds = GetRandomStrings(randomNumber);
            string mexWorkflowId = GetRandomString();
            List<MeshMessage> retrievedMessages = GetRandomMessages(randomMessageIds, mexWorkflowId);
            Guid identifier = Guid.NewGuid();

            this.dateTimeBrokerMock.Setup(service =>
                service.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.identifierBrokerMock.Setup(service =>
                service.GetIdentifier())
                    .Returns(identifier);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(randomMessageIds);

            List<PdsAudit> pdsAuditsList = new List<PdsAudit>();

            foreach (var message in retrievedMessages)
            { 
                this.meshServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId))
                        .ReturnsAsync(message);

                if (message.Headers["Mex-WorkflowId"].FirstOrDefault() != this.pdsConfiguration.WorkflowId)
                {
                    continue;
                }

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
                    Id = identifier,
                    CorrelationId = correlationId,
                    FileName = fileName,
                    Message = $"Received message from mesh with id {message.MessageId}",
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

            this.dateTimeBrokerMock.Verify(service =>
                service.GetCurrentDateTimeOffset(),
                    Times.Exactly(retrievedMessages.Count));

            this.identifierBrokerMock.Verify(service =>
                service.GetIdentifier(),
                    Times.Exactly(retrievedMessages.Count));

            this.meshServiceMock.Verify(service =>
              service.RetrieveMessageIdsFromInboxAsync(),
                        Times.Once);

            foreach (var message in retrievedMessages)
            {

                this.meshServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId),
                        Times.Once);

                if (message.Headers["Mex-WorkflowId"].FirstOrDefault() != this.pdsConfiguration.WorkflowId)
                {
                    continue;
                }

                Document document = new Document
                {
                    FileName = message.Headers["Mex-FileName"].FirstOrDefault().ToString(),
                    DocumentData = message.FileContent,
                };

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(It.Is(SameDocumentAs(document))),
                        Times.Once);

                Guid correlationId = Guid.Parse(message.Headers["Mex-LocalID"].FirstOrDefault());
                string fileName = message.Headers["Mex-FileName"].FirstOrDefault();

                PdsAudit pdsAudit = new PdsAudit
                {
                    Id = identifier,
                    CorrelationId = correlationId,
                    FileName = fileName,
                    Message = $"Received message from mesh with id {message.MessageId}",
                    CreatedDate = randomDate,
                    UpdatedDate = randomDate,
                    CreatedBy = "System",
                    UpdatedBy = "System"
                };

                this.pdsAuditServiceMock.Verify(service =>
                    service.AddPdsAuditAsync(It.Is(SamePdsAuditAs(pdsAudit))),
                        Times.Once);

                pdsAuditsList.Add(pdsAudit);
            };

            this.meshServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
