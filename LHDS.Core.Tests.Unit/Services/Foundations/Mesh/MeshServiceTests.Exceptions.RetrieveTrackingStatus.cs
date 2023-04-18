// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
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
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveTrackingStatusIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string mailboxId = GetRandomMessage();
            string messageId = GetRandomMessage();
            var serviceException = new Exception();

            var failedMeshServiceException =
               new FailedMeshServiceException(serviceException);

            var expectedMeshServiceException =
               new MeshServiceException(failedMeshServiceException);

            this.meshBrokerMock.Setup(broker =>
                broker.TrackMessageAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<MeshMessage> RetrieveTrackingStatusTask =
                this.meshService.RetrieveTrackingStatusAsync(messageId);

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
               broker.LogError(It.Is(SameExceptionAs(
                   expectedMeshServiceException))),
                       Times.Once);

            this.meshBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
