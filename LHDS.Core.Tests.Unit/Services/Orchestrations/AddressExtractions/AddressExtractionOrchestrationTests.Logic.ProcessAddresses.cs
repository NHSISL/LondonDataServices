// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
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
        public async Task ShouldProcessAddressesAsync()
        {
            // Given
            Guid randomId = Guid.NewGuid();
            string inputFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                 @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessResolvedAddressesAsync.csv");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> outputAddresses = randomAddresses.DeepClone();

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(inputData, inputFilename))
                    .ReturnsAsync(outputAddresses);

            List<Address> expectedAddresses = new List<Address>();

            foreach (Address address in outputAddresses)
            {
                string stringAddress = address.GetFormattedAddress();

                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    PostalAddress = GetRandomString(),
                    JsonPostalAddress = GetRandomString()
                };

                this.addressNormalisationServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ReturnsAsync(addressNormalisation);

                address.PostalAddress = addressNormalisation.PostalAddress;
                address.JsonPostalAddress = addressNormalisation.JsonPostalAddress;

                expectedAddresses.Add(address);
            }

            // When
            List<Address> actualAddresses = await this.addressExtractionOrchestrationService
                .ProcessAddressesAsync(inputData, inputFilename);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(inputData, inputFilename),
                    Times.Once());

            foreach (Address address in randomAddresses)
            {
                string stringAddress = address.GetFormattedAddress();

                this.addressNormalisationServiceMock.Verify(service =>
                    service.GetNormalisedAddress(stringAddress),
                        Times.Once);
            }

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

