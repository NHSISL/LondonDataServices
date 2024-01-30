// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressNormalisations;
using Moq;
using Renci.SshNet.Common;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressNormalisations
{
    public partial class AddressNormalisationOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessFileAndNormaliseAndLogAsync()
        {
            // Given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            string inputData = GetRandomString();

            ValueTask<List<Address>> randomAddresses = CreateRandomAddressesAsync();

            // Address Normalisation
            var randomAddress = GetRandomString();
            string inputAddress = randomAddress;

            AddressNormalisation addressNormalisation = new AddressNormalisation
            {
                PostalAddress = GetRandomString(),
                JsonPostalAddress = GetRandomString(),
                AddressComponents = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Street", GetRandomString()),
                        new KeyValuePair<string, string>("City", GetRandomString()),
                        new KeyValuePair<string, string>("PostCode", GetRandomString()),
                    }
            };

            var expectedNormalisedAddress = addressNormalisation.DeepClone();
            AddressLoadingAudit randomAddressLoadingAudit = CreateRandomAddressLoadingAudit(randomDateTimeOffset);
            AddressLoadingAudit inputAddressLoadingAudit = randomAddressLoadingAudit;
            AddressLoadingAudit storageAddressLoadingAudit = inputAddressLoadingAudit;
            AddressLoadingAudit expectedAddressLoadingAudit = storageAddressLoadingAudit.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTimeOffset())
                   .Returns(randomDateTimeOffset);

            this.addressParserServiceMock.Setup(processing =>
               processing.ProcessCsvAsync(inputData))
                    .ReturnsAsync(randomAddresses);

            this.addressNormalisationProcessingServiceMock.Setup(processing =>
               processing.GetNormalisedAddress(inputAddress))
                    .ReturnsAsync(addressNormalisation);

            this.addressLoadingAuditProcessingServiceMock.Setup(processing =>
              processing.AddAddressLoadingAuditAsync(inputAddressLoadingAudit))
                   .ReturnsAsync(storageAddressLoadingAudit);

            // When
            List<Address> actualAddresses = await this.addressNormalisationOrchestrationService
                .ProcessDataAsync(inputData);

            // Then

            this.addressParserServiceMock.VerifyNoOtherCalls();
        }
    }
}

