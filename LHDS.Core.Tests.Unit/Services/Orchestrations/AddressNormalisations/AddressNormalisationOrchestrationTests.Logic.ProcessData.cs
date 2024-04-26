// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationServiceTests
    {
        [Fact(Skip = "To be deleted. Not going to fix")]
        public async Task ShouldProcessFileAndNormaliseAndLogAsync()
        {
            // Given
            string inputData = GetRandomString();
            string someFilename = GetRandomString();
            List<Address> randomAddresses = CreateRandomAddresses().ToList();
            List<Address> inputAddresses = randomAddresses.DeepClone();

            List<Address> processedAddresses =
                new List<Address>();

            this.addressParserProcessingServiceMock.Setup(processing =>
              processing.ProcessCsvAsync(inputData, someFilename))
                   .ReturnsAsync(randomAddresses);

            foreach (Address address in inputAddresses)
            {
                AddressNormalisation addressNormalisation = new AddressNormalisation
                {
                    PostalAddress = GetRandomString(),
                    JsonPostalAddress = GetRandomString(),
                    AddressComponents = GenerateKeyValuePairList(GetRandomNumber())
                };

                string stringAddress = GenerateStringAddress(address);

                this.addressNormalisationProcessingServiceMock.Setup(service =>
                    service.GetNormalisedAddress(stringAddress))
                        .ReturnsAsync(addressNormalisation);

                processedAddresses.Add(address);
            }

            List<Address> expectedAddress = processedAddresses.DeepClone();

            // Where
            List<AddressNormalisation> actualAddresses =
                 await this.addressNormalisationOrchestrationService.ProcessDataAsync(inputData);

            // Then
            actualAddresses.Should().HaveCount(expectedAddress.Count);

            this.addressParserProcessingServiceMock.Verify(processing =>
                   processing.ProcessCsvAsync(It.IsAny<string>(), It.IsAny<string>()),
                       Times.Once);

            foreach (Address address in inputAddresses)
            {
                string stringAddress = GenerateStringAddress(address);

                this.addressNormalisationProcessingServiceMock.Verify(service =>
                    service.GetNormalisedAddress(stringAddress),
                        Times.Once());
            }

            this.addressParserProcessingServiceMock.VerifyNoOtherCalls();
            this.addressNormalisationProcessingServiceMock.VerifyNoOtherCalls();
        }
    }
}