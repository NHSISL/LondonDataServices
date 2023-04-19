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
        public async Task ShouldThrowDependencyValidationOnPushExpiredOptOutsToMeshIfExpiredIfDependencyValidationOccursAndLogItAsync(
           Xeption dependancyValidationException)
        {
            // Given
            var expectedDependencyException =
               new OptOutOrchestrationDependencyValidationException(
                   dependancyValidationException.InnerException as Xeption);

            this.optOutProcessingServiceMock.Setup(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()))
                    .ThrowsAsync(dependancyValidationException);

            // When
            ValueTask pushExpiredOptOutsToMeshIfExpiredTask =
               this.optOutOrchestrationService.PushExpiredOptOutsToMeshForRenewalAsync();

            OptOutOrchestrationDependencyValidationException actualException =
                await Assert.ThrowsAsync<OptOutOrchestrationDependencyValidationException>(
                    pushExpiredOptOutsToMeshIfExpiredTask.AsTask);

            // Then
            actualException.Should()
                 .BeEquivalentTo(expectedDependencyException);

            this.optOutProcessingServiceMock.Verify(processings =>
                processings.RetrieveAllExpiredOptOutsAsync(It.IsAny<int>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogError(It.Is(SameExceptionAs(
                  expectedDependencyException))),
                      Times.Once);

            this.optOutProcessingServiceMock.VerifyNoOtherCalls();
            this.csvMapperProcessingServiceMock.VerifyNoOtherCalls();
            this.meshProcessingServiceMock.VerifyNoOtherCalls();
            this.documentProcessingServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}
