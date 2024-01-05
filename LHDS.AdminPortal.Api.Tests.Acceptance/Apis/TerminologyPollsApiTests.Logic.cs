using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.TerminologyPolls;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.TerminologyPolls
{
    public partial class TerminologyPollsApiTests
    {
        [Fact]
        public async Task ShouldPostTerminologyPollAsync()
        {
            // given
            TerminologyPoll randomTerminologyPoll = CreateRandomTerminologyPoll();
            TerminologyPoll inputTerminologyPoll = randomTerminologyPoll;
            TerminologyPoll expectedTerminologyPoll = inputTerminologyPoll;

            // when 
            await this.apiBroker.PostTerminologyPollAsync(inputTerminologyPoll);

            TerminologyPoll actualTerminologyPoll =
                await this.apiBroker.GetTerminologyPollByIdAsync(inputTerminologyPoll.Id);

            // then
            actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);
            await this.apiBroker.DeleteTerminologyPollByIdAsync(actualTerminologyPoll.Id);
        }
    }
}