// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task IsExactMatchForResolvedAddressAsShouldReturnTrueIfMatchFoundAsync()
        {
            // Given
            IQueryable<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses();
            IQueryable<ResolvedAddress> storageResolvedAddresses = randomResolvedAddresses;
            string randomAddress = randomResolvedAddresses.First().PostalAddress;
            string inputAddress = randomAddress;
            bool expectedResult = true;

            this.resolvedAddressServiceMock.Setup(broker =>
                broker.RetrieveAllResolvedAddresses())
                    .Returns(storageResolvedAddresses);

            // When
            (bool IsMatched, Guid? ItemId) actualResult = await this.resolvedAddressProcessingService
                .IsExactMatchForResolvedAddressAsync(inputAddress);

            // Then
            actualResult.IsMatched.Should().Be(expectedResult);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddresses(),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}