// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessAddressesAsync.csv");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> outputAddresses = randomAddresses.DeepClone();
            List<AddressNormalisation> outputAddressNormalisations

            this.addressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(inputData))
                    .ReturnsAsync(outputAddresses);

            foreach(Address address in outputAddresses) 
            {
                this.addressNormalisationServiceMock
            }

            List<Address> expectedAddresses = outputAddresses.DeepClone();

            // When
            List<Address> actualAddresses = await this.addressExtractionOrchestrationService
                .ProcessDataAsync(inputData);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            await VerifyMocksForProvidedZips(randomDateTimeOffset, randomId, inputData);

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

