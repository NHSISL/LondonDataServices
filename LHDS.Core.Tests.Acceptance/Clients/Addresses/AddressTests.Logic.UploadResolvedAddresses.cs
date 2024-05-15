// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LHDS.Core.Tests.Acceptance.Clients.Addresses
{
    public partial class AddressTests
    {
        [Fact]
        public async Task ShouldUploadResolvedAddressesAsync()
        {
            // Given
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            Guid expectedBatchReference = Guid.NewGuid();
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses(dateTimeOffset);

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                await this.resolvedAddressProcessingService.AddResolvedAddressAsync(resolvedAddress);
            }

            // When
            Guid actualBatchReference =
                await this.addressClient.ProcessResolvedAddressDataAsync();

            // Then
            actualBatchReference.Should().Be(expectedBatchReference);

            //this.resolvedAddressOrchestrationServiceMock.Verify(service =>
            //    service.UploadResolvedAddressesAsync(),
            //        Times.Once());

            //this.resolvedAddressOrchestrationServiceMock.VerifyNoOtherCalls();
            //this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            //this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
            //assert.items in file count rows.
        }
    }
}
