// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfMessageIsNullAsync()
        {
            // given
            MeshMessage nullMeshMessage = null;
            Message nullMessage = null;

            var nullMeshMessageException =
                new NullMeshMessageException();

            var expectedMeshValidationException =
                new MeshValidationException(nullMeshMessageException);

            // when
            ValueTask<MeshMessage> addMessageTask =
                this.meshService.SendMessageAsync(nullMeshMessage);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    addMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(nullMessage),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfHeadersDictionaryIsNullAsync()
        {
            // given
            MeshMessage meshMessageWithNullHeaders = new MeshMessage
            {
                Headers = null
            };

            Message messageWithNullHeaders = new Message
            {
                Headers = null
            };

            var nullHeadersException =
                new NullHeadersException();

            var expectedMeshValidationException =
                new MeshValidationException(nullHeadersException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshService.SendMessageAsync(meshMessageWithNullHeaders);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    sendMessageTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(messageWithNullHeaders),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfRequiredMessageItemsAreNullAsync(
            string invalidInput)
        {
            // given
            dynamic dynamicMeshMessageProperties = CreateRandomMeshMessageProperties();
            string inputMessageId = GetRandomString();

            var randomMessage = new Message
            {
                MessageId = inputMessageId,
                Headers = new Dictionary<string, List<string>>(),
                StringContent = invalidInput,
            };

            randomMessage.Headers.Add("Mex-From", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-To", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-WorkflowID", new List<string> { invalidInput });
            randomMessage.Headers.Add("Content-Type", new List<string> { invalidInput });
            randomMessage.Headers.Add("Mex-FileName", new List<string> { invalidInput });

            var inputMessage = randomMessage;

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = inputMessageId,
                Headers = new Dictionary<string, List<string>>(),
                StringContent = invalidInput,
            };

            randomMeshMessage.Headers.Add("Mex-From", new List<string> { invalidInput });
            randomMeshMessage.Headers.Add("Mex-To", new List<string> { invalidInput });
            randomMeshMessage.Headers.Add("Mex-WorkflowID", new List<string> { invalidInput });
            randomMeshMessage.Headers.Add("Content-Type", new List<string> { invalidInput });
            randomMeshMessage.Headers.Add("Mex-FileName", new List<string> { invalidInput });

            var inputMeshMessage = randomMeshMessage;

            var invalidMeshMessageException =
                new InvalidMeshMessageException();

            invalidMeshMessageException.AddData(
                key: nameof(Message.StringContent),
                values: "Text is required");

            invalidMeshMessageException.AddData(
                key: "Content-Type",
                values: "Header value is required");

            invalidMeshMessageException.AddData(
                key: "Mex-FileName",
                values: "Header value is required");

            invalidMeshMessageException.AddData(
                key: "Mex-From",
                values: "Header value is required");

            invalidMeshMessageException.AddData(
                key: "Mex-To",
                values: "Header value is required");

            invalidMeshMessageException.AddData(
                key: "Mex-WorkflowID",
                values: "Header value is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                innerException: invalidMeshMessageException,
                validationSummary: GetValidationSummary(invalidMeshMessageException.Data));

            // when
            ValueTask<MeshMessage> sendFileTask =
                this.meshService.SendMessageAsync(inputMeshMessage);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    sendFileTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(inputMessage),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
        }
    }
}
