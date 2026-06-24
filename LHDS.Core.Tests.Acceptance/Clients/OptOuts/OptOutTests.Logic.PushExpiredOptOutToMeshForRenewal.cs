// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldPushExpiredOptOutToMeshForRenewalAsync()
        {
            //Given
            DateTimeOffset currentDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            string timestamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");
            string messageId = GetRandomString();
            int randomNumber = GetRandomNumber();
            List<string> messageIds = new List<string> { messageId };

            List<OptOut> outputOptOuts =
                CreateRandomOptOuts(randomNumber, currentDateTimeOffset);

            string mexWorkflowId = this.optOutConfiguration.WorkflowId;
            DateTimeOffset batchReferenceDateTime = currentDateTimeOffset;
            string batchReference = batchReferenceDateTime.ToString("yyyyMMddHHmmss");
            string mexTo = this.optOutConfiguration.To;
            var optOutStringList = new StringBuilder();

            outputOptOuts
                .ForEach(optOut =>
                    optOutStringList.AppendLine($"{optOut.UniqueReference},{optOut.NhsNumber},{optOut.Status},,"));

            foreach (OptOut optOut in outputOptOuts)
            {
                await this.optOutService.AddOptOutAsync(optOut);
            }

            Message message = new Message
            {
                MessageId = messageId,
                Headers = new Dictionary<string, List<string>>
                {
                    { "mex-to", new List<string> { mexTo } },
                    { "mex-workflowid", new List<string> { mexWorkflowId } },
                    { "mex-localid", new List<string> { batchReference } },
                    { "mex-filename", new List<string> { $"{batchReference}.txt" } }
                }
            };

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    "",
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    "",
                    "text/plain",
                    "",
                    "application/json",
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(message);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(messageId, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(message);

            //When
            var actualMessage = await this.optOutClient
                .PushExpiredOptOutsToMeshForRenewalAsync(TestContext.Current.CancellationToken);

            //Then
            actualMessage.Should().BeEquivalentTo(message);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    "",
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    "",
                    "text/plain",
                    "",
                    "application/json",
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(messageId, It.IsAny<CancellationToken>()),
                    Times.Once);

            foreach (OptOut optOut in outputOptOuts)
            {
                await this.optOutService.RemoveOptOutByIdAsync(optOut.Id);
            }

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
