// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAddressLoadingAuditByIdAsync()
        {
            // given
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            AddressLoadingAudit inputAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit storageAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit expectedAddressLoadingAudit = storageAddressLoadingAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(inputAddressLoadingAudit.Id))
                    .ReturnsAsync(storageAddressLoadingAudit);

            // when
            AddressLoadingAudit actualAddressLoadingAudit =
                await this.addressLoadingAuditService.RetrieveAddressLoadingAuditByIdAsync(inputAddressLoadingAudit.Id);

            // then
            actualAddressLoadingAudit.Should().BeEquivalentTo(expectedAddressLoadingAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(inputAddressLoadingAudit.Id),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}