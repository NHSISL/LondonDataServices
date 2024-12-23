// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Text;
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
        public async Task ShouldPickupFileAndSendToMeshAsync()
        {
            // given
            Guid identifier = Guid.NewGuid();
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            var randomString = GetRandomString();
            var inputString = randomString;
            var inputBytes = Encoding.UTF8.GetBytes(inputString);
            var fileName = GetRandomString();

            string mexTo = this.pdsConfiguration.To;
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            byte[] fileContent = inputBytes;
            string mexSubject = string.Empty;
            string mexLocalId = identifier.ToString();
            string mexFileName = fileName;
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

            outputMessage.MessageId = identifier.ToString();

            PdsAudit randomPdsAudit =
                GetRandomPdsAudit(identifier, identifier, fileName, randomDate, outputMessage.MessageId);

            PdsAudit inputPdsAudit = randomPdsAudit;
            PdsAudit outputPdsAudit = inputPdsAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(identifier);

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

            this.pdsAuditServiceMock.Setup(service =>
               service.AddPdsAuditAsync(It.Is(SamePdsAuditAs(inputPdsAudit))))
                    .ReturnsAsync(outputPdsAudit);

            PdsAudit expectedPdsAudit = outputPdsAudit.DeepClone();

            //when
            PdsAudit actualPdsAudit =
                await this.pdsOrchestrationService.PickupFileAndSendToMesh(inputBytes, fileName);

            //then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

            this.dateTimeBrokerMock.Verify(broker =>
              broker.GetCurrentDateTimeOffset(),
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

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(2));

            this.pdsAuditServiceMock.Verify(service =>
              service.AddPdsAuditAsync(It.Is(SamePdsAuditAs(outputPdsAudit))),
                  Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
