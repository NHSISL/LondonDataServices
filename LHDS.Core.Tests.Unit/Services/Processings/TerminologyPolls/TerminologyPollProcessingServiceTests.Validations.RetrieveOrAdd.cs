// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRetrieveOrAddIfResourceTypeIsInvalidAndLogItAsync(
            string invalidString)
        {
            // given
            string inputString = invalidString;

            var invalidArgumentTerminologyPollsProcessingException =
                new InvalidArgumentTerminologyPollsProcessingException(
                    message: "Invalid argument terminology poll processing. Please correct the errors and try again.");

            invalidArgumentTerminologyPollsProcessingException.AddData(
                key: "resourceType",
                values: "Text is required");

            var expectedTerminologyPollProcessingValidationException =
            new TerminologyPollProcessingValidationException(
                message: "Terminology poll processing validation errors occurred, please try again.",
                innerException: invalidArgumentTerminologyPollsProcessingException);

            // when
            ValueTask<TerminologyPoll> retrieveOrAddTerminologyPollTask =
                this.terminologyPollProcessingService.RetrieveOrAddTerminologyPollAsync(inputString);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(
                    retrieveOrAddTerminologyPollTask.AsTask);

            //then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}