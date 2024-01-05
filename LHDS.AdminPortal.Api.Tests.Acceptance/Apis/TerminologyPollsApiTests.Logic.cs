using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task ShouldGetAllTerminologyPollsAsync()
        {
            // given
            List<TerminologyPoll> randomTerminologyPolls = await PostRandomTerminologyPollsAsync();
            List<TerminologyPoll> expectedTerminologyPolls = randomTerminologyPolls;

            // when
            List<TerminologyPoll> actualTerminologyPolls = await this.apiBroker.GetAllTerminologyPollsAsync();

            // then
            foreach (TerminologyPoll expectedTerminologyPoll in expectedTerminologyPolls)
            {
                TerminologyPoll actualTerminologyPoll = actualTerminologyPolls.Single(approval => approval.Id == expectedTerminologyPoll.Id);
                actualTerminologyPoll.Should().BeEquivalentTo(expectedTerminologyPoll);
                await this.apiBroker.DeleteTerminologyPollByIdAsync(actualTerminologyPoll.Id);
            }
        }
    }
}