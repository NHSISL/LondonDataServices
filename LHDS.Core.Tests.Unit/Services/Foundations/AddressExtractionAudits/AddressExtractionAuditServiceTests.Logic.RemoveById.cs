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
        public async Task ShouldRemoveAddressExtractionAuditByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputAddressExtractionAuditId = randomId;
            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit();
            AddressExtractionAudit storageAddressExtractionAudit = randomAddressExtractionAudit;
            AddressExtractionAudit expectedInputAddressExtractionAudit = storageAddressExtractionAudit;
            AddressExtractionAudit deletedAddressExtractionAudit = expectedInputAddressExtractionAudit;
            AddressExtractionAudit expectedAddressExtractionAudit = deletedAddressExtractionAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(inputAddressExtractionAuditId))
                    .ReturnsAsync(storageAddressExtractionAudit);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAddressExtractionAuditAsync(expectedInputAddressExtractionAudit))
                    .ReturnsAsync(deletedAddressExtractionAudit);

            // when
            AddressExtractionAudit actualAddressExtractionAudit = await this.addressExtractionAuditService
                .RemoveAddressExtractionAuditByIdAsync(inputAddressExtractionAuditId);

            // then
            actualAddressExtractionAudit.Should().BeEquivalentTo(expectedAddressExtractionAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAddressExtractionAuditByIdAsync(inputAddressExtractionAuditId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAddressExtractionAuditAsync(expectedInputAddressExtractionAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}