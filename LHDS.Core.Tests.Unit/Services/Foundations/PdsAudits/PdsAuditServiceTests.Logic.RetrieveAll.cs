// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.PdsAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.PdsAudits
{
    public partial class PdsAuditServiceTests
    {
        [Fact]
        public void ShouldReturnPdsAudits()
        {
            // given
            IQueryable<PdsAudit> randomPdsAudits = CreateRandomPdsAudits();
            IQueryable<PdsAudit> storagePdsAudits = randomPdsAudits;
            IQueryable<PdsAudit> expectedPdsAudits = storagePdsAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllPdsAudits())
                    .Returns(storagePdsAudits);

            // when
            IQueryable<PdsAudit> actualPdsAudits =
                this.pdsAuditService.RetrieveAllPdsAudits();

            // then
            actualPdsAudits.Should().BeEquivalentTo(expectedPdsAudits);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllPdsAudits(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}