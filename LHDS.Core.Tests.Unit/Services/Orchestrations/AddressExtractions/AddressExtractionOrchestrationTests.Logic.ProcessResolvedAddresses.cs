// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractionOrchestrationServiceTests
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
            List<ResolvedAddress> randomResolvedAddresses = CreateRandomResolvedAddresses().ToList();
            List<ResolvedAddress> outputResolvedAddresses = randomResolvedAddresses.DeepClone();

            this.resolvedAddressParserServiceMock.Setup(service =>
                service.ProcessCsvAsync(inputData))
                    .ReturnsAsync(outputResolvedAddresses);

            List<ResolvedAddress> expectedResolvedAddresses = new List<ResolvedAddress>();

            foreach (ResolvedAddress resolvedAddress in outputResolvedAddresses)
            {
                string stringAddress = resolvedAddress.UnstructuredPostalAddress;

                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    PostalAddress = GetRandomString(),
                    JsonPostalAddress = GetRandomString()
                };

                this.addressNormalisationServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ReturnsAsync(addressNormalisation);

                resolvedAddress.PostalAddress = addressNormalisation.PostalAddress;
                resolvedAddress.JsonPostalAddress = addressNormalisation.JsonPostalAddress;

                expectedResolvedAddresses.Add(resolvedAddress);
            }

            // When
            List<ResolvedAddress> actualResolvedAddress = await this.addressExtractionOrchestrationService
                .ProcessResolvedAddressesAsync(inputData);

            // Then
            actualResolvedAddress.Should().BeEquivalentTo(expectedResolvedAddresses, options =>
                options.Excluding(address => address.Id));

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(inputData),
                    Times.Once());

            foreach (ResolvedAddress resolvedAddress in randomResolvedAddresses)
            {
                string stringAddress = resolvedAddress.UnstructuredPostalAddress;

                this.addressNormalisationServiceMock.Verify(service =>
                    service.GetNormalisedAddress(stringAddress),
                        Times.Once);
            }

            this.resolvedAddressParserServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationServiceMock.VerifyNoOtherCalls();
            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

