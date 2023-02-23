using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.IngestionTrackings;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.IngestionTrackings
{
    public partial class IngestionTrackingsApiTests
    {
        [Fact]
        public async Task ShouldPostIngestionTrackingAsync()
        {
            // given
            IngestionTracking randomIngestionTracking = CreateRandomIngestionTracking();
            IngestionTracking inputIngestionTracking = randomIngestionTracking;
            IngestionTracking expectedIngestionTracking = inputIngestionTracking;

            // when 
            await this.apiBroker.PostIngestionTrackingAsync(inputIngestionTracking);

            IngestionTracking actualIngestionTracking =
                await this.apiBroker.GetIngestionTrackingByIdAsync(inputIngestionTracking.Id);

            // then
            actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
            await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
        }

        [Fact]
        public async Task ShouldGetAllIngestionTrackingsAsync()
        {
            // given
            List<IngestionTracking> randomIngestionTrackings = await PostRandomIngestionTrackingsAsync();
            List<IngestionTracking> expectedIngestionTrackings = randomIngestionTrackings;

            // when
            List<IngestionTracking> actualIngestionTrackings = await this.apiBroker.GetAllIngestionTrackingsAsync();

            // then
            foreach (IngestionTracking expectedIngestionTracking in expectedIngestionTrackings)
            {
                IngestionTracking actualIngestionTracking = actualIngestionTrackings.Single(approval => approval.Id == expectedIngestionTracking.Id);
                actualIngestionTracking.Should().BeEquivalentTo(expectedIngestionTracking);
                await this.apiBroker.DeleteIngestionTrackingByIdAsync(actualIngestionTracking.Id);
            }
        }
    }
}