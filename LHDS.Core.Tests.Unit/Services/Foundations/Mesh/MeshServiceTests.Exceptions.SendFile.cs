//---------------------------------------------------------------
//Copyright(c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Moq;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using NEL.MESH.Models.Foundations.Mesh;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldThrowMeshClientValidationExceptionOnSendFileIfValidationFailsAndLogItAsync()
        {
            // given
            dynamic dynamicMeshMessageProperties = CreateRandomMeshMessageProperties();
            var randomMessage = GetRandomString();

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var inputMeshMessage = randomMeshMessage;
            var validationException = new Exception(randomMessage);

            var meshClientValidationException =
                new MeshClientValidationException(validationException as Xeption);

            var expectedDependencyValidationException =
                new MeshServiceDependencyValidationException(meshClientValidationException);

            this.meshBrokerMock.Setup(broker =>
                broker.SendFileAsync(It.IsAny<Message>()))
                    .ThrowsAsync(meshClientValidationException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshService.SendFileAsync(inputMeshMessage);

            MeshServiceDependencyValidationException actualValidationException =
                await Assert.ThrowsAsync<MeshServiceDependencyValidationException>(sendMessageTask.AsTask);

            // then
            MeshServiceDependencyValidationException actualMeshServiceDependencyValidationException =
                await Assert.ThrowsAsync<MeshServiceDependencyValidationException>(
                    sendMessageTask.AsTask);

            actualValidationException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(It.IsAny<Message>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedDependencyValidationException))),
                   Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowMeshClientDependencyExceptionOnSendFileIfDependencyFailsAndLogItAsync()
        {
            // given
            dynamic dynamicMeshMessageProperties = CreateRandomMeshMessageProperties();
            var randomMessage = GetRandomString();

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                StringContent = dynamicMeshMessageProperties.StringContent,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var inputMeshMessage = randomMeshMessage;
            var dependencyException = new Exception(randomMessage);

            var meshClientDependencyException =
                new MeshClientDependencyException(dependencyException as Xeption);

            var expectedDependencyException =
                new MeshServiceDependencyException(meshClientDependencyException);

            this.meshBrokerMock.Setup(broker =>
                broker.SendFileAsync(It.IsAny<Message>()))
                    .ThrowsAsync(meshClientDependencyException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshService.SendFileAsync(inputMeshMessage);

            MeshServiceDependencyException actualDependencyException =
                await Assert.ThrowsAsync<MeshServiceDependencyException>(sendMessageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(expectedDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(It.IsAny<Message>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedDependencyException))),
                   Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowMeshClientServiceExceptionOnSendFileIfClientServiceErrorOccursAndLogItAsync()
        {
            // given
            dynamic dynamicMeshMessageProperties = CreateRandomMeshMessageProperties();
            var randomMessage = GetRandomString();

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                StringContent = dynamicMeshMessageProperties.StringContent,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var inputMeshMessage = randomMeshMessage;
            var clientServiceException = new Exception(randomMessage);

            var meshClientServiceException =
                new MeshClientServiceException(clientServiceException as Xeption);

            var expectedClientServiceException =
                new MeshServiceDependencyException(meshClientServiceException);

            this.meshBrokerMock.Setup(broker =>
                broker.SendFileAsync(It.IsAny<Message>()))
                    .ThrowsAsync(meshClientServiceException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshService.SendFileAsync(inputMeshMessage);

            MeshServiceDependencyException actualDependencyException =
                await Assert.ThrowsAsync<MeshServiceDependencyException>(sendMessageTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(expectedClientServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(It.IsAny<Message>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedClientServiceException))),
                   Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnSendFileIfServiceErrorOccursAndLogItAsync()
        {
            // given
            dynamic dynamicMeshMessageProperties =
                CreateRandomMeshMessageProperties();

            MeshMessage randomMeshMessage = new MeshMessage
            {
                MessageId = dynamicMeshMessageProperties.MessageId,
                Headers = dynamicMeshMessageProperties.Headers,
                StringContent = dynamicMeshMessageProperties.StringContent,
                FileContent = dynamicMeshMessageProperties.FileContent,
                TrackingInfo = MaptToMeshMessageTrackingInfo(dynamicMeshMessageProperties.TrackingInfo)
            };

            var inputMeshMessage = randomMeshMessage;
            var serviceException = new Exception();

            var failedMeshServiceException =
               new FailedMeshServiceException(serviceException);

            var expectedMeshServiceException =
               new MeshServiceException(failedMeshServiceException);

            this.meshBrokerMock.Setup(broker =>
                broker.SendFileAsync(It.IsAny<Message>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<MeshMessage> sendMessageTask =
                this.meshService.SendFileAsync(inputMeshMessage);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>
                    (sendMessageTask.AsTask);

            // then
            actualMeshServiceException.Should()
                .BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.SendFileAsync(It.IsAny<Message>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedMeshServiceException))),
                       Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
