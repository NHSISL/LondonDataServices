// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressPersistances
{
    public partial class AddressPersistanceOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldMatchAndPersistResolvedAddressAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ResolvedAddress randomResolvedAddress = CreateRandomResolvedAddress(randomDateTimeOffset);
            ResolvedAddress inputResolvedAddress = randomResolvedAddress;
            List<Address> randomAddresses = CreateRandomAddressList();
            List<Address> storageAddresses = randomAddresses;

            this.addressProcessingServiceMock.Setup(processing =>
                processing.RetrieveAddressByPostCodeAsync(inputResolvedAddress.PostCode))
                    .ReturnsAsync(storageAddresses);

            this.addressMatcherProcessingServiceMock.Setup(processing =>
                processing.FindBestMatch())
                    .ReturnsAsync(inputResolvedAddress);

            // When
            ResolvedAddress actualResolvedAddress =
                await this.addressPersistanceOrchestrationService.MatchAndPersistResolvedAddressAsync(inputResolvedAddress);

            // Then



            addressProcessingServiceMock.VerifyNoOtherCalls();
            addressMatcherProcessingServiceMock.VerifyNoOtherCalls();
            loggingBrokerMock.VerifyNoOtherCalls();
            dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}

