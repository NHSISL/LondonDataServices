// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Processings.Mesh.Exceptions;
using Moq;
using NEL.MESH.Models.Foundations.Mesh;
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
            Message randomMessage = CreateRandomMessage();

            var expectedMeshProcessingDependencyValidationException =
                new MeshProcessingDependencyValidationException(
                    dependencyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<Message> retrieveMessageAndAcknowledgeTask =
                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(randomMessage);

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

        //        [Theory]
        //        [MemberData(nameof(DependencyExceptions))]
        //        public async Task ShouldThrowDependencyOnRetrieveMessageAndAcknowledgeIfDependencyErrorOccursAndLogItAsync(
        //          Xeption dependencyException)
        //        {
        //            // given
        //            string mailboxId = GetRandomString();
        //            string messageId = GetRandomString();

        //            var expectedMeshProcessingDependencyException =
        //                new MeshProcessingDependencyException(
        //                    dependencyException.InnerException as Xeption);

        //            this.meshServiceMock.Setup(service =>
        //                service.RetrieveMessageByIdAsync(mailboxId, messageId))
        //                    .Throws(dependencyException);

        //            // when
        //            ValueTask<string> retrieveMessageAndAcknowledgeTask =
        //                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(mailboxId, messageId);

        //            MeshProcessingDependencyException actualException =
        //                await Assert.ThrowsAsync<MeshProcessingDependencyException>(retrieveMessageAndAcknowledgeTask.AsTask);

        //            // then
        //            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

        //            this.meshServiceMock.Verify(service =>
        //                service.RetrieveMessageByIdAsync(mailboxId, messageId),
        //                    Times.Once);

        //            this.loggingBrokerMock.Verify(broker =>
        //                 broker.LogError(It.Is(SameExceptionAs(
        //                     expectedMeshProcessingDependencyException))),
        //                         Times.Once);

        //            this.meshServiceMock.VerifyNoOtherCalls();
        //            this.loggingBrokerMock.VerifyNoOtherCalls();
        //        }

        //        [Fact]
        //        public async Task ShouldThrowServiceExceptionOnRetrieveMessageAndAcknowledgeIfServiceErrorOccursAsync()
        //        {
        //            // given
        //            string mailboxId = GetRandomString();
        //            string messageId = GetRandomString();

        //            var serviceException = new Exception();

        //            var failedMeshProcessingServiceException =
        //                new FailedMeshProcessingServiceException(serviceException);

        //            var expectedMeshProcessingServiveException =
        //                new MeshProcessingServiceException(
        //                    failedMeshProcessingServiceException);

        //            this.meshServiceMock.Setup(service =>
        //                service.RetrieveMessageByIdAsync(mailboxId, messageId))
        //                    .Throws(serviceException);

        //            // when
        //            ValueTask<string> retrieveMessageAndAcknowledgeTask =
        //                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(mailboxId, messageId);

        //            MeshProcessingServiceException actualException =
        //                await Assert.ThrowsAsync<MeshProcessingServiceException>(retrieveMessageAndAcknowledgeTask.AsTask);

        //            // then
        //            actualException.Should().BeEquivalentTo(expectedMeshProcessingServiveException);

        //            this.meshServiceMock.Verify(service =>
        //                service.RetrieveMessageByIdAsync(mailboxId, messageId),
        //                    Times.Once);

        //            this.loggingBrokerMock.Verify(broker =>
        //                 broker.LogError(It.Is(SameExceptionAs(
        //                     expectedMeshProcessingServiveException))),
        //                         Times.Once);

        //            this.meshServiceMock.VerifyNoOtherCalls();
        //            this.loggingBrokerMock.VerifyNoOtherCalls();
        //        }

    }
}
