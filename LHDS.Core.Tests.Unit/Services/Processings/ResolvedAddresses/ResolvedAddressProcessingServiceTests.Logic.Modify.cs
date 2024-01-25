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
        public async Task ShouldModifyResolvedAddressAsync()
        {
            // Given
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress();
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            ResolvedAddress storageResolvedAddress = inputResolvedAddress;
            ResolvedAddress expectedResolvedAddress = storageResolvedAddress.DeepClone();

            this.resolvedAddressServiceMock.Setup(service =>
                service.ModifyResolvedAddressAsync(inputResolvedAddress))
                    .ReturnsAsync(storageResolvedAddress);

            // When
            await this.resolvedAddressProcessingService.ModifyResolvedAddressAsync(inputResolvedAddress);

            // Then
            this.resolvedAddressServiceMock.Verify(service =>
                service.ModifyResolvedAddressAsync(inputResolvedAddress),
                    Times.Once);

            this.resolvedAddressServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}