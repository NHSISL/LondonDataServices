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
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll storageTerminologyPoll = inputTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = storageTerminologyPoll.DeepClone();

            this.terminologyPollServiceMock.Setup(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll))
                    .ReturnsAsync(storageTerminologyPoll);

            // when
            TerminologyPoll actualTerminologyPoll = await this.terminologyPollProcessingService
                .AddTerminologyPollAsync(inputTerminologyPoll);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);

            this.terminologyPollServiceMock.Verify(service =>
                service.AddTerminologyPollAsync(inputTerminologyPoll),
                    Times.Once());

            this.terminologyPollServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}