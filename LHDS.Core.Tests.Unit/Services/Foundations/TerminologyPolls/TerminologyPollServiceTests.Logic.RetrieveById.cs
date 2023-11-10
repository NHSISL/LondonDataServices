using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.TerminologyPolls
{
    public partial class TerminologyPollServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveTerminologyPollByIdAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(inputTerminologyPoll.Id))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll =
                await this.terminologyPollService.RetrieveTerminologyPollByIdAsync(inputTerminologyPoll.Id);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(inputTerminologyPoll.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}