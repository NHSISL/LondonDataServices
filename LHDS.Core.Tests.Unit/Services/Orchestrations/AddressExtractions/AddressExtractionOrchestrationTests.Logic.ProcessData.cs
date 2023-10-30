// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Orchestrations.AddressExtractions
{
    public partial class AddressExctractioneOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldProcessAddressesDataAndLogAsync()
        {
            // Given
            string inputFilePath = @"./Resources/TestNestedZip.zip";
            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            List<string> unZippedInputFilePaths =
                new List<string> { @"./Resources/TestCsv1.csv", @"./Resources/TestCsv2.csv" };

            List<Address> extractedAddresses = new List<Address>();

            foreach (string filePath in unZippedInputFilePaths)
            {
                List<Address> currentCsvAddresses = new List<Address>();

                string randomCsvFormattedAddresses;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    randomCsvFormattedAddresses = reader.ReadToEnd();
                }

                string inputCsvFormattedAddresses = randomCsvFormattedAddresses;
                byte[] byteAddressesCsv = Encoding.GetEncoding("UTF-8").GetBytes(inputCsvFormattedAddresses);

                List<string> lines =
                    inputCsvFormattedAddresses.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

                foreach (string line in lines)
                {
                    string[] index = line.Split(",");

                    Address address = new Address
                    {
                        Id = Guid.NewGuid(),
                        UPRN = index[3],
                        UPSN = index[4],
                        OrganisationName = index[5],
                        DepartmentName = index[6],
                        SubBuildingName = index[7],
                        BuildingName = index[8],
                        BuildingNumber = index[9],
                        DependentThoroughfare = index[10],
                        Thoroughfare = index[11],
                        DoubleDependentLocality = index[12],
                        DependentLocality = index[13],
                        PostTown = index[14],
                        PostCode = index[15],
                    };

                    currentCsvAddresses.Add(address);
                    extractedAddresses.Add(address);
                }

                this.addressParserServiceMock.Setup(service =>
                    service.ProcessCsvAsync(byteAddressesCsv))
                        .ReturnsAsync(currentCsvAddresses);
            }

            List<Address> expectedAddresses = extractedAddresses.DeepClone();

            // When
            List<Address> actualAddresses =
                await this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
               options.Excluding(address => address.Id));

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()),
                    Times.Exactly(unZippedInputFilePaths.Count()));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Exactly(unZippedInputFilePaths.Count() * 2));

            this.addressExtractionAuditServiceMock.Verify(service =>
                service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Exactly(unZippedInputFilePaths.Count));

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

