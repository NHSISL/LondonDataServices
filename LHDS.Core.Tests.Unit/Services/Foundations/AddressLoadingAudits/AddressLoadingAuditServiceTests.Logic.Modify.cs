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
        public async Task ShouldModifyAddressLoadingAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomModifyAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit storageAddressLoadingAudit = inputAddressLoadingAudit.DeepClone();
            storageAddressLoadingAudit.UpdatedDate = randomAddressLoadingAudit.CreatedDate;
            AddressLoadingAudit updatedAddressLoadingAudit = inputAddressLoadingAudit;
            AddressLoadingAudit expectedAddressLoadingAudit = updatedAddressLoadingAudit.DeepClone();
            Guid addressLoadingAuditId = inputAddressLoadingAudit.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAddressLoadingAuditAsync(inputAddressLoadingAudit))
                    .ReturnsAsync(updatedAddressLoadingAudit);

            // when
            AddressLoadingAudit actualAddressLoadingAudit =
                await this.addressLoadingAuditService.ModifyAddressLoadingAuditAsync(inputAddressLoadingAudit);

            // then
            actualAddressLoadingAudit.Should().BeEquivalentTo(expectedAddressLoadingAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressLoadingAuditAsync(inputAddressLoadingAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}