// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRemoveResolvedAddressByIdAsync()
        {
            // Given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress storageResolvedAddress = randomResolvedAddress;
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();

            this.resolvedAddressServiceMock.Setup(service =>
                service.RemoveResolvedAddressByIdAsync(randomResolvedAddress.Id))
                    .ReturnsAsync(storageResolvedAddress);

            // When
            ResolvedAddress actualResolvedAddress =
                await this.resolvedAddressProcessingService
                    .RemoveResolvedAddressByIdAsync(randomResolvedAddress.Id);

            // Then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddress);

            this.resolvedAddressServiceMock.Verify(service =>
                service.RemoveResolvedAddressByIdAsync(randomResolvedAddress.Id),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}