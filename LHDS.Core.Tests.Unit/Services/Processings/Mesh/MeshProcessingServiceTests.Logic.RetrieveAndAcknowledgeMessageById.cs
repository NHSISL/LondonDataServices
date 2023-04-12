//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System.Threading.Tasks;
//using Moq;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
//{
//    public partial class MeshProcessingServiceTests
//    {
//        [Fact]
//        public async Task ShouldReturnRetrieveMessageIdAndAcknowledgeAsync()
//        {
//            // given
//            string randomMailboxId = GetRandomString();
//            string inputMailboxId = randomMailboxId;
//            string randomMessageId = GetRandomString();
//            string inputMessageId = randomMessageId;

//            // when
//            await this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(inputMailboxId, inputMessageId);

//            // then
//            this.meshServiceMock.Verify(service =>
//                service.RetrieveMessageByIdAsync(inputMailboxId, inputMessageId),
//                    Times.Once());

//            this.meshServiceMock.Verify(service =>
//               service.AcknowledgeMessageByIdAsync(inputMailboxId, inputMessageId),
//                   Times.Once());

//            this.meshServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }
//    }
//}
