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
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfTerminologyPollIsNullAndLogItAsync()
        {
            // given
            TerminologyPoll nullTerminologyPoll = null;
            var nullTerminologyPollProcessingException =
                new NullTerminologyPollProcessingException(message: "Terminology poll is null.");

            var expectedTerminologyPollProcessingValidationException =
                new TerminologyPollProcessingValidationException(
                    message: "Terminology poll processing validation errors occurred, please try again.",
                    innerException: nullTerminologyPollProcessingException);

            // when
            ValueTask<TerminologyPoll> modifyTerminologyPollTask =
                this.terminologyPollProcessingService.ModifyTerminologyPollAsync(nullTerminologyPoll);

            TerminologyPollProcessingValidationException actualTerminologyPollProcessingValidationException =
                await Assert.ThrowsAsync<TerminologyPollProcessingValidationException>(
                    modifyTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollProcessingValidationException.Should()
                .BeEquivalentTo(expectedTerminologyPollProcessingValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedTerminologyPollProcessingValidationException))),
                        Times.Once);

            this.terminologyPollServiceMock.Verify(service =>
                service.ModifyTerminologyPollAsync(It.IsAny<TerminologyPoll>()),
                    Times.Never);

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}