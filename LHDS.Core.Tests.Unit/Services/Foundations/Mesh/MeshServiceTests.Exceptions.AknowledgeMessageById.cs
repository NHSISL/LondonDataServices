// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
{
    public partial class MeshServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnAknowledgeMessageByIdIfServiceErrorOccursAndLogItAsync()
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
                broker.AcknowledgeMessageByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<bool> RetrieveAknowledgeMessageByIdTask =
                this.meshService.AcknowledgeMessageByIdAsync(
                    messageId,
                    TestContext.Current.CancellationToken);

            MeshServiceException actualMeshServiceException =
                await Assert.ThrowsAsync<MeshServiceException>
                    (RetrieveAknowledgeMessageByIdTask.AsTask);

            // then
            actualMeshServiceException.Should()
                .BeEquivalentTo(expectedMeshServiceException);

            this.meshBrokerMock.Verify(broker =>
                broker.AcknowledgeMessageByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
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
