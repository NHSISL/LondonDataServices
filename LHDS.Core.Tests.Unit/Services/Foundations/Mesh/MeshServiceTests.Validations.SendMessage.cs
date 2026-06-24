// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfInputArgumentsAreInvalidAsync(
            string invalidInput)
        {
            // given
            string mexTo = invalidInput;
            string mexWorkflowId = invalidInput;
            using Stream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));
            string mexSubject = invalidInput;
            string mexLocalId = invalidInput;
            string mexFileName = invalidInput;
            string mexContentChecksum = invalidInput;
            string contentType = invalidInput;
            string contentEncoding = invalidInput;
            string accept = invalidInput;

            var invalidMeshMessageException =
                new InvalidMeshMessageException(
                    message: "Invalid mesh message, please correct errors and try again.");

            invalidMeshMessageException.AddData(
                key: "MexTo",
                values: "Text is required");

            invalidMeshMessageException.AddData(
                key: "MexWorkflowId",
                values: "Text is required");

            var expectedMeshValidationException =
               new MeshValidationException(
                   message: "Mesh validation errors occurred, please try again.",
                   innerException: invalidMeshMessageException);

            // when
            ValueTask<MeshMessage> sendFileTask =
                this.meshService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    TestContext.Current.CancellationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(sendFileTask.AsTask);

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
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
                        Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnSendMessageIfStreamIsInvalidAsync()
        {
            // given
            string mexTo = GetRandomString();
            string mexWorkflowId = GetRandomString();
            Stream inputStream = null;
            string mexSubject = GetRandomString();
            string mexLocalId = GetRandomString();
            string mexFileName = GetRandomString();
            string mexContentChecksum = GetRandomString();
            string contentType = GetRandomString();
            string contentEncoding = GetRandomString();
            string accept = "text/plain";

            var invalidMeshMessageException =
                new InvalidMeshMessageException(
                    message: "Invalid mesh message, please correct errors and try again.");

            invalidMeshMessageException.AddData(
                key: "Content",
                values: "Stream must be readable and contain data");

            var expectedMeshValidationException =
                new MeshValidationException(
                    message: "Mesh validation errors occurred, please try again.",
                    innerException: invalidMeshMessageException);

            // when
            ValueTask<MeshMessage> sendFileTask =
                this.meshService.SendMessageAsync(
                    mexTo,
                    mexWorkflowId,
                    inputStream,
                    mexSubject,
                    mexLocalId,
                    mexFileName,
                    mexContentChecksum,
                    contentType,
                    contentEncoding,
                    accept,
                    TestContext.Current.CancellationToken);

            MeshValidationException actualMeshValidationException =
                await Assert.ThrowsAsync<MeshValidationException>(sendFileTask.AsTask);

            // then
            actualMeshValidationException.Should()
                .BeEquivalentTo(expectedMeshValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendMessageAsync(
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
                        Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedMeshValidationException))),
                        Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
