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
        public async Task ShouldThrowValidationExceptionOnRemoveByIdIfTerminologyPollIdIsNullAndLogItAsync()
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
            ValueTask<TerminologyPoll> removeTerminologyPollByIDTask =
                this.terminologyPollProcessingService.RemoveTerminologyPollByIdAsync(invalidTerminologyPollId);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(
                    removeTerminologyPollByIDTask.AsTask);

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.terminologyPollServiceMock.Verify(service =>
                service.RemoveTerminologyPollByIdAsync(invalidTerminologyPollId),
                    Times.Never);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}