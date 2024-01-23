using System.Linq;
using FluentAssertions;
using Moq;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public void ShouldReturnResolvedAddresses()
        {
            // given
            IQueryable<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            IQueryable<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses;
            IQueryable<ResolvedAddress> expectedResolvedAddresses = storageResolvedAddresses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddresses())
                    .Returns(storageResolvedAddresses);

            // when
            IQueryable<ResolvedAddress> actualResolvedAddresses =
                this.resolvedAddressService.RetrieveAllResolvedAddresses();

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddresses(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}