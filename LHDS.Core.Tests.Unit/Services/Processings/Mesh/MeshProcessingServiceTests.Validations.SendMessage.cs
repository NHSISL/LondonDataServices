// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfReturnIsNullAndLogItAsync()
        {
            //Given
            MeshMessage randomMessage = CreateRandomMessage();
            MeshMessage nonExistMessage = randomMessage;
            MeshMessage nullMessage = null;

            var nullMeshProcessingException =
               new NullMeshProcessingException();

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(nullMeshProcessingException);

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(nonExistMessage))
                .ReturnsAsync(nullMessage);

            //When
            ValueTask<MeshMessage> SendMessageTask =
                this.meshProcessingService.SendMessageAsync(nonExistMessage);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(SendMessageTask.AsTask);

            //Then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(service =>
               service.SendMessageAsync(nonExistMessage),
               Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshProcessingValidationException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfReturnedMessageIdIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            MeshMessage randomSendMessage = CreateRandomSendMessage();
            MeshMessage returnedSendMessage = CreateRandomMessage();
            returnedSendMessage.MessageId = invalidText;

            var invalidMeshMessageException =
                new InvalidMeshMessageException();

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(randomSendMessage))
                .ReturnsAsync(returnedSendMessage);

            invalidMeshMessageException.AddData(
                key: nameof(returnedSendMessage.MessageId),
                values: "Text is required");

            var expectedMeshProcessingValidationException =
            new MeshProcessingValidationException(
                innerException: invalidMeshMessageException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshProcessingService.SendMessageAsync(randomSendMessage);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(() =>
                    sendMessageTask.AsTask());

            //then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(randomSendMessage),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshProcessingValidationException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIRetrieveTrackingStatusIsNullAndLogItAsync()
        {
            //Given
            MeshMessage randomMessage = CreateRandomMessage();
            MeshMessage returnMessage = randomMessage;

            MeshMessage nonExistMessage = randomMessage;
            MeshMessage nullMessage = null;

            var nullMeshProcessingException =
               new NullMeshProcessingException();

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(nullMeshProcessingException);

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(nonExistMessage))
                .ReturnsAsync(returnMessage);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusByIdAsync(nonExistMessage.MessageId))
                .ReturnsAsync(nullMessage);

            //When
            ValueTask<MeshMessage> SendMessageTask =
                this.meshProcessingService.SendMessageAsync(nonExistMessage);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(SendMessageTask.AsTask);

            //Then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(service =>
               service.SendMessageAsync(nonExistMessage),
               Times.Once);

            this.meshServiceMock.Verify(service =>
              service.RetrieveTrackingStatusByIdAsync(nonExistMessage.MessageId),
              Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshProcessingValidationException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
