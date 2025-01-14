// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressServiceTests
    {
        [Fact]
        public async Task ShouldReturnResolvedAddressesAsync()
        {
            // given
            IQueryable<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            IQueryable<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses;
            IQueryable<ResolvedAddress> expectedResolvedAddresses = storageResolvedAddresses;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllResolvedAddressesAsync())
                    .ReturnsAsync(storageResolvedAddresses);

            // when
            IQueryable<ResolvedAddress> actualResolvedAddresses =
                await this.resolvedAddressService.RetrieveAllResolvedAddressesAsync();

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllResolvedAddressesAsync(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}