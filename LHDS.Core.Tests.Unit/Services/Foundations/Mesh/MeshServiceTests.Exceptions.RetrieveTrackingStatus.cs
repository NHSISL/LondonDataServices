// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Moq;
using NEL.MESH.Models.Clients.Mesh.Exceptions;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldThrowMeshServiceDependencyValidationExceptionOnRetrieveTrackingStatusIfValidationFailsAndLogItAsync()
        {
            // given
            string messageId = GetRandomString();
            string randomMessage = GetRandomString();
            var validationException = new Exception(randomMessage);

            var meshClientValidationException =
                new MeshClientValidationException(validationException as Xeption);

            var expectedDependencyValidationException =
                new MeshServiceDependencyValidationException(
                    message: "Mesh service dependency validation occurred, please try again.",
                    innerException: meshClientValidationException);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(It.IsAny<string>()))
                    .ThrowsAsync(meshClientValidationException);

            // when
            ValueTask<MeshMessage> retrieveTrackingStatusTask =
                this.meshService.RetrieveTrackingStatusByIdAsync(messageId);

            MeshServiceDependencyValidationException actualValidationException =
                await Assert.ThrowsAsync<MeshServiceDependencyValidationException>(retrieveTrackingStatusTask.AsTask);

            // then
            MeshServiceDependencyValidationException actualMeshServiceDependencyValidationException =
                await Assert.ThrowsAsync<MeshServiceDependencyValidationException>(
                    retrieveTrackingStatusTask.AsTask);

            actualValidationException.Should().BeEquivalentTo(expectedDependencyValidationException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(It.IsAny<string>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(expectedDependencyValidationException))),
                   Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowMeshServiceDependencyExceptionOnRetrieveTrackingStatusIfDependencyFailsAndLogItAsync()
        {
            // given
            string messageId = GetRandomString();
            string randomMessage = GetRandomString();
            var dependencyException = new Exception(randomMessage);

            var meshClientDependencyException =
                new MeshClientDependencyException(dependencyException as Xeption);

            var expectedDependencyException =
                new MeshServiceDependencyException(
                    message: "Mesh service dependency error occurred, please contact support.",
                    innerException: meshClientDependencyException);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(It.IsAny<string>()))
                    .ThrowsAsync(meshClientDependencyException);

            // when
            ValueTask<MeshMessage> retrieveTrackingStatusTask =
                this.meshService.RetrieveTrackingStatusByIdAsync(messageId);

            MeshServiceDependencyException actualDependencyException =
                await Assert.ThrowsAsync<MeshServiceDependencyException>(retrieveTrackingStatusTask.AsTask);

            // then
            actualDependencyException.Should().BeEquivalentTo(expectedDependencyException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(It.IsAny<string>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(expectedDependencyException))),
                   Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveTrackingStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string messageId = GetRandomString();
            var serviceException = new Exception();

            var failedMeshServiceException =
               new FailedMeshServiceException(
                   message: "Failed mesh service error occurred, please contact support.",
                   innerException: serviceException);

            var expectedMeshServiceException =
               new MeshServiceException(
                   message: "Mesh service error occurred, please contact support.",
                   innerException: failedMeshServiceException);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<MeshMessage> RetrieveTrackingStatusTask =
                this.meshService.RetrieveTrackingStatusByIdAsync(messageId);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>
                    (RetrieveTrackingStatusTask.AsTask);

            // then
            actualMeshServiceException.Should()
                .BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.TrackMessageAsync(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedMeshServiceException))),
                       Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
