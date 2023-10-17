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
        public async Task ShouldAddAddressLoadingAuditAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset =
                GetRandomDateTimeOffset();

            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit storageAddressLoadingAudit = inputAddressLoadingAudit;
            AddressLoadingAudit expectedAddressLoadingAudit = storageAddressLoadingAudit.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAddressLoadingAuditAsync(inputAddressLoadingAudit))
                    .ReturnsAsync(storageAddressLoadingAudit);

            // when
            AddressLoadingAudit actualAddressLoadingAudit = await this.addressLoadingAuditService
                .AddAddressLoadingAuditAsync(inputAddressLoadingAudit);

            // then
            actualAddressLoadingAudit.Should().BeEquivalentTo(expectedAddressLoadingAudit);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAddressLoadingAuditAsync(inputAddressLoadingAudit),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}