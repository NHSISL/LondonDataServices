//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System.Collections.Generic;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
//{
//    public partial class MeshServiceTests
//    {
//        [Fact]
//        public async Task ShouldReturnRetrieveMessageIdsFromInboxAsync()
//        {
//            // given
//            string randomMessageId = GetRandomMessage();
//            string inputMailboxId = randomMessageId;

//            List<string> outputValidationResult = GetRandomMessages(GetRandomNumber());
//            List<string> expectedValidationResult = outputValidationResult;

//            this.meshBrokerMock.Setup(broker =>
//                broker.GetMessageIdsFromInboxAsync(inputMailboxId))
//                    .ReturnsAsync(outputValidationResult);

//            // when
//            List<string> actualMeshValidation =
//                await this.meshService.RetrieveMessageIdsFromInboxAsync(inputMailboxId);

//            // then
//            actualMeshValidation.Should().BeSameAs(expectedValidationResult);

//            this.meshBrokerMock.Verify(broker =>
//                broker.GetMessageIdsFromInboxAsync(inputMailboxId),
//                    Times.Once());

//            this.meshBrokerMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }
//    }
//}
