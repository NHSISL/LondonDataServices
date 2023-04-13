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
        public async Task ShouldThrowDependencyValidationExceptionOnSendMessageIfDependencyValidationErrorOccursAndLogItAsync(
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

            this.meshServiceMock.Setup(service =>
                service.RetrieveTrackingStatusAsync(randomMessage.MessageId))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<Message> retrieveSendMessageTask =
                this.meshProcessingService.SendMessageAsync(randomMessage);

            MeshProcessingDependencyValidationException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(retrieveSendMessageTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

            this.meshServiceMock.Verify(service =>
                service.SendMessageAsync(randomMessage),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
              service.RetrieveTrackingStatusAsync(randomMessage.MessageId),
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
        public async Task ShouldThrowDependencyOnSendMessageIfDependencyErrorOccursAndLogItAsync(
            Xeption dependencyException)
        {
            // given
            Message randomMessage = CreateRandomMessage();

            var expectedMeshProcessingDependencyException =
                new MeshProcessingDependencyException(
                    dependencyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
             service.RetrieveTrackingStatusAsync(randomMessage.MessageId))
                 .Throws(dependencyException);

            this.meshServiceMock.Setup(service =>
                service.SendMessageAsync(randomMessage))
                    .Throws(dependencyException);

            // when
            ValueTask<Message> retrieveMessageAndAcknowledgeTask =
                this.meshProcessingService.SendMessageAsync(randomMessage);

            MeshProcessingDependencyException actualException =
                await Assert.ThrowsAsync<MeshProcessingDependencyException>(retrieveMessageAndAcknowledgeTask.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveTrackingStatusAsync(randomMessage.MessageId),
                    Times.Once);

            this.meshServiceMock.Verify(service =>
               service.SendMessageAsync(randomMessage),
                   Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedMeshProcessingDependencyException))),
                         Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        //[Fact]
        //public async Task ShouldThrowServiceExceptionOnSendMessageIfServiceErrorOccursAsync()
        //{
        //    // given
        //    string mailboxId = GetRandomString();
        //    string messageId = GetRandomString();

        //    var serviceException = new Exception();

        //    var failedMeshProcessingServiceException =
        //        new FailedMeshProcessingServiceException(serviceException);

        //    var expectedMeshProcessingServiveException =
        //        new MeshProcessingServiceException(
        //            failedMeshProcessingServiceException);

        //    this.meshServiceMock.Setup(service =>
        //        service.SendMessageAsync(messageId))
        //            .Throws(serviceException);

        //    // when
        //    ValueTask<string> retrieveSendMessageTask =
        //        this.meshProcessingService.SendMessageAsync(mailboxId, messageId);

        //    MeshProcessingServiceException actualException =
        //        await Assert.ThrowsAsync<MeshProcessingServiceException>(retrieveSendMessageTask.AsTask);

        //    // then
        //    actualException.Should().BeEquivalentTo(expectedMeshProcessingServiveException);

        //    this.meshServiceMock.Verify(service =>
        //        service.SendMessageAsync(messageId),
        //            Times.Once);

        //    this.meshServiceMock.Verify(service =>
        //       service.RetrieveTrackingStatusAsync(mailboxId, messageId),
        //           Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //         broker.LogError(It.Is(SameExceptionAs(
        //             expectedMeshProcessingServiveException))),
        //                 Times.Once);

        //    this.meshServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}
    }
}
