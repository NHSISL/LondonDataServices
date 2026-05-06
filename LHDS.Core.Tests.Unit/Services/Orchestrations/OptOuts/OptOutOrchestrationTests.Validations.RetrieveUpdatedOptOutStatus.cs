// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Mesh;
using LHDS.Core.Models.Orchestrations.OptOuts.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.OptOuts
{
    public partial class OptOutOrchestrationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveUpdatedOptOutStatusIfHeadersInvalidAndLogItAsync()
        {
            // given
            string randomString = GetRandomString();
            MeshMessage randomMessage = CreateRandomMessage();
            randomMessage.MessageId = randomString;
            randomMessage.Headers["mex-workflowid"] = new List<string> { this.optOutConfiguration.WorkflowId };
            MeshMessage retrievedMessage = randomMessage;
            List<string> retrievedMessageIds = new List<string> { retrievedMessage.MessageId };

            this.meshProcessingServiceMock.SetupSequence(service =>
                service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(retrievedMessageIds)
                    .ReturnsAsync(new List<string>());

            retrievedMessage.Headers.Remove("mex-localid");

            string tempFilePath =
                System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.Guid.NewGuid().ToString());

            this.tempLocationBrokerMock.Setup(broker =>
                broker.GetUniqueHomeFilePath())
                    .Returns(tempFilePath);

            this.fileBrokerMock.Setup(broker =>
                broker.DeleteFileAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);

            this.meshProcessingServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(retrievedMessage);

            var invalidMeshMessageOrchestrationException =
                new InvalidMeshMessageOrchestrationException(
                    message: "Invalid mesh message orchestration error, please correct the errors and try again.");

            invalidMeshMessageOrchestrationException.AddData(
                key: "mex-localid",
                values: "Header value is required");

            var expectedOptOutOrchestrationValidationException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: invalidMeshMessageOrchestrationException);

            var aggregateException =
                new AggregateException(
                    $"Unable to retrieve message for 1 message IDs",
                    expectedOptOutOrchestrationValidationException);

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
                this.optOutOrchestrationService.RetrieveUpdatedMeshConsentStatusesChangesAsync(
                    TestContext.Current.CancellationToken);

            OptOutOrchestrationServiceException actualOptOutOrchestrationServiceException =
                await Assert.ThrowsAsync<OptOutOrchestrationServiceException>(actualMeshMessages.AsTask);

            // Then
            actualOptOutOrchestrationServiceException.Should()
                .BeEquivalentTo(expectedOptOutOrchestrationServiceException);

            this.meshProcessingServiceMock.Verify(service =>
                 service.RetrieveMessageIdsFromInboxAsync(It.IsAny<CancellationToken>()),
                     Times.Once);

            this.meshProcessingServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId, It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                    Times.Once);

            var expectedOptOutOrchestrationValidationLoggingException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: invalidMeshMessageOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutOrchestrationValidationLoggingException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedOptOutOrchestrationServiceException))),
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
