// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Processings.TerminologyPolls.Exceptions;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfTerminologyPollIdIsNullAndLogItAsync()
        {
            // given
            Guid invalidTerminologyPollId = Guid.Empty;

            var invalidArgumentTerminologyPollsProcessingException =
                new InvalidArgumentTerminologyPollsProcessingException(
                    message: "Invalid argument terminology poll processing. Please correct the errors and try again.");

            invalidArgumentTerminologyPollsProcessingException.AddData(
                key: nameof(TerminologyPoll.Id),
                values: "Id is required");

            var expectedTerminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: invalidArgumentTerminologyPollsProcessingException);

            // when
            ValueTask<TerminologyPoll> retrieveTerminologyPollByIDTask =
                this.terminologyPollProcessingService.RetrieveTerminologyPollByIdAsync(invalidTerminologyPollId);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(
                    retrieveTerminologyPollByIDTask.AsTask);

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
        }
    }
}