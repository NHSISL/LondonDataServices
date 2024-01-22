using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public async Task ShouldRemoveAddressLoadingAuditByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputAddressLoadingAuditId = randomId;
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit();
            AddressLoadingAudit storageAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit expectedInputAddressLoadingAudit = storageAddressLoadingAudit;
            AddressLoadingAudit deletedAddressLoadingAudit = expectedInputAddressLoadingAudit;
            AddressLoadingAudit expectedAddressLoadingAudit = deletedAddressLoadingAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(inputAddressLoadingAuditId))
                    .ReturnsAsync(storageAddressLoadingAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAddressLoadingAuditAsync(expectedInputAddressLoadingAudit))
                    .ReturnsAsync(deletedAddressLoadingAudit);

            // when
            AddressLoadingAudit actualAddressLoadingAudit = await this.addressLoadingAuditService
                .RemoveAddressLoadingAuditByIdAsync(inputAddressLoadingAuditId);

            // then
            actualAddressLoadingAudit.Should().BeEquivalentTo(expectedAddressLoadingAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressLoadingAuditByIdAsync(inputAddressLoadingAuditId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressLoadingAuditAsync(expectedInputAddressLoadingAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}