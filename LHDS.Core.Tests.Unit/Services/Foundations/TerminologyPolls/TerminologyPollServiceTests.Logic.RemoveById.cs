using System;
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
        public async Task ShouldRemoveTerminologyPollByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputTerminologyPollId = randomId;
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll storageTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll expectedInputTerminologyPoll = storageTerminologyPoll;
            TerminologyPoll deletedTerminologyPoll = expectedInputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = deletedTerminologyPoll.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTerminologyPollByIdAsync(inputTerminologyPollId))
                    .ReturnsAsync(storageTerminologyPoll);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTerminologyPollAsync(expectedInputTerminologyPoll))
                    .ReturnsAsync(deletedTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollService
                .RemoveTerminologyPollByIdAsync(inputTerminologyPollId);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTerminologyPollByIdAsync(inputTerminologyPollId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTerminologyPollAsync(expectedInputTerminologyPoll),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}