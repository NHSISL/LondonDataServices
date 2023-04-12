//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System.Net.Http;
//using System.Reflection.PortableExecutable;
//using System.Runtime.InteropServices;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Moq;
//using NEL.MESH.Models.Foundations.Mesh;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
//{
//    public partial class MeshServiceTests
//    {
//        [Fact]
//        public async Task ShouldReturnSendMessageAsync()
//        {
//            // given
//            dynamic meshMessageProperties =
//                CreateRandomMeshMessageProperties();

//            var randomMeshMessage = new Message
//            {
//                MessageId = meshMessageProperties.MessageId,
//                Headers = meshMessageProperties.Headers,
//                StringContent = meshMessageProperties.StringContent,
//                FileContent = meshMessageProperties.FileContent,
//                TrackingInfo = meshMessageProperties.TrackingInfo
//            };

//            Message inputMessage = randomMeshMessage;

//            var expectedMeshMessage = new Message
//            {
//                MessageId = meshMessageProperties.MessageId,
//                Headers = meshMessageProperties.Headers,
//                StringContent = meshMessageProperties.StringContent,
//                FileContent = meshMessageProperties.FileContent,
//                TrackingInfo = meshMessageProperties.TrackingInfo
//            };

//            this.meshBrokerMock.Setup(broker =>
//                broker.SendMessageAsync(inputMessage))
//                    .ReturnsAsync(randomMeshMessage);

//            // when
//            Message actualMeshMessage =
//                await this.meshService.SendMessageAsync(inputMessage);

//            // then
//            actualMeshMessge.Should().Be(expectedValidationResult);

//            this.meshBrokerMock.Verify(broker =>
//                broker.SendMessageAsync(inputMessage),
//                    Times.Once());

//            this.meshBrokerMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }
//    }
//}
