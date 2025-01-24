// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Moq;
using Xeptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Theory]
        [MemberData(nameof(OptOutDependencyValidationExceptions))]
        public async Task
            ShouldThrowAggregateDependencyValidationExceptionOnUpdateIfErrorsInLoopAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // Given
            List<string> randomMessageIds = GetRandomStrings(1);
            List<Exception> exceptions = new List<Exception>();

            this.meshProcessingServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(randomMessageIds)
                    .ReturnsAsync(new List<string>());

            foreach (var id in randomMessageIds)
            {
                this.meshProcessingServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(id))
                        .ThrowsAsync(dependencyValidationException);

                var optOutOrchestrationDependencyValidationException =
                    new OptOutOrchestrationDependencyValidationException(
                        message: "Opt Out orchestration dependency validation errors occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyValidationException.InnerException as Xeption);

                exceptions.Add(optOutOrchestrationDependencyValidationException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} message IDs",
                    exceptions);

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: failedOptOutOrchestrationServiceException);

            // When
            ValueTask<List<MeshMessage>> actualMeshMessages =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationServiceException actualOptOutOrchestrationServiceException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(async () =>
                    await actualMeshMessages);

            // Then
            actualOptOutOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.meshProcessingServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (var id in randomMessageIds)
            {
                this.meshProcessingServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(id),
                        Times.Once);
            }

            var optOutOrchestrationDependencyValidationLoggingException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    optOutOrchestrationDependencyValidationLoggingException))),
                        Times.Exactly(randomMessageIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualOptOutOrchestrationServiceException))),
                        Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OptOutDependencyExceptions))]
        public async Task
            ShouldThrowAggregateDependencyExceptionOnUpdateIfErrorsInLoopAndLogItAsync(
            Xeption dependencyException)
        {
            // Given
            List<string> randomMessageIds = GetRandomStrings(1);
            List<Exception> exceptions = new List<Exception>();

            this.meshProcessingServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(randomMessageIds)
                    .ReturnsAsync(new List<string>());

            foreach (var id in randomMessageIds)
            {
                this.meshProcessingServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(id))
                        .ThrowsAsync(dependencyException);

                var optOutOrchestrationDependencyException =
                    new OptOutOrchestrationDependencyException(
                        message: "Opt Out orchestration dependency error occurred, " +
                            "fix the errors and try again.",
                        innerException: dependencyException.InnerException as Xeption);

                exceptions.Add(optOutOrchestrationDependencyException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} message IDs",
                    exceptions);

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: failedOptOutOrchestrationServiceException);

            // When
            ValueTask<List<MeshMessage>> actualMeshMessages =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationServiceException actualOptOutOrchestrationServiceException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(async () =>
                    await actualMeshMessages);

            // Then
            actualOptOutOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.meshProcessingServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (var id in randomMessageIds)
            {
                this.meshProcessingServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(id),
                        Times.Once);
            }

            var optOutOrchestrationDependencyLoggingException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyException.InnerException as Xeption);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    optOutOrchestrationDependencyLoggingException))),
                        Times.Exactly(randomMessageIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    actualOptOutOrchestrationServiceException))),
                        Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowAggregateServiceExceptionOnUpdateIfErrorsInLoopAndLogItAsync()
        {
            // Given
            List<string> randomMessageIds = GetRandomStrings(1);
            List<Exception> exceptions = new List<Exception>();
            var serviceException = new Exception();

            this.meshProcessingServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(randomMessageIds)
                    .ReturnsAsync(new List<string>());

            var innerFailedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var innerOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: innerFailedOptOutOrchestrationServiceException);

            foreach (var id in randomMessageIds)
            {
                this.meshProcessingServiceMock.Setup(service =>
                    service.RetrieveMessageByIdAsync(id))
                        .ThrowsAsync(serviceException);

                exceptions.Add(innerOptOutOrchestrationServiceException);
            }

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for {exceptions.Count} message IDs",
                    exceptions);

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out aggregate orchestration service error occurred, " +
                        "please contact support.",
                    innerException: aggregateException);

            var expectedOptOutOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    innerException: failedOptOutOrchestrationServiceException);

            // When
            ValueTask<List<MeshMessage>> actualMeshMessages =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationServiceException actualOptOutOrchestrationServiceException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(async () =>
                    await actualMeshMessages);

            // Then
            actualOptOutOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.meshProcessingServiceMock.Verify(service =>
                service.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            foreach (var id in randomMessageIds)
            {
                this.meshProcessingServiceMock.Verify(service =>
                    service.RetrieveMessageByIdAsync(id),
                        Times.Once);
            }

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    innerOptOutOrchestrationServiceException))),
                        Times.Exactly(randomMessageIds.Count));

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutOrchestrationServiceException))),
                        Times.Once);

            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OptOutDependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnUpdateIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyValidationException(
                    message: "Opt Out orchestration dependency validation errors occurred, " +
                        "fix the errors and try again.",
                    innerException: dependancyValidationException.InnerException as Xeption);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask<List<MeshMessage>> updateOptOutExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyValidationException>(
                    updateOptOutExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(OptOutDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnUpdateIfDependencyExceptionOccursAndLogItAsync(
           Xeption dependancyException)
        {
            // Given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyException(
                    message: "Opt Out orchestration dependency error occurred, fix the errors and try again.",
                    innerException: dependancyException.InnerException as Xeption);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                    .ThrowsAsync(dependancyException);

            // When
            ValueTask<List<MeshMessage>> updateOptOutExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationDependencyException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyException>(
                    updateOptOutExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                .BeEquivalentTo(expectedDependencyException);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnUpdateIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceException = new Exception();

            var failedOptOutOrchestrationServiceException =
                new FailedOptOutOrchestrationServiceException(
                    message: "Failed opt out orchestration service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedOptOrchestrationServiceException =
                new OptOutOrchestrationServiceException(
                    message: "Opt Out orchestration service error occurred, please contact support.",
                    failedOptOutOrchestrationServiceException);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                   .ThrowsAsync(serviceException);

            // when
            ValueTask<List<MeshMessage>> updateOptOutExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync();

            OptOutOrchestrationServiceException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(
                    updateOptOutExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // then
            actualException.Should()
                .BeEquivalentTo(expectedOptOrchestrationServiceException);

            this.meshProcessingServiceMock.Verify(processings =>
                processings.RetrieveMessageIdsFromInboxAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOrchestrationServiceException))),
                        Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
