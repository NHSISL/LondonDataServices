// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
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
        [Fact]
        public async Task ShouldRetreiveMessagesForMatchingPdsWorkflowIdFromMeshAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            int randomNumber = 1; // GetRandomNumber();
            List<string> randomMessageIds = GetRandomStrings(randomNumber);
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            string mexReturnWorkflowId = this.pdsConfiguration.ReturnWorkflowId;
            List<MeshMessage> retrievedMessages = GetRandomMessages(randomMessageIds, mexWorkflowId);
            string randomContainer = GetRandomString();
            string inputContainer = "pds";
            Guid identifier = Guid.NewGuid();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDate);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(identifier);

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

                string filename = message.Headers["mex-filename"].FirstOrDefault();
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                string[] fileNameParts = fileNameWithoutExtension.Split('_');

                string fileNameOutput =
                    $"{fileNameParts[1]}_{fileNameParts[2]}_{fileNameParts[0]}_{fileNameParts[3]}";

                fileNameOutput += Path.GetExtension(filename);
                string inputFileName = $"{pdsConfiguration.OutputFolder}/{fileNameOutput}";
                Stream inputStream = new MemoryStream(message.FileContent);
                Guid correlationId = Guid.Parse(message.Headers["mex-localid"].FirstOrDefault());

                IQueryable<PdsAudit> pdsAuditsWithCorrelationId = CreateRandomPdsAuditsWithCorrelationId(correlationId);

                this.pdsAuditServiceMock
                    .Setup(service =>
                        service.RetrieveAllPdsAuditsByCorrelationIdAsync(correlationId))
                    .ReturnsAsync(pdsAuditsWithCorrelationId);

                this.documentServiceMock
                    .Setup(service =>
                        service.AddDocumentAsync(It.Is(SameStreamAs(inputStream)), inputFileName, inputContainer))
                    .Returns(ValueTask.CompletedTask);

                PdsAudit pdsAudit = new PdsAudit
                {
                    Id = identifier,
                    CorrelationId = correlationId,
                    FileName = inputFileName,
                    Message = $"Received message from mesh with id {message.MessageId}",
                    MessageId = message.MessageId,
                    CreatedDate = randomDate,
                    UpdatedDate = randomDate,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    RequestType = "Response",
                    IsCompleted = true
                };

                this.pdsAuditServiceMock.Setup(service =>
                    service.AddPdsAuditAsync(pdsAudit))
                        .ReturnsAsync(pdsAudit);

                pdsAuditsList.Add(pdsAudit);
            }
            ;

            List<PdsAudit> expectedPdsAudits = pdsAuditsList.DeepClone();

            //when
            List<PdsAudit> actualPdsAudits =
                await this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            //then
            actualPdsAudits.Should().BeEquivalentTo(expectedPdsAudits);
            pdsAuditsList.Count.Should().Be(retrievedMessages.Count);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(retrievedMessages.Count));

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(retrievedMessages.Count));

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                    Times.Exactly(2));

            foreach (var message in retrievedMessages)
            {
                this.meshServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(message.MessageId),
                        Times.Once);

                string filename = message.Headers["mex-filename"].FirstOrDefault();
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                string[] fileNameParts = fileNameWithoutExtension.Split('_');

                string fileNameOutput =
                    $"{fileNameParts[1]}_{fileNameParts[2]}_{fileNameParts[0]}_{fileNameParts[3]}";

                fileNameOutput += Path.GetExtension(filename);
                string inputFileName = $"{pdsConfiguration.OutputFolder}/{fileNameOutput}";
                Stream inputStream = new MemoryStream(message.FileContent);
                Guid correlationId = Guid.Parse(message.Headers["mex-localid"].FirstOrDefault());

                this.pdsAuditServiceMock.Verify(service =>
                    service.RetrieveAllPdsAuditsByCorrelationIdAsync(correlationId),
                    Times.Once);

                this.documentServiceMock.Verify(service =>
                    service.AddDocumentAsync(It.IsAny<Stream>(), inputFileName, inputContainer),
                        Times.Once);


                PdsAudit pdsAudit = new PdsAudit
                {
                    Id = identifier,
                    CorrelationId = correlationId,
                    FileName = inputFileName,
                    Message = $"Received message from mesh with id {message.MessageId}",
                    MessageId = message.MessageId,
                    CreatedDate = randomDate,
                    UpdatedDate = randomDate,
                    CreatedBy = "System",
                    UpdatedBy = "System",
                    RequestType = "Response",
                    IsCompleted = true
                };

                this.pdsAuditServiceMock.Verify(service =>
                    service.AddPdsAuditAsync(It.Is(SamePdsAuditAs(pdsAudit))),
                        Times.Once);

                this.meshServiceMock.Verify(service =>
                    service.AcknowledgeMessageByIdAsync(message.MessageId),
                        Times.Exactly(1));

                pdsAuditsList.Add(pdsAudit);
            }
            ;

            this.pdsAuditServiceMock.Verify(service =>
                service.ModifyPdsAuditAsync(It.IsAny<PdsAudit>()),
                    Times.Exactly(retrievedMessages.Count));

            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
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

                if (message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.WorkflowId ||
                    message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.ReturnWorkflowId)
                {
                    continue;
                }
            }
            ;

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

                if (message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.WorkflowId ||
                    message.Headers["mex-workflowid"].FirstOrDefault() != this.pdsConfiguration.ReturnWorkflowId)
                {
                    continue;
                }
            }
            ;

            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}