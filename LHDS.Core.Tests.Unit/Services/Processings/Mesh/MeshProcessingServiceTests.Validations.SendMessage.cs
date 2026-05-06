// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading;
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
        public async Task ShouldThrowValidationExceptionOnSendMessageIfInputsIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string mexTo = invalidText;
            string mexWorkflowId = invalidText;
            using Stream fileContent = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));
            string mexSubject = invalidText;
            string mexLocalId = invalidText;
            string mexFileName = invalidText;
            string mexContentChecksum = invalidText;
            string contentType = invalidText;
            string contentEncoding = invalidText;
            string accept = invalidText;

            var invalidMeshProcessingArgumentException =
                new InvalidMeshProcessingArgumentException(
                    message: "Invalid mesh processing argument. Please correct the errors and try again.");

            invalidMeshProcessingArgumentException.AddData(
                key: "MexTo",
                values: "Text is required");

            invalidMeshProcessingArgumentException.AddData(
                key: "MexWorkflowId",
                values: "Text is required");

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(
                    message: "Mesh processing validation errors occured, please try again",
                    innerException: invalidMeshProcessingArgumentException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshProcessingService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    TestContext.Current.CancellationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(sendMessageTask.AsTask);

            //then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept),
                        Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedMeshProcessingValidationException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfStreamIsInvalidAsync()
        {
            // given
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            string mexSubject = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string contentType = GetRandomString();
            string contentEncoding = GetRandomString();
            string accept = "text/plain";

            var invalidMeshProcessingArgumentException =
                new InvalidMeshProcessingArgumentException(
                    message: "Invalid mesh processing argument. Please correct the errors and try again.");

            invalidMeshProcessingArgumentException.AddData(
                key: "Content",
                values: "Stream must be readable and contain data");

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(
                    message: "Mesh processing validation errors occured, please try again",
                    innerException: invalidMeshProcessingArgumentException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshProcessingService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    TestContext.Current.CancellationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(sendMessageTask.AsTask);

            //then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept),
                        Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedMeshProcessingValidationException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfRetrieveTrackingStatusIsNullAndLogItAsync()
        {
            //Given
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            string mexSubject = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string contentType = GetRandomString();
            string contentEncoding = GetRandomString();
            string accept = "text/plain";

            MeshMessage returnMessage = ComposeMessage.CreateMeshMessage(
                mexTo,
                mexWorkflowId,
                mexSubject,
                mexLocalId,
                mexFileName,
                mexContentChecksum,
                contentType,
                contentEncoding,
                accept);

            returnMessage.MessageId = GetRandomString();
            MeshMessage nullTrackingMessage = null;

            var nullMeshProcessingException =
               new NullMeshMessageProcessingException(
                   message: "Mesh processing service exception. Message is Null.");

            var expectedMeshProcessingValidationException =
                new MeshProcessingValidationException(
                    message: "Mesh processing validation errors occured, please try again",
                    nullMeshProcessingException);

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()))
                        .ReturnsAsync(returnMessage);

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusByIdAsync(
                    returnMessage.MessageId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(nullTrackingMessage);

            //When
            using Stream fileContent = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));

            ValueTask<MeshMessage> SendMessageTask =
                this.meshProcessingService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    fileContent,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    TestContext.Current.CancellationToken);

            MeshProcessingValidationException actualMeshProcessingValidationException =
                await Assert.ThrowsAsync<MeshProcessingValidationException>(SendMessageTask.AsTask);

            //Then
            actualMeshProcessingValidationException.Should()
                .BeEquivalentTo(expectedMeshProcessingValidationException);

            this.meshServiceMock.Verify(service =>
               service.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    It.IsAny<Stream>(),
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    It.IsAny<CancellationToken>()),
                        Times.Once);

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusByIdAsync(
                    returnMessage.MessageId,
                    It.IsAny<CancellationToken>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedMeshProcessingValidationException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
