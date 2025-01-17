// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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
        public async Task ShouldModifyResolvedAddressIfOneExistsAndNotAddAsync()
        {
            // Given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress storageResolvedAddress = randomResolvedAddress;
            ResolvedAddress modifiedResolvedAddress = storageResolvedAddress.DeepClone();
            modifiedResolvedAddress.UnstructuredPostalAddress = modifiedResolvedAddress.UnstructuredPostalAddress + "Modified";
            ResolvedAddress updatedResolvedAddress = modifiedResolvedAddress.DeepClone();
            ResolvedAddress expectedResolvedAddress = updatedResolvedAddress;
            List<ResolvedAddress> storageResolvedAddresses = new List<ResolvedAddress> { storageResolvedAddress };

            this.resolvedAddressServiceMock.Setup(service =>
                service.RetrieveAllResolvedAddressesAsync())
                    .ReturnsAsync(value: storageResolvedAddresses.AsQueryable());

            this.resolvedAddressServiceMock.Setup(service =>
                service.ModifyResolvedAddressAsync(modifiedResolvedAddress))
                    .ReturnsAsync(value: updatedResolvedAddress);

            // When
            await this.resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(modifiedResolvedAddress);

            // Then
            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.resolvedAddressServiceMock.Verify(service =>
                service.ModifyResolvedAddressAsync(modifiedResolvedAddress),
                    Times.Once);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(modifiedResolvedAddress),
                    Times.Never);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldAddResolvedAddressIfResolvedAddressDoesNotExistsAsync()
        {
            // Given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress.DeepClone();
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress;

            this.resolvedAddressServiceMock.Setup(service =>
                service.AddResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(value: storageResolvedAddress);

            // When
            await this.resolvedAddressProcessingService.ModifyOrAddResolvedAddressAsync(inputResolvedAddress);

            // Then
            this.resolvedAddressServiceMock.Verify(service =>
                service.RetrieveAllResolvedAddressesAsync(),
                    Times.Once);

            this.resolvedAddressServiceMock.Verify(service =>
                service.AddResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.resolvedAddressServiceMock.Verify(service =>
                service.ModifyResolvedAddressAsync(inputResolvedAddress),
                    Times.Never);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}