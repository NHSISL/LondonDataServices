// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using LHDS.Core.Models.Orchestrations.Pds.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.Pds
{
    public partial class PdsOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(PdsDependencyValidationExceptions))]
        public async Task
            ShouldThrowAggregateDependencyValidationExceptionOnRetrieveAndUpdateIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            List<string> randomMessageIds = GetRandomStrings(1);
            List<Exception> exceptions = new List<Exception>();

            this.meshServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(randomMessageIds)
                    .ReturnsAsync(new List<string>());

            foreach (var id in randomMessageIds)
            {
                string tempFilePath =
                    System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid().ToString());

                this.tempLocationBrokerMock.Setup(broker =>
                    broker.GetUniqueHomeFilePath())
                        .Returns(tempFilePath);

                this.fileBrokerMock.Setup(broker =>
                    broker.DeleteFileAsync(It.IsAny<string>()))
                        .ReturnsAsync(true);

                this.meshServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(id, It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(dependencyValidationException);

                var pdsOrchestrationDependencyValidationException =
                    new PdsOrchestrationDependencyValidationException(
                        message: "PDS orchestration dependency validation errors occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(pdsOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} message IDs",
                    exceptions);

            var failedPdsOrchestrationServiceException =
                new FailedPdsOrchestrationServiceException(
                    message: "Failed PDS aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedPdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: failedPdsOrchestrationServiceException);

            // When
            ValueTask<List<PdsAudit>> retreiveMessagesFromMeshAndUpdateStorageTask =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationServiceException actualPdsOrchestrationServiceException =
                await Assert.ThrowsAsync<PdsOrchestrationServiceException>(
                    retreiveMessagesFromMeshAndUpdateStorageTask.AsTask);

            // Then
            actualPdsOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationServiceException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            foreach (var id in randomMessageIds)
            {
                this.meshServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(id, It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                        Times.Once);
            }

            var pdsOrchestrationDependencyValidationLoggingException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    pdsOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(randomMessageIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualPdsOrchestrationServiceException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(PdsDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnRetrieveAndUpdateIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            List<string> randomMessageIds = GetRandomStrings(GetRandomNumber());
            List<Exception> exceptions = new List<Exception>();

            this.meshServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(randomMessageIds)
                    .ReturnsAsync(new List<string>());

            foreach (var id in randomMessageIds)
            {
                string tempFilePath =
                    System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid().ToString());

                this.tempLocationBrokerMock.Setup(broker =>
                    broker.GetUniqueHomeFilePath())
                        .Returns(tempFilePath);

                this.fileBrokerMock.Setup(broker =>
                    broker.DeleteFileAsync(It.IsAny<string>()))
                        .ReturnsAsync(true);

                this.meshServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(id, It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(dependencyException);

                var pdsOrchestrationDependencyException =
                    new PdsOrchestrationDependencyException(
                        message: "PDS orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(pdsOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} message IDs",
                    exceptions);

            var failedPdsOrchestrationServiceException =
                new FailedPdsOrchestrationServiceException(
                    message: "Failed PDS aggregate orchestration service error occurred, " +
                    "please contact support.",
                    innerException: aggregateException);

            var expectedPdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: failedPdsOrchestrationServiceException);

            // When
            ValueTask<List<PdsAudit>> retreiveMessagesFromMeshAndUpdateStorageTask =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationServiceException actualPdsOrchestrationServiceException =
                await Assert.ThrowsAsync<PdsOrchestrationServiceException>(
                    retreiveMessagesFromMeshAndUpdateStorageTask.AsTask);

            // Then
            actualPdsOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationServiceException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            foreach (var id in randomMessageIds)
            {
                this.meshServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(id, It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                        Times.Once);
            }

            var pdsOrchestrationDependencyLoggingException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, " +
                    "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    pdsOrchestrationDependencyLoggingException))),
                        Times.Exactly(randomMessageIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualPdsOrchestrationServiceException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnRetrieveAndUpdateIfErrorsInLoopAndLogItAsync()
        {
            // Given
            List<string> randomMessageIds = GetRandomStrings(GetRandomNumber());
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();

            this.meshServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(randomMessageIds)
                    .ReturnsAsync(new List<string>());

            var innerFailedPdsOrchestrationServiceException =
                new FailedPdsOrchestrationServiceException(
                    message: "Failed PDS orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerPdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: innerFailedPdsOrchestrationServiceException);

            foreach (var id in randomMessageIds)
            {
                string tempFilePath =
                    System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid().ToString());

                this.tempLocationBrokerMock.Setup(broker =>
                    broker.GetUniqueHomeFilePath())
                        .Returns(tempFilePath);

                this.fileBrokerMock.Setup(broker =>
                    broker.DeleteFileAsync(It.IsAny<string>()))
                        .ReturnsAsync(true);

                this.meshServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(id, It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerPdsOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} message IDs",
                    exceptions);

            var failedPdsOrchestrationServiceException =
                new FailedPdsOrchestrationServiceException(
                    message: "Failed PDS aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedPdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: failedPdsOrchestrationServiceException);

            // When
            ValueTask<List<PdsAudit>> retreiveMessagesFromMeshAndUpdateStorageTask =
                 this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationServiceException actualPdsOrchestrationServiceException =
                await Assert.ThrowsAsync<PdsOrchestrationServiceException>(
                    retreiveMessagesFromMeshAndUpdateStorageTask.AsTask);

            // Then
            actualPdsOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationServiceException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            foreach (var id in randomMessageIds)
            {
                this.meshServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(id, It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                        Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    innerPdsOrchestrationServiceException))),
                        Times.Exactly(randomMessageIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsOrchestrationServiceException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(PdsDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnRetrieveAndUpdateIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            var expectedDependencyException =
                new PdsOrchestrationDependencyValidationException(
                    message: "PDS orchestration dependency validation errors occurred, fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
              service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(dependancyValidationException);

            //when
            ValueTask<List<PdsAudit>> retreiveMessagesFromMeshAndUpdateStorageTask =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationDependencyValidationException>(
                    retreiveMessagesFromMeshAndUpdateStorageTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshServiceMock.Verify(broker =>
                broker.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(PdsDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAndUpdateIfDependencyExceptionOccursAndLogItAsync(
         Xeption dependancyException)
        {
            // given
            var expectedDependencyException =
                new PdsOrchestrationDependencyException(
                    message: "PDS orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(dependancyException);

            // when
            ValueTask<List<PdsAudit>> retreiveMessagesFromMeshAndUpdateStorageTask =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationDependencyException>(
                    retreiveMessagesFromMeshAndUpdateStorageTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAndUpdateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedPdsOrchestrationServiceException =
                new FailedPdsOrchestrationServiceException(
                    message: "Failed PDS orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedPdsOrchestrationServiceException =
                new PdsOrchestrationServiceException(
                    message: "PDS orchestration service error occurred, please contact support.",
                    innerException: failedPdsOrchestrationServiceException);

            this.meshServiceMock.Setup(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<List<PdsAudit>> retreiveMessagesFromMeshAndUpdateStorageTask =
                this.pdsOrchestrationService.RetreiveMessagesFromMeshAndUpdateStorage();

            PdsOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<PdsOrchestrationServiceException>(
                    retreiveMessagesFromMeshAndUpdateStorageTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedPdsOrchestrationServiceException);

            this.meshServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsOrchestrationServiceException))),
                        Times.Once);

            this.meshServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.pdsAuditServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
