//---------------------------------------------------------------
//Copyright(c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

//using System;
//using System.Threading.Tasks;
//using FluentAssertions;
//using LHDS.Core.Models.Foundations.Mesh;
//using LHDS.Core.Models.Foundations.Mesh.Exceptions;
//using Moq;
//using NEL.MESH.Models.Foundations.Mesh;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
//{
//    public partial class MeshServiceTests
//    {
//        [Fact]
//        public async Task ShouldThrowServiceExceptionOnSendFileIfServiceErrorOccursAndLogItAsync()
//        {
//            given
//           dynamic dynamicMeshMessageProperties =
//               CreateRandomMeshMessageProperties();

//            var randomMessage = new Message
//            {
//                MessageId = dynamicMeshMessageProperties.MessageId,
//                Headers = dynamicMeshMessageProperties.Headers,
//                StringContent = dynamicMeshMessageProperties.StringContent,
//                FileContent = dynamicMeshMessageProperties.FileContent,
//                TrackingInfo = dynamicMeshMessageProperties.TrackingInfo
//            };

//            var inputMessage = randomMessage;

//            MeshMessage randomMeshMessage = new MeshMessage
//            {
//                MessageId = dynamicMeshMessageProperties.MessageId,
//                Headers = dynamicMeshMessageProperties.Headers,
//                StringContent = dynamicMeshMessageProperties.StringContent,
//                FileContent = dynamicMeshMessageProperties.FileContent,
//                TrackingInfo = dynamicMeshMessageProperties.TrackingInfo
//            };

//            var inputMeshMessage = randomMeshMessage;
//            var serviceException = new Exception();

//            var failedMeshServiceException =
//               new FailedMeshServiceException(serviceException);

//            var expectedMeshServiceException =
//               new MeshServiceException(failedMeshServiceException);

//            this.meshBrokerMock.Setup(broker =>
//                broker.SendMessageAsync(inputMessage))
//                    .ThrowsAsync(serviceException);

//            when
//            ValueTask<MeshMessage> sendMessageTask =
//                this.meshService.SendFileAsync(inputMeshMessage);

//            MeshServiceException actualMeshServiceException =
//                await Assert.ThrowsAsync<MeshServiceException>
//                    (sendMessageTask.AsTask);

//            then
//            actualMeshServiceException.Should()
//                .BeEquivalentTo(expectedMeshServiceException);

//            this.meshBrokerMock.Verify(broker =>
//                broker.SendFileAsync(inputMessage),
//                    Times.Once);

//            this.loggingBrokerMock.Verify(broker =>
//               broker.LogError(It.Is(SameExceptionAs(
//                   expectedMeshServiceException))),
//                       Times.Once);

//            this.meshBrokerMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }
//    }
//}
