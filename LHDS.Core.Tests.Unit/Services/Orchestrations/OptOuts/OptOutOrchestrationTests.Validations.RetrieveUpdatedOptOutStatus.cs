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
                service.RetrieveMessageIdsFromInboxAsync())
                    .ReturnsAsync(retrievedMessageIds)
                    .ReturnsAsync(new List<string>());

            retrievedMessage.Headers.Remove("mex-localid");

            this.meshProcessingServiceMock.Setup(service =>
                service.RetrieveMessageByIdAsync(It.IsAny<string>()))
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

            this.meshProcessingServiceMock.Verify(service =>
                service.RetrieveMessageByIdAsync(randomMessage.MessageId),
                    Times.Once);

            var expectedOptOutOrchestrationValidationLoggingException =
                new OptOutOrchestrationValidationException(
                    message: "Opt Out orchestration validation errors occurred, please try again.",
                    innerException: invalidMeshMessageOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedOptOutOrchestrationValidationLoggingException))),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
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
