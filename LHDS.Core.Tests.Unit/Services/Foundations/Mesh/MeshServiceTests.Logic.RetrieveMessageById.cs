//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System.Threading.Tasks;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
//{
//    public partial class MeshServiceTests
//    {
//        [Fact]
//        public async Task ShouldReturnRetrieveMessageByIdAsync()
//        {
//            // given
//            string randomMailboxId = GetRandomMessage();
//            string inputMailboxId = randomMailboxId;
//            string randomMessageId = GetRandomMessage();
//            string inputMessageId = randomMessageId;
//            string outputValidationResult = "";
//            string expectedValidationResult = outputValidationResult;

//            this.meshBrokerMock.Setup(broker =>
//                broker.GetMessageByIdAsync(inputMailboxId, inputMessageId))
//                    .ReturnsAsync(outputValidationResult);

//            // when
//            string actualMeshValidationResult =
//                await this.meshService.RetrieveMessageByIdAsync(inputMailboxId, inputMessageId);

//            // then
//            actualMeshValidationResult.Should().Be(expectedValidationResult);

//            this.meshBrokerMock.Verify(broker =>
//                broker.GetMessageByIdAsync(inputMailboxId, inputMessageId),
//                    Times.Once());

//            this.meshBrokerMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }
//    }
//}
