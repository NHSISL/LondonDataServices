//// ---------------------------------------------------------------
//// Copyright (c) North East London ICB. All rights reserved.
//// ---------------------------------------------------------------

//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using FluentAssertions;
//using LHDS.Core.Models.Processings.Mesh.Exceptions;
//using Moq;
//using Xeptions;
//using Xunit;

//namespace LHDS.Core.Tests.Unit.Services.Processings.Mesh
//{
//    public partial class MeshProcessingServiceTests
//    {
//        [Theory]
//        [MemberData(nameof(DependencyValidationExceptions))]
//        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveMessageIdsIfDependencyValidationErrorOccursAndLogItAsync(
//           Xeption dependencyValidationException)
//        {
//            // given
//            string mailboxId = GetRandomString();

//            var expectedMeshProcessingDependencyValidationException =
//                new MeshProcessingDependencyValidationException(
//                    dependencyValidationException.InnerException as Xeption);

//            this.meshServiceMock.Setup(service =>
//                service.RetrieveMessageIdsFromInboxAsync(mailboxId))
//                    .Throws(dependencyValidationException);

//            // when
//            ValueTask<List<string>> retrieveMessageByIdsTask =
//                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync(mailboxId);

//            MeshProcessingDependencyValidationException actualException =
//                await Assert.ThrowsAsync<MeshProcessingDependencyValidationException>(retrieveMessageByIdsTask.AsTask);

//            // then
//            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyValidationException);

//            this.meshServiceMock.Verify(service =>
//                service.RetrieveMessageIdsFromInboxAsync(mailboxId),
//                    Times.Once);

//            this.loggingBrokerMock.Verify(broker =>
//                 broker.LogError(It.Is(SameExceptionAs(
//                     expectedMeshProcessingDependencyValidationException))),
//                         Times.Once);

//            this.meshServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }

//        [Theory]
//        [MemberData(nameof(DependencyExceptions))]
//        public async Task ShouldThrowDependencyOnRetrieveMessageIdsIfDependencyErrorOccursAndLogItAsync(
//          Xeption dependencyException)
//        {
//            // given
//            string mailboxId = GetRandomString();

//            var expectedMeshProcessingDependencyException =
//                new MeshProcessingDependencyException(
//                    dependencyException.InnerException as Xeption);

//            this.meshServiceMock.Setup(service =>
//                service.RetrieveMessageIdsFromInboxAsync(mailboxId))
//                    .Throws(dependencyException);

//            // when
//            ValueTask<List<string>> retrieveMessageByIdsTask =
//                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync(mailboxId);

//            MeshProcessingDependencyException actualException =
//                await Assert.ThrowsAsync<MeshProcessingDependencyException>(retrieveMessageByIdsTask.AsTask);

//            // then
//            actualException.Should().BeEquivalentTo(expectedMeshProcessingDependencyException);

//            this.meshServiceMock.Verify(service =>
//                service.RetrieveMessageIdsFromInboxAsync(mailboxId),
//                    Times.Once);

//            this.loggingBrokerMock.Verify(broker =>
//                 broker.LogError(It.Is(SameExceptionAs(
//                     expectedMeshProcessingDependencyException))),
//                         Times.Once);

//            this.meshServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task ShouldThrowServiceExceptionOnRetrieveMessageIdsIfServiceErrorOccursAsync()
//        {
//            // given
//            string mailboxId = GetRandomString();

//            var serviceException = new Exception();

//            var failedMeshProcessingServiceException =
//                new FailedMeshProcessingServiceException(serviceException);

//            var expectedMeshProcessingServiveException =
//                new MeshProcessingServiceException(
//                    failedMeshProcessingServiceException);

//            this.meshServiceMock.Setup(service =>
//                service.RetrieveMessageIdsFromInboxAsync(mailboxId))
//                    .Throws(serviceException);

//            // when
//            ValueTask<List<string>> retrievRetrieveMessageIdsFromInboxTask =
//                this.meshProcessingService.RetrieveMessageIdsFromInboxAsync(mailboxId);

//            MeshProcessingServiceException actualException =
//                await Assert.ThrowsAsync<MeshProcessingServiceException>(retrievRetrieveMessageIdsFromInboxTask.AsTask);

//            // then
//            actualException.Should().BeEquivalentTo(expectedMeshProcessingServiveException);

//            this.meshServiceMock.Verify(service =>
//                service.RetrieveMessageIdsFromInboxAsync(mailboxId),
//                    Times.Once);

//            this.loggingBrokerMock.Verify(broker =>
//                 broker.LogError(It.Is(SameExceptionAs(
//                     expectedMeshProcessingServiveException))),
//                         Times.Once);

//            this.meshServiceMock.VerifyNoOtherCalls();
//            this.loggingBrokerMock.VerifyNoOtherCalls();
//        }

//    }
//}
