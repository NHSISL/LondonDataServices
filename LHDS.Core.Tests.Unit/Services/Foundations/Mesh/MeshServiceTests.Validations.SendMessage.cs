// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

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
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfRequiredMessageItemsAreNullAsync(
            string invalidInput)
        {
            // given
            string inputMessageId = GetRandomString();

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = inputMessageId,
                Headers = null,
                StringContent = invalidInput,
            };

            var inputMeshMessage = randomMeshMessage;

            var invalidMeshMessageException =
                new InvalidMeshMessageException();

            invalidMeshMessageException.AddData(
                key: nameof(Message.StringContent),
                values: "Text is required");

            invalidMeshMessageException.AddData(
                key: "Headers",
                values: "Values is required");

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
                broker.SendMessageAsync(It.IsAny<Message>()),
                        Times.Never);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
