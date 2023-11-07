using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.TerminologyPolls;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.TerminologyPolls
{
    public partial class TerminologyPollProcessingServiceTests
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

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(storageTerminologyPoll);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollProcessingService
                .AddTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}