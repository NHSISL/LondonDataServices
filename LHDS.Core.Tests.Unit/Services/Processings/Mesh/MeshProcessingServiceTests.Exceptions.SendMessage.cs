// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task
            ShouldThrowDependencyValidationExceptionOnSendMessageIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
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

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    message: "Mesh processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

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
                    default))
                        .ThrowsAsync(dependencyValidationException);

            Stream fileContent = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));

            // when
            ValueTask<MeshMessage> retrieveSendMessageTask =
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
                    accept);

            MeshProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(
                    retrieveSendMessageTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

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
                    default),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyValidationException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnSendMessageIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
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

            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    message: "Mesh processing dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

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
                    default))
                    .ThrowsAsync(dependencyException);

            Stream fileContent = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));

            // when
            ValueTask<MeshMessage> retrieveMessageAndAcknowledgeTask =
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
                    accept);

            MeshProcessingDependencyException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(
                    retrieveMessageAndAcknowledgeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

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
                    default),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnSendMessageIfServiceErrorOccursAsync()
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

            var serviceException = new Exception();

            var failedMeshProcessingServiceException =
                new FailedMeshProcessingServiceException(
                    message: "Failed mesh processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedMeshProcessingServiveException =
                new MeshProcessingServiceException(
                    message: "Mesh processing service error occurred, please contact support.",
                    innerException: failedMeshProcessingServiceException);

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
                    default))
                        .ThrowsAsync(serviceException);

            Stream fileContent = new MemoryStream(Encoding.UTF8.GetBytes(GetRandomString()));

            // when
            ValueTask<MeshMessage> retrieveSendMessageTask =
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
                    accept);

            MeshProcessingServiceException actualException =
                await Assert.ThrowsAsync<MeshProcessingServiceException>(retrieveSendMessageTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingServiveException);

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
                    default),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogErrorAsync(It.Is(SameExceptionAs(
                     expectedMeshProcessingServiveException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
