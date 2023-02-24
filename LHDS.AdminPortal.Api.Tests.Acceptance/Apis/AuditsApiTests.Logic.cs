using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.AdminPortal.Api.Tests.Acceptance.Models.Audits;
using Xunit;

namespace LHDS.AdminPortal.Api.Tests.Acceptance.Apis.Audits
{
    public partial class AuditsApiTests
    {
        [Fact]
        public async Task ShouldPostAuditAsync()
        {
            // given
            Audit randomAudit = CreateRandomAudit();
            Audit inputAudit = randomAudit;
            Audit expectedAudit = inputAudit;

            // when 
            await this.apiBroker.PostAuditAsync(inputAudit);

            Audit actualAudit =
                await this.apiBroker.GetAuditByIdAsync(inputAudit.Id);

            // then
            actualAudit.Should().BeEquivalentTo(expectedAudit);
            await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
        }

        [Fact]
        public async Task ShouldGetAllAuditsAsync()
        {
            // given
            List<Audit> randomAudits = await PostRandomAuditsAsync();
            List<Audit> expectedAudits = randomAudits;

            // when
            List<Audit> actualAudits = await this.apiBroker.GetAllAuditsAsync();

            // then
            foreach (Audit expectedAudit in expectedAudits)
            {
                Audit actualAudit = actualAudits.Single(approval => approval.Id == expectedAudit.Id);
                actualAudit.Should().BeEquivalentTo(expectedAudit);
                await this.apiBroker.DeleteAuditByIdAsync(actualAudit.Id);
            }
        }
    }
}