// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
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

            var invalidTerminologyPollException =
                new InvalidTerminologyPollException(
                    message: "Invalid terminology poll. Please correct the errors and try again.");

            invalidTerminologyPollException.AddData(
                key: nameof(TerminologyPoll.Id),
                values: "Id is required");

            // when
            ValueTask<TerminologyPoll> retrieveTerminologyPollByIDTask =
                this.terminologyPollProcessingService.RetrieveTerminologyPollByIdAsync(invalidTerminologyPollId);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(() =>
                    retrieveTerminologyPollByIDTask.AsTask());

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(invalidTerminologyPollException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    invalidTerminologyPollException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
        }

    }
}