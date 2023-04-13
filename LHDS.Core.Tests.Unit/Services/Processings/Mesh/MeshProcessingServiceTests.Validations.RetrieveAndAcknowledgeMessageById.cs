// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
{
    public partial class MeshProcessingServiceTests
    {
        //[Fact]
        //public async Task ShouldThrowValidationExceptionOnRetrieveMessagAndkAcknowledgeIfArgsIsInvalidAndLogItAsync()
        //{
        //    // given
        //    Message nullMessage = null;

        //    var invalidMeshProcessingArgumentException =
        //        new InvalidMeshProcessingArgumentException();

        //    var expectedMeshProcessingValidationException =
        //    new MeshProcessingValidationException(
        //           innerException: invalidMeshProcessingArgumentException,
        //           validationSummary: GetValidationSummary(invalidMeshProcessingArgumentException.Data));

        //    // when
        //    ValueTask<Message> retrieveMessageIdsFromInboxTask =
        //        this.meshProcessingService.RetrieveAndAcknowledgeMessageByIdAsync(nullMessage);

        //    MeshProcessingValidationException actualMeshProcessingValidationException =
        //        await Assert.ThrowsAsync<MeshProcessingValidationException>(retrieveMessageIdsFromInboxTask.AsTask);

        //    //then
        //    actualMeshProcessingValidationException.Should()
        //        .BeEquivalentTo(expectedMeshProcessingValidationException);

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogError(It.Is(SameExceptionAs(
        //            expectedMeshProcessingValidationException))),
        //                Times.Once);

        //    this.meshServiceMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //}
    }
}
