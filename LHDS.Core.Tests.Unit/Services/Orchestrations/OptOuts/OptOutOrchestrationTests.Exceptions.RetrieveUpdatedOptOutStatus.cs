// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationOnUpdateIfDependValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            var expectedDependencyException =
                new OptOutOrchestrationDependencyValidationException(
                   dependancyValidationException.InnerException as Xeption);

            this.meshProcessingServiceMock.Setup(processings =>
                processings.RetrieveMessageIdsFromInboxAsync())
                     .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask updateOptOutExpiredOptOutsToMeshIfExpiredTask =
                this.optOutOrchestrationService.RetrieveUpdatedMeshOptOutStatusChangesAsync();

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
                broker.LogError(It.Is(SameExceptionAs(
                    expectedDependencyException))),
                        Times.Once);

            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
        }

    }
}
