// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact(Skip = "Conversion to stream")]
        public async Task ShouldRetreiveMessagesForMatchingPdsWorkflowIdFromMeshAsync()
        {
            //// given
            //DateTimeOffset randomDate = GetRandomDateTimeOffset();
            //int randomNumber = GetRandomNumber();
            //List<string> randomMessageIds = GetRandomStrings(randomNumber);
            //string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            //List<MeshMessage> retrievedMessages = GetRandomMessages(randomMessageIds, mexWorkflowId);
            //Guid identifier = Guid.NewGuid();

            //this.dateTimeBrokerMock.Setup(broker =>
            //    broker.GetCurrentDateTimeOffset())
            //        .Returns(randomDate);

            //this.identifierBrokerMock.Setup(broker =>
            //    broker.GetIdentifier())
            //        .Returns(identifier);

            //this.meshServiceMock.SetupSequence(service =>
            //    service.RetrieveMessageIdsFromInboxAsync())
            //        .ReturnsAsync(randomMessageIds)
            //        .ReturnsAsync(new List<string>());

            //List<PdsAudit> pdsAuditsList = new List<PdsAudit>();

            //foreach (var message in retrievedMessages)
            //{
            //    this.meshServiceMock.Setup(service =>
            //        service.RetrieveMessageByIdAsync(message.MessageId))
            //            .ReturnsAsync(message);

            //    string filename = message.Headers["mex-filename"].FirstOrDefault();
            //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            //    string[] fileNameParts = fileNameWithoutExtension.Split('_');

            //    string fileNameOutput =
            //        $"{fileNameParts[1]}_{fileNameParts[2]}_{fileNameParts[0]}_{fileNameParts[3]}";

            //    fileNameOutput += Path.GetExtension(filename);

            //    Document document = new Document
            //    {
            //        FileName = $"{pdsConfiguration.OutputFolder}/{fileNameOutput}",
            //        DocumentData = message.FileContent,
            //    };

            //    this.documentServiceMock.Setup(broker =>
            //        broker.AddDocumentAsync(document, It.IsAny<string>()));

            //    Guid correlationId = Guid.Parse(message.Headers["mex-localid"].FirstOrDefault());

            //    PdsAudit pdsAudit = new PdsAudit
            //    {
            //        Id = identifier,
            //        CorrelationId = correlationId,
            //        FileName = document.FileName,
            //        Message = $"Received message from mesh with id {message.MessageId}",
            //        MessageId = message.MessageId,
            //        CreatedDate = randomDate,
            //        UpdatedDate = randomDate,
            //        CreatedBy = "System",
            //        UpdatedBy = "System"
            //    };

            //    this.pdsAuditServiceMock.Setup(service =>
            //        service.AddPdsAuditAsync(pdsAudit))
            //            .ReturnsAsync(pdsAudit);

            //    pdsAuditsList.Add(pdsAudit);
            //};

            //List<PdsAudit> expectedPdsAudits = pdsAuditsList.DeepClone();

            ////when
            //List<PdsAudit> actualPdsAudits =
            //    await this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            ////then
            //actualPdsAudits.Should().BeEquivalentTo(expectedPdsAudits);
            //pdsAuditsList.Count.Should().Be(retrievedMessages.Count);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffset(),
            //        Times.Exactly(retrievedMessages.Count));

            //this.identifierBrokerMock.Verify(broker =>
            //    broker.GetIdentifier(),
            //        Times.Exactly(retrievedMessages.Count));

            //this.meshServiceMock.Verify(service =>
            //    service.RetrieveMessageIdsFromInboxAsync(),
            //        Times.Exactly(2));

            //foreach (var message in retrievedMessages)
            //{
            //    this.meshServiceMock.Verify(service =>
            //        service.RetrieveMessageByIdAsync(message.MessageId),
            //            Times.Once);

            //    string filename = message.Headers["mex-filename"].FirstOrDefault();
            //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            //    string[] fileNameParts = fileNameWithoutExtension.Split('_');

            //    string fileNameOutput =
            //        $"{fileNameParts[1]}_{fileNameParts[2]}_{fileNameParts[0]}_{fileNameParts[3]}";

            //    fileNameOutput += Path.GetExtension(filename);

            //    Document document = new Document
            //    {
            //        FileName = $"{pdsConfiguration.OutputFolder}/{fileNameOutput}",
            //        DocumentData = message.FileContent,
            //    };

            //    this.documentServiceMock.Verify(service =>
            //        service.AddDocumentAsync(It.Is(SameDocumentAs(document)), It.IsAny<string>()),
            //            Times.Once);

            //    Guid correlationId = Guid.Parse(message.Headers["mex-localid"].FirstOrDefault());

            //    PdsAudit pdsAudit = new PdsAudit
            //    {
            //        Id = identifier,
            //        CorrelationId = correlationId,
            //        FileName = document.FileName,
            //        Message = $"Received message from mesh with id {message.MessageId}",
            //        MessageId = message.MessageId,
            //        CreatedDate = randomDate,
            //        UpdatedDate = randomDate,
            //        CreatedBy = "System",
            //        UpdatedBy = "System"
            //    };

            //    this.pdsAuditServiceMock.Verify(service =>
            //        service.AddPdsAuditAsync(It.Is(SamePdsAuditAs(pdsAudit))),
            //            Times.Once);

            //    pdsAuditsList.Add(pdsAudit);
            //};

            //this.meshServiceMock.VerifyNoOtherCalls();
            //this.documentServiceMock.VerifyNoOtherCalls();
            //this.dateTimeBrokerMock.VerifyNoOtherCalls();
            //this.identifierBrokerMock.VerifyNoOtherCalls();
            //this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetreiveMessagesForNonMatchingPdsWorkflowIdFromMeshAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            List<string> randomMessageIds = GetRandomStrings(randomNumber);
            string mexWorkflowId = GetRandomString();
            List<MeshMessage> retrievedMessages = GetRandomMessages(randomMessageIds, mexWorkflowId);

            this.meshServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(randomMessageIds)
                    .ReturnsAsync(new List<string>());

            List<PdsAudit> pdsAuditsList = new List<PdsAudit>();

            foreach (var message in retrievedMessages)
            {
                this.meshServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId))
                        .ReturnsAsync(message);

                if (message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.WorkflowId)
                {
                    continue;
                }
            };

            List<PdsAudit> expectedPdsAudits = pdsAuditsList.DeepClone();

            //when
            List<PdsAudit> actualPdsAudits =
                await this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            //then
            actualPdsAudits.Should().BeEquivalentTo(expectedPdsAudits);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                    Times.Exactly(2));

            foreach (var message in retrievedMessages)
            {
                this.meshServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId),
                        Times.Once);

                if (message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.WorkflowId)
                {
                    continue;
                }
            };

            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
