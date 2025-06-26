// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldReturnResolvedAddressAuditsAsync()
        {
            // given
            IQueryable<ResolvedAddressAudit> randomResolvedAddressAudits = CreateRandomResolvedAddressAudits();
            IQueryable<ResolvedAddressAudit> storageResolvedAddressAudits = randomResolvedAddressAudits;
            IQueryable<ResolvedAddressAudit> expectedResolvedAddressAudits = storageResolvedAddressAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressAuditsAsync())
                    .ReturnsAsync(storageResolvedAddressAudits);

            // when
            IQueryable<ResolvedAddressAudit> actualResolvedAddressAudits =
                await this.resolvedAddressAuditService.RetrieveAllResolvedAddressAuditsAsync();

            // then
            actualResolvedAddressAudits.Should().BeEquivalentTo(expectedResolvedAddressAudits);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddressAuditsAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}