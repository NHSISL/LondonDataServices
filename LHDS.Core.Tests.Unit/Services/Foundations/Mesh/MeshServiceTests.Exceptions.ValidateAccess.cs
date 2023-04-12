//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System;
//using System.Threading.Tasks;
//using FluentAssertions;
//using LHDS.Core.Models.Foundations.Mesh.Exceptions;
//using Moq;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Foundations.Mesh
//{
//    public partial class MeshServiceTests
//    {
//        [Fact]
//        public async Task ShouldThrowServiceExceptionOnValidateAccessIfServiceErrorOccursAndLogItAsync()
//        {
//            // given
//            string exceptionMessage = GetRandomMessage();
//            var serviceException = new Exception(exceptionMessage);

//            var failedMeshServiceException =
//               new FailedMeshServiceException(serviceException);

//            var expectedMeshServiceException =
//               new MeshServiceException(failedMeshServiceException);

//            this.meshBrokerMock.Setup(broker =>
//                broker.ValidateAccessAsync())
//                    .ThrowsAsync(serviceException);

//            // when
//            ValueTask<bool> RetrieveValidationAccessTask =
//                this.meshService.ValidateMailboxAccessAsync();

//            MeshServiceException actualMeshServiceException =
//                await Assert.ThrowsAsync<MeshServiceException>(RetrieveValidationAccessTask.AsTask);

//            // then
//            actualMeshServiceException.Should()
//                .BeEquivalentTo(expectedMeshServiceException);

//            this.meshBrokerMock.Verify(broker =>
//                broker.ValidateAccessAsync(),
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
