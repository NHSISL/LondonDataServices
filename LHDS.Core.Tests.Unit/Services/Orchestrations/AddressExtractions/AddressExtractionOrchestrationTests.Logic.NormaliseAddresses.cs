// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Extensions.Addresses;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldNormaliseAddressesAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Address randomAddress = CreateRandomAddress();
            randomAddress.IsNormalised = false;
            randomAddress.IsErrored = false;
            randomAddress.Processing = false;
            List<Address> randomAddresses = new List<Address> { randomAddress };

            AddressNormalisation addressNormalisation = new AddressNormalisation
            {
                PostalAddress = GetRandomString(),
                JsonPostalAddress = GetRandomString()
            };

            dateTimeBrokerMock
                .Setup(broker => broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            addressProcessingServiceMock
                .SetupSequence(service => service.RetrieveAllAddresses())
                    .Returns(randomAddresses.AsQueryable())
                    .Returns(new List<Address>().AsQueryable());

            var processingAddress = randomAddress.DeepClone();
            processingAddress.Processing = true;
            processingAddress.UpdatedDate = randomDateTimeOffset;

            addressProcessingServiceMock
            .Setup(service => service.ModifyAddressAsync(It.Is(SameAddressAs(processingAddress))))
                .ReturnsAsync(processingAddress);

            addressNormalisationProcessingServiceMock
                .Setup(service => service.GetNormalisedAddress(randomAddress.GetFormattedAddress()))
                    .ReturnsAsync(addressNormalisation);

            Address modifiedAddress = processingAddress.DeepClone();
            modifiedAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
            modifiedAddress.PostalAddress = addressNormalisation.PostalAddress;
            modifiedAddress.IsErrored = false;
            modifiedAddress.IsNormalised = true;
            modifiedAddress.Processing = false;
            modifiedAddress.UpdatedDate = randomDateTimeOffset;

            addressProcessingServiceMock
                .Setup(service => service.ModifyAddressAsync(modifiedAddress))
                    .ReturnsAsync(modifiedAddress);

            // When
            await this.addressExtractionOrchestrationService.NormaliseAddressesAsync();

            // Then
            addressProcessingServiceMock
                .Verify(service => service.RetrieveAllAddresses(),
                    Times.Exactly(2));

            addressProcessingServiceMock
                .Verify(service => service.ModifyAddressAsync(It.Is(SameAddressAs(processingAddress))),
                    Times.Once);

            addressNormalisationProcessingServiceMock
                .Verify(service => service.GetNormalisedAddress(randomAddress.GetFormattedAddress()),
                    Times.Once);

            dateTimeBrokerMock
                .Verify(broker => broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(2));

            addressProcessingServiceMock
                .Verify(service => service.ModifyAddressAsync(It.Is(SameAddressAs(modifiedAddress))),
                    Times.Once);

            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.addressProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

