// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllResolvedAddresses()
        {
            // given
            IQueryable<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            IQueryable<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses;
            IQueryable<ResolvedAddress> expectedResolvedAddresses = storageResolvedAddresses;

            this.resolvedAddressServiceMock.Setup(broker =>
                broker.RetrieveAllResolvedAddresses())
                    .Returns(storageResolvedAddresses);

            // when
            IQueryable<ResolvedAddress> actualResolvedAddresses =
                this.resolvedAddressProcessingService.RetrieveAllResolvedAddresses();

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses);

            this.resolvedAddressServiceMock.Verify(broker =>
                broker.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}