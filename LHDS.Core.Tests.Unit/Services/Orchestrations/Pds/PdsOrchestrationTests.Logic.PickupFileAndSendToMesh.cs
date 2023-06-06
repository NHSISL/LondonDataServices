// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            var inputBytes = Encoding.ASCII.GetBytes(inputString);
            var fileName = GetRandomString();
            PdsAudit pdsAudit = GetRandomPdsAudit(identifier, fileName, randomDate);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.identifierBrokerMock.Setup(processing =>
               processing.GetIdentifier())
                   .Returns(identifier);

            this.pdsAuditServiceMock.Setup(service =>
                service.AddPdsAuditAsync(pdsAudit));

            string mexTo = this.pdsConfiguration.To;
            string mexWorkflowId = this.pdsConfiguration.WorkflowId;
            byte[] fileContent = inputBytes;
            string mexSubject = string.Empty;
            string mexLocalId = pdsAudit.CorrelationId.ToString();
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

            PdsAudit expectedPdsAudit = pdsAudit.DeepClone();

            //when
            PdsAudit actualPdsAudit =
                await this.pdsOrchestrationService.PickupFileAndSendToMesh(inputBytes, fileName);

            //then
            actualPdsAudit.Should().BeEquivalentTo(expectedPdsAudit);

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
            this.documentServiceMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
