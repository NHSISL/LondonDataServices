// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Clients;
using LHDS.Core.Models.Foundations.OptOuts;
using Moq;
using NEL.MESH.Clients.Mailboxes;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;
using Xunit.Sdk;
using static System.Net.Mime.MediaTypeNames;

namespace LHDS.Core.Tests.Acceptance.Clients.OptOuts
{
    public partial class OptOutTests
    {
        [Fact]
        public async Task ShouldPushExpiredOptOutToMeshForRenewalAsync()
        {
            //Given
            string messageId = GetRandomString();
            int randomNumber = GetRandomNumber();
            List<string> messageIds = new List<string> { messageId };

            List<OptOut> outputOptOuts = 
                CreateRandomOptOuts(randomNumber, this.dateTimeBroker.GetCurrentDateTimeOffset());

            string mexWorkflowId = this.optOutConfiguration.WorkflowId;
            string batchReference = this.dateTimeBroker.GetCurrentDateTimeOffset().ToString("yyyyMMddHHmmss");
            string mexTo = this.optOutConfiguration.To;
            var optOutStringList = new StringBuilder();

            outputOptOuts
                .ForEach(optOut => 
                    optOutStringList.AppendLine($"{optOut.UniqueReference},{optOut.NhsNumber},{optOut.Status},,"));

            byte[] fileContent = Encoding.ASCII.GetBytes(optOutStringList.ToString());

            foreach(OptOut optOut in outputOptOuts)
            {
                await this.optOutService.AddOptOutAsync(optOut);
            }

            Message message = ComposeMessage.CreateFileMessage(
                mexTo,
                mexWorkflowId,
                fileContent,
                mexLocalId: batchReference,
                mexFileName: $"{batchReference}.txt",
                contentType: "text/plain");

            message.MessageId = messageId;

            this.meshBrokerMock.Setup(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<byte[]>(),
                    "", 
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    "",
                    "text/plain",
                    "",
                    "application/json"))
                        .ReturnsAsync(message);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(messageId))
                    .ReturnsAsync(message);

            //When
            var actualMessage = await this.optOutClient.PushExpiredOptOutsToMeshForRenewalAsync();

            //Then
            actualMessage.Should().BeEquivalentTo(message);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<byte[]>(),
                    "",
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    "",
                    "text/plain",
                    "",
                    "application/json"),
                        Times.Once);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(messageId),
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
