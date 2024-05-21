// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExtractionOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessResolvedAddressesAsync()
        {
            // Given
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Orchestrations/AddressExtractions/ShouldProcessResolvedAddressesAsync.csv");

            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);
            string stringData = Encoding.UTF8.GetString(inputData);
            bool hasHeaderRecord = true;
            string randomFilename = GetRandomString();
            Guid identifier = Guid.NewGuid();

            Dictionary<string, int> fieldMappings = new Dictionary<string, int>
                {
                    { nameof(ResolvedAddress.UniqueReference), 0 },
                    { nameof(ResolvedAddress.PostCode), 1 },
                    { nameof(ResolvedAddress.PostalAddress), 2 }
                };

            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses().ToList();
            List<ResolvedAddress> outputResolvedAddresses = randomResolvedAddresses.DeepClone();

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(identifier);

            this.csvHelperBrokerMock.Setup(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(stringData, hasHeaderRecord, fieldMappings))
                    .ReturnsAsync(outputResolvedAddresses);

            List<ResolvedAddress> expectedResolvedAddresses = new List<ResolvedAddress>();

            foreach (ResolvedAddress resolvedAddress in outputResolvedAddresses)
            {
                string stringAddress = resolvedAddress.PostalAddress ?? string.Empty;

                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    PostalAddress = GetRandomString(),
                    JsonPostalAddress = GetRandomString()
                };

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ReturnsAsync(addressNormalisation);

                resolvedAddress.UnstructuredPostalAddress = addressNormalisation.PostalAddress ?? string.Empty;
                resolvedAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;
                expectedResolvedAddresses.Add(resolvedAddress);
            }

            // When
            List<ResolvedAddress> actualResolvedAddress = await this.addressExtractionOrchestrationService
                .ProcessResolvedAddressesAsync(inputData, randomFilename);

            // Then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddresses, options =>
                options.Excluding(address => address.Id));

            this.csvHelperBrokerMock.Verify(service =>
                service.MapCsvToObjectAsync<ResolvedAddress>(stringData, hasHeaderRecord, fieldMappings),
                    Times.Once());

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Exactly(outputResolvedAddresses.Count()));

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string stringAddress = resolvedAddress.PostalAddress ?? string.Empty;

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(stringAddress),
                        Times.Once);
            }

            this.auditBrokerMock.Verify(broker =>
                broker.LogInformation(
                    "Address",
                    "Successfully extracted address from Ordinance Database",
                    $"Successfully extracted address with id: {identifier} from file: {randomFilename}",
                    randomFilename,
                    identifier),
                        Times.Exactly(randomResolvedAddresses.Count));

            this.csvHelperBrokerMock.VerifyNoOtherCalls();
            this.auditBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}

