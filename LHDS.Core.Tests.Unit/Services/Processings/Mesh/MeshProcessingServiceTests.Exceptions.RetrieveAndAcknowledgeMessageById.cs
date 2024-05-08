// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveMessageAndAcknowledgeIfDependencyValidationErrorOccursAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            MeshMessage randomMessage = CreateRandomMessage();

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    message: "Mesh processing dependency validation occurred, please try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<MeshMessage> retrieveMessageAndAcknowledgeTask =
                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(randomMessage.MessageId);

            MeshProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(retrieveMessageAndAcknowledgeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyValidationException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyOnRetrieveMessageAndAcknowledgeIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            MeshMessage randomMessage = CreateRandomMessage();

            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    message: "Mesh processing dependency error occurred, please contact support.",
                    innerException: dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId))
                    .Throws(dependencyException);

            // when
            ValueTask<MeshMessage> retrieveMessageAndAcknowledgeTask =
                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(randomMessage.MessageId);

            MeshProcessingDependencyException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(retrieveMessageAndAcknowledgeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedMeshProcessingDependencyException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveMessageAndAcknowledgeIfServiceErrorOccursAsync()
        {
            // given
            MeshMessage randomMessage = CreateRandomMessage();

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
                service.RetrieveMessageByIdAsync(randomMessage.MessageId))
                    .Throws(serviceException);

            // when
            ValueTask<MeshMessage> retrieveMessageAndAcknowledgeTask =
                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(randomMessage.MessageId);

            MeshProcessingServiceException actualException =
                await Assert.ThrowsAsync<MeshProcessingServiceException>(retrieveMessageAndAcknowledgeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingServiveException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedMeshProcessingServiveException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
