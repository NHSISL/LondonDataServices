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
        public async Task ShouldAddAddressExtractionAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            AddressExtractionAudit randomAddressExtractionAudit = CreateRandomAddressExtractionAudit(randomDateTimeOffset);
            AddressExtractionAudit inputAddressExtractionAudit = randomAddressExtractionAudit;
            AddressExtractionAudit storageAddressExtractionAudit = inputAddressExtractionAudit;
            AddressExtractionAudit expectedAddressExtractionAudit = storageAddressExtractionAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAddressExtractionAuditAsync(inputAddressExtractionAudit))
                    .ReturnsAsync(storageAddressExtractionAudit);

            // when
            AddressExtractionAudit actualAddressExtractionAudit = await this.addressExtractionAuditService
                .AddAddressExtractionAuditAsync(inputAddressExtractionAudit);

            // then
            actualAddressExtractionAudit.Should().BeEquivalentTo(expectedAddressExtractionAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressExtractionAuditAsync(inputAddressExtractionAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}