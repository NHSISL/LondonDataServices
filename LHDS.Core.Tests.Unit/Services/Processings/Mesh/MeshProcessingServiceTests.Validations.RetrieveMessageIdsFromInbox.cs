//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System.Collections.Generic;
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
//        public async Task ShouldThrowValidationExceptionOnRetrieveMessageIdsFromInboxIfArgsIsInvalidAndLogItAsync(
//            string invalidText)
//        {
//            // given
//            string mailboxId = invalidText;

//            var invalidMeshProcessingArgumentException =
//                new InvalidMeshProcessingArgumentException();

//            invalidMeshProcessingArgumentException.AddData(
//                key: nameof(mailboxId),
//                values: "Text is required");

//            var expectedMeshProcessingValidationException =
//            new MeshProcessingValidationException(
//                   innerException: invalidMeshProcessingArgumentException,
//                   validationSummary: GetValidationSummary(invalidMeshProcessingArgumentException.Data));

//            // when
//            ValueTask<List<string>> retrieveMessageIdsFromInboxTask =
//                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync(mailboxId);

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
