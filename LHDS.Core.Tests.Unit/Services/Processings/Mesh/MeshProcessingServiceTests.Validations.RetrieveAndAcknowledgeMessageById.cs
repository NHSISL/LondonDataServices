//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System.Threading.Tasks;
//using FluentAssertions;
//using LHDS.Core.Models.Processings.Mesh.Exceptions;
//using Moq;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
//{
//    public partial class MeshProcessingServiceTests
//    {
//        [Theory]
//        [InlineData(null)]
//        [InlineData("")]
//        [InlineData(" ")]
//        public async Task ShouldThrowValidationExceptionOnRetrieveMessagAndkAcknowledgeIfArgsIsInvalidAndLogItAsync(
//            string invalidText)
//        {
//            // given
//            string mailboxId = invalidText;
//            string messageId = invalidText;

//            var invalidMeshProcessingArgumentException =
//                new InvalidMeshProcessingArgumentException();

//            invalidMeshProcessingArgumentException.AddData(
//                key: nameof(mailboxId),
//                values: "Text is required");

//            invalidMeshProcessingArgumentException.AddData(
//               key: nameof(messageId),
//               values: "Text is required");

//            var expectedMeshProcessingValidationException =
//            new MeshProcessingValidationException(
//                   innerException: invalidMeshProcessingArgumentException,
//                   validationSummary: GetValidationSummary(invalidMeshProcessingArgumentException.Data));

//            // when
//            ValueTask<string> retrieveMessageIdsFromInboxTask =
//                this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(mailboxId, messageId);

//            MeshProcessingValidationException actualMeshProcessingValidationException =
//                await Assert.ThrowsAsync<MeshProcessingValidationException>(retrieveMessageIdsFromInboxTask.AsTask);

//            //then
//            actualMeshProcessingValidationException.Should()
//                .BeEquivalentTo(expectedMeshProcessingValidationException);

//            this.loggingBrokerMock.Verify(broker =>
//                broker.LogError(It.Is(SameExceptionAs(
//                    expectedMeshProcessingValidationException))),
//                        Times.Once);

//            this.meshServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }
//    }
//}
