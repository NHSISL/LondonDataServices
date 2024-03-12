using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.SubscriberAgreements;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.SubscriberAgreements
{
    public partial class SubscriberAgreementsApiTests
    {
        [Fact]
        public async Task ShouldPostSubscriberAgreementAsync()
        {
            // given
            SubscriberAgreement randomSubscriberAgreement = CreateRandomSubscriberAgreement();
            SubscriberAgreement inputSubscriberAgreement = randomSubscriberAgreement;
            SubscriberAgreement expectedSubscriberAgreement = inputSubscriberAgreement;

            // when 
            await this.apiBroker.PostSubscriberAgreementAsync(inputSubscriberAgreement);

            SubscriberAgreement actualSubscriberAgreement =
                await this.apiBroker.GetSubscriberAgreementByIdAsync(inputSubscriberAgreement.Id);

            // then
            actualSubscriberAgreement.Should().BeEquivalentTo(expectedSubscriberAgreement);
            await this.apiBroker.DeleteSubscriberAgreementByIdAsync(actualSubscriberAgreement.Id);
        }
    }
}