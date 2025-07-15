// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldReturnPdsAuditsByCorrelationAsync()
        {
            // given
            Guid randomCorrelationId = Guid.NewGuid();
            Guid inputCorrelationId = randomCorrelationId;
            IQueryable<PdsAudit> randomPdsAuditsWithCorrelationId = CreateRandomPdsAuditsWithCorrelationId(inputCorrelationId);
            IQueryable<PdsAudit> randomPdsAudits = CreateRandomPdsAudits();
            IQueryable<PdsAudit> allPdsAudits = randomPdsAuditsWithCorrelationId.Concat(randomPdsAudits);
            IQueryable<PdsAudit> storagePdsAudits = allPdsAudits;
            IQueryable<PdsAudit> expectedPdsAudits = randomPdsAuditsWithCorrelationId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAuditsAsync())
                    .ReturnsAsync(storagePdsAudits);

            // when
            IQueryable<PdsAudit> actualPdsAudits =
                await this.pdsAuditService.RetrieveAllPdsAuditsByCorrelationIdAsync(inputCorrelationId);

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