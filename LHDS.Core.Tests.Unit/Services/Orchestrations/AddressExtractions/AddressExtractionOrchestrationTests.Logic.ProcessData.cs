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
            string inputFilePath = @"c:\temp\AB.zip";
            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            List<string> unZippedInputFilePaths =
                new List<string> { @"c:\temp\Addresses\SP9500.csv", @"c:\temp\Addresses\SU9565.csv" };

            List<Address> extractedAddresses = new List<Address>();

            foreach (string filePath in unZippedInputFilePaths)
            {
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
                    if (line.StartsWith("28,"))
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

                        extractedAddresses.Add(address);
                    }
                }

                this.addressParserServiceMock.Setup(service =>
                    service.ProcessCsvAsync(byteAddressesCsv))
                        .ReturnsAsync(extractedAddresses);
            }

            List<Address> expectedAddresses = extractedAddresses.DeepClone();

            // Where
            List<Address> actualAddresses =
                await this.addressExtractionOrchestrationService.ProcessDataAsync(inputData);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
               options.Excluding(address => address.Id));

            this.addressParserServiceMock.Verify(service =>
                service.ProcessCsvAsync(It.IsAny<byte[]>()),
                    Times.Exactly(unZippedInputFilePaths.Count));

            this.addressExtractionAuditServiceMock.Verify(service =>
                service.AddAddressExtractionAuditAsync(It.IsAny<AddressExtractionAudit>()),
                    Times.Exactly(extractedAddresses.Count));

            this.addressParserServiceMock.VerifyNoOtherCalls();
            this.documentServiceMock.VerifyNoOtherCalls();
            this.addressExtractionAuditServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

