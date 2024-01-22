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
        public async Task ShouldAddTerminologyPollAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll(randomDateTimeOffset);
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollService
                .AddTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}