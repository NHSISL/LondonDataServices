// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Text;
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
        public async Task ShouldThrowValidationExceptionOnSendFileIfMessageIsNullAsync()
        {
            // given
            MeshMessage nullMeshMessage = null;
            Message nullMessage = null;

            var nullMeshMessageException =
                new NullMeshMessageException();

            var expectedMeshValidationException =
                new MeshValidationException(nullMeshMessageException);

            // when
            ValueTask<MeshMessage> sendFileTask =
                this.meshService.SendFileAsync(nullMeshMessage);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    sendFileTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(nullMessage),
                        Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendFileIfRequiredMessageItemsAreNullAsync(
            string invalidInput)
        {
            // given
            string inputMessageId = GetRandomString();

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = inputMessageId,
                Headers = null,
                FileContent = null,
            };

            var inputMeshMessage = randomMeshMessage;

            var invalidMeshMessageException =
                new InvalidMeshMessageException();

            invalidMeshMessageException.AddData(
                key: nameof(Message.FileContent),
                values: "Content is required");

            invalidMeshMessageException.AddData(
                key: "Headers",
                values: "Values is required");

            var expectedMeshValidationException =
                new MeshValidationException(
                innerException: invalidMeshMessageException,
                validationSummary: GetValidationSummary(invalidMeshMessageException.Data));

            // when
            ValueTask<MeshMessage> sendFileTask =
                this.meshService.SendFileAsync(inputMeshMessage);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(() =>
                    sendFileTask.AsTask());

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(It.IsAny<Message>()),
                        Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
