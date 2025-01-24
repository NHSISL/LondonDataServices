// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public async Task ShouldReturnPdsAuditsAsync()
        {
            // given
            IQueryable<PdsAudit> randomPdsAudits = CreateRandomPdsAudits();
            IQueryable<PdsAudit> storagePdsAudits = randomPdsAudits;
            IQueryable<PdsAudit> expectedPdsAudits = storagePdsAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ReturnsAsync(storagePdsAudits);

            // when
            IQueryable<PdsAudit> actualPdsAudits =
                await this.pdsAuditService.RetrieveAllPdsAuditsAsync();

            // then
            actualPdsAudits.Should().BeEquivalentTo(expectedPdsAudits);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAuditsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}