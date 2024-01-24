// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.ResolvedAddresses
{
    public partial class ResolvedAddressProcessingServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveResolvedAddressIfOneExistsAndNotAddAsync()
        {
            // Given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id))
                    .ReturnsAsync(value: storageResolvedAddress);

            // When
            await this.resolvedAddressProcessingService.RetrieveOrAddResolvedAddressAsync(inputResolvedAddress);

            // Then
            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(inputResolvedAddress),
                    Times.Never);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddResolvedAddressIfOneDoesNotExistAsync()
        {
            // Given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id))
                    .ReturnsAsync(value: null);

            this.resolvedAddressServiceMock.Setup(service =>
                service.AddResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(storageResolvedAddress);

            // When
            await this.resolvedAddressProcessingService.RetrieveOrAddResolvedAddressAsync(inputResolvedAddress);

            // Then
            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveResolvedAddressByIdAsync(inputResolvedAddress.Id),
                    Times.Once);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}