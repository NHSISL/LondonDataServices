// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Fact]
        public async Task
            ShouldSkipMessageWithMissingWorkflowIdHeaderAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            List<string> randomMessageIds = new List<string> { randomMessageId };

            MeshMessage messageWithMissingHeaders = new MeshMessage
            {
                MessageId = randomMessageId,
                Headers = new Dictionary<string, List<string>>
                {
                    { "mex-to", new List<string> { GetRandomString() } }
                }
            };

            string tempFilePath = Path.Combine(
                Path.GetTempPath(), Guid.NewGuid().ToString());

            this.tempLocationBrokerMock.Setup(broker =>
                broker.GetUniqueHomeFilePath())
                    .Returns(tempFilePath);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFileAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);

            this.meshServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync(
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(randomMessageIds)
                        .ReturnsAsync(new List<string>());

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(messageWithMissingHeaders);

            // when
            List<PdsAudit> actualPdsAudits =
                await this.pdsOrchestrationService
                    .RetreiveMessagesFromMeshAndUpdateStorage(
                        TestContext.Current.CancellationToken);

            // then
            actualPdsAudits.Should().BeEmpty();

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(
                    It.IsAny<CancellationToken>()),
                        Times.Exactly(2));

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.tempLocationBrokerMock.Verify(broker =>
                broker.GetUniqueHomeFilePath(),
                    Times.Once);

            this.tempLocationBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldSkipMessageWithMissingFilenameHeaderAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            List<string> randomMessageIds = new List<string> { randomMessageId };

            MeshMessage messageWithMissingFilename = new MeshMessage
            {
                MessageId = randomMessageId,
                Headers = new Dictionary<string, List<string>>
                {
                    { "mex-workflowid", new List<string> { this.pdsConfiguration.WorkflowId } },
                    { "mex-localid", new List<string> { Guid.NewGuid().ToString() } }
                }
            };

            string tempFilePath = Path.Combine(
                Path.GetTempPath(), Guid.NewGuid().ToString());

            this.tempLocationBrokerMock.Setup(broker =>
                broker.GetUniqueHomeFilePath())
                    .Returns(tempFilePath);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFileAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);

            this.meshServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync(
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(randomMessageIds)
                        .ReturnsAsync(new List<string>());

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(messageWithMissingFilename);

            // when
            List<PdsAudit> actualPdsAudits =
                await this.pdsOrchestrationService
                    .RetreiveMessagesFromMeshAndUpdateStorage(
                        TestContext.Current.CancellationToken);

            // then
            actualPdsAudits.Should().BeEmpty();

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(
                    It.IsAny<CancellationToken>()),
                        Times.Exactly(2));

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.tempLocationBrokerMock.Verify(broker =>
                broker.GetUniqueHomeFilePath(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                        Times.Never);

            this.tempLocationBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldSkipMessageWithInvalidLocalIdHeaderAsync()
        {
            // given
            string randomMessageId = GetRandomString();
            List<string> randomMessageIds = new List<string> { randomMessageId };

            MeshMessage messageWithInvalidLocalId = new MeshMessage
            {
                MessageId = randomMessageId,
                Headers = new Dictionary<string, List<string>>
                {
                    { "mex-workflowid", new List<string> { this.pdsConfiguration.WorkflowId } },
                    { "mex-filename", new List<string> { GetRandomString() } },
                    { "mex-localid", new List<string> { GetRandomString() } }
                }
            };

            string tempFilePath = Path.Combine(
                Path.GetTempPath(), Guid.NewGuid().ToString());

            this.tempLocationBrokerMock.Setup(broker =>
                broker.GetUniqueHomeFilePath())
                    .Returns(tempFilePath);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFileAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);

            this.meshServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync(
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(randomMessageIds)
                        .ReturnsAsync(new List<string>());

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(messageWithInvalidLocalId);

            // when
            List<PdsAudit> actualPdsAudits =
                await this.pdsOrchestrationService
                    .RetreiveMessagesFromMeshAndUpdateStorage(
                        TestContext.Current.CancellationToken);

            // then
            actualPdsAudits.Should().BeEmpty();

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(
                    It.IsAny<CancellationToken>()),
                        Times.Exactly(2));

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(
                    randomMessageId,
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.tempLocationBrokerMock.Verify(broker =>
                broker.GetUniqueHomeFilePath(),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
                service.AcknowledgeMessageByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()),
                        Times.Never);

            this.tempLocationBrokerMock.VerifyNoOtherCalls();
            this.meshServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
        }
    }
}
