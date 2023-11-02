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
        public async Task ShouldModifyTerminologyPollAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            TerminologyPoll randomTerminologyPoll = CreateRandomModifyTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll.DeepClone();
            storageTerminologyPoll.UpdatedDate = randomTerminologyPoll.CreatedDate;
            TerminologyPoll updatedTerminologyPoll = inputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = updatedTerminologyPoll.DeepClone();
            Guid terminologyPollId = inputTerminologyPoll.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(updatedTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll =
                await this.terminologyPollService.ModifyTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}