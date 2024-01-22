using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditServiceTests
    {
        [Fact]
        public void ShouldReturnAddressLoadingAudits()
        {
            // given
            IQueryable<AddressLoadingAudit> randomAddressLoadingAudits = CreateRandomAddressLoadingAudits();
            IQueryable<AddressLoadingAudit> storageAddressLoadingAudits = randomAddressLoadingAudits;
            IQueryable<AddressLoadingAudit> expectedAddressLoadingAudits = storageAddressLoadingAudits;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAddressLoadingAudits())
                    .Returns(storageAddressLoadingAudits);

            // when
            IQueryable<AddressLoadingAudit> actualAddressLoadingAudits =
                this.addressLoadingAuditService.RetrieveAllAddressLoadingAudits();

            // then
            actualAddressLoadingAudits.Should().BeEquivalentTo(expectedAddressLoadingAudits);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAddressLoadingAudits(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}