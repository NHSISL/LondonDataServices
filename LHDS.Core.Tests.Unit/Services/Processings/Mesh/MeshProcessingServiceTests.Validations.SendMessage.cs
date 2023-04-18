// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfArgsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            MeshMessage randomMessage = CreateRandomMessage();
            randomMessage.MessageId = invalidText;

            var invalidMeshProcessingArgumentException =
                new InvalidMeshProcessingArgumentException();

            invalidMeshProcessingArgumentException.AddData(
               key: nameof(randomMessage.MessageId),
               values: "Text is required");

            var expectedMeshProcessingValidationException =
            new MeshProcessingValidationException(
                   innerException: invalidMeshProcessingArgumentException,
                   validationSummary: GetValidationSummary(invalidMeshProcessingArgumentException.Data));

            // when
            ValueTask<MeshMessage> retrieveSendMessageTask =
                this.meshProcessingService.SendMessageAsync(randomMessage);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(retrieveSendMessageTask.AsTask);

            //then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshProcessingValidationException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

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
