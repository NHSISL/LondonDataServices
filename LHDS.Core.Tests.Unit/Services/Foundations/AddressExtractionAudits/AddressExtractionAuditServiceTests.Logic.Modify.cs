using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditServiceTests
    {
        [Fact]
        public async Task ShouldModifyAddressExtractionAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomModifyAddressExtractionAudit(randomDateTimeOffset);
            AddressExtractionAudit inputAddressExtractionAudit = randomAddressExtractionAudit;
            AddressExtractionAudit storageAddressExtractionAudit = inputAddressExtractionAudit.DeepClone();
            storageAddressExtractionAudit.UpdatedDate = randomAddressExtractionAudit.CreatedDate;
            AddressExtractionAudit updatedAddressExtractionAudit = inputAddressExtractionAudit;
            AddressExtractionAudit expectedAddressExtractionAudit = updatedAddressExtractionAudit.DeepClone();
            Guid addressExtractionAuditId = inputAddressExtractionAudit.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAddressExtractionAuditAsync(inputAddressExtractionAudit))
                    .ReturnsAsync(updatedAddressExtractionAudit);

            // when
            AddressExtractionAudit actualAddressExtractionAudit =
                await this.addressExtractionAuditService.ModifyAddressExtractionAuditAsync(inputAddressExtractionAudit);

            // then
            actualAddressExtractionAudit.Should().BeEquivalentTo(expectedAddressExtractionAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAddressExtractionAuditAsync(inputAddressExtractionAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}