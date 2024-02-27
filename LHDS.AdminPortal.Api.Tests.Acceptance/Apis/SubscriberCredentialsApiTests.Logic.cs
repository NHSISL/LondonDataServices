using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberCredentials;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberCredentials
{
    public partial class SubscriberCredentialsApiTests
    {
        [Fact]
        public async Task ShouldPostSubscriberCredentialAsync()
        {
            // given
            SubscriberCredential randomSubscriberCredential = CreateRandomSubscriberCredential();
            SubscriberCredential inputSubscriberCredential = randomSubscriberCredential;
            SubscriberCredential expectedSubscriberCredential = inputSubscriberCredential;

            // when 
            await this.apiBroker.PostSubscriberCredentialAsync(inputSubscriberCredential);

            SubscriberCredential actualSubscriberCredential =
                await this.apiBroker.GetSubscriberCredentialByIdAsync(inputSubscriberCredential.Id);

            // then
            actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
            await this.apiBroker.DeleteSubscriberCredentialByIdAsync(actualSubscriberCredential.Id);
        }

        [Fact]
        public async Task ShouldGetAllSubscriberCredentialsAsync()
        {
            // given
            List<SubscriberCredential> randomSubscriberCredentials = await PostRandomSubscriberCredentialsAsync();
            List<SubscriberCredential> expectedSubscriberCredentials = randomSubscriberCredentials;

            // when
            List<SubscriberCredential> actualSubscriberCredentials = await this.apiBroker.GetAllSubscriberCredentialsAsync();

            // then
            foreach (SubscriberCredential expectedSubscriberCredential in expectedSubscriberCredentials)
            {
                SubscriberCredential actualSubscriberCredential = actualSubscriberCredentials.Single(approval => approval.Id == expectedSubscriberCredential.Id);
                actualSubscriberCredential.Should().BeEquivalentTo(expectedSubscriberCredential);
                await this.apiBroker.DeleteSubscriberCredentialByIdAsync(actualSubscriberCredential.Id);
            }
        }
    }
}