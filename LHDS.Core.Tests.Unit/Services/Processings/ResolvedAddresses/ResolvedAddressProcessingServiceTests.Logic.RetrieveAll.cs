// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllResolvedAddressesAsync()
        {
            // given
            IQueryable<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            IQueryable<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses;
            IQueryable<ResolvedAddress> expectedResolvedAddresses = storageResolvedAddresses;

            this.resolvedAddressServiceMock.Setup(broker =>
                broker.RetrieveAllResolvedAddressesAsync())
                    .ReturnsAsync(storageResolvedAddresses);

            // when
            IQueryable<ResolvedAddress> actualResolvedAddresses =
                await this.resolvedAddressProcessingService.RetrieveAllResolvedAddressesAsync();

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses);

            this.resolvedAddressServiceMock.Verify(broker =>
                broker.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}