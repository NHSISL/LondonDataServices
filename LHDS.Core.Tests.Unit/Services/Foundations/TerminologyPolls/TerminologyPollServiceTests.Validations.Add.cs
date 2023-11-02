using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfTerminologyPollIsNullAndLogItAsync()
        {
            // given
            TerminologyPoll nullTerminologyPoll = null;

            var nullTerminologyPollException =
                new NullTerminologyPollException(message: "TerminologyPoll is null.");

            var expectedTerminologyPollValidationException =
                new TerminologyPollValidationException(
                    message: "TerminologyPoll validation errors occurred, please try again.",
                    innerException: nullTerminologyPollException);

            // when
            ValueTask<TerminologyPoll> addTerminologyPollTask =
                this.terminologyPollService.AddTerminologyPollAsync(nullTerminologyPoll);

            TerminologyPollValidationException actualTerminologyPollValidationException =
                await Assert.ThrowsAsync<TerminologyPollValidationException>(
                    addTerminologyPollTask.AsTask);

            // then
            actualTerminologyPollValidationException.Should().BeEquivalentTo(expectedTerminologyPollValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedTerminologyPollValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}