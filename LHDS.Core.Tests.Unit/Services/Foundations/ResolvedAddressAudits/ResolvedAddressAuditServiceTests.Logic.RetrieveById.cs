// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveResolvedAddressAuditByIdAsync()
        {
            // given
            ResolvedAddressAudit randomResolvedAddressAudit = CreateRandomResolvedAddressAudit();
            ResolvedAddressAudit inputResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit storageResolvedAddressAudit = randomResolvedAddressAudit;
            ResolvedAddressAudit expectedResolvedAddressAudit = storageResolvedAddressAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(inputResolvedAddressAudit.Id))
                    .ReturnsAsync(storageResolvedAddressAudit);

            // when
            ResolvedAddressAudit actualResolvedAddressAudit =
                await this.resolvedAddressAuditService.RetrieveResolvedAddressAuditByIdAsync(inputResolvedAddressAudit.Id);

            // then
            actualResolvedAddressAudit.Should().BeEquivalentTo(expectedResolvedAddressAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectResolvedAddressAuditByIdAsync(inputResolvedAddressAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}