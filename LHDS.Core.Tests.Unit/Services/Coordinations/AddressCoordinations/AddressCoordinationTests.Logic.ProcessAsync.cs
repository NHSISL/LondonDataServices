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
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Coordinations.AddressCoordinations
{
    public partial class AddressCoordinationServiceTests
    {
        [Fact]
        public async Task ShouldProcessDataAndLogAsync()
        {
            // Given
            string inputFilePath = @"c:\temp\TestNestedZip.zip";
            byte[] inputData = await File.ReadAllBytesAsync(inputFilePath);

            List<string> unZippedInputFilePaths =
                new List<string> { @"c:\temp\TestCsv1.csv", @"c:\temp\TestCsv2.csv" };

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
            }

            this.addressExtractionOrchestrationServiceMock.Setup(service =>
                service.ProcessData(inputData))
                    .Returns(extractedAddresses);

            List<Address> persistedAddresses = new List<Address>();

            foreach(Address address in extractedAddresses)
            {
                address.PostalAddress = GetRandomString();
                address.JsonPostalAddress = GetRandomString();
                persistedAddresses.Add(address);
            }

            this.addressPersistanceOrchestrationServiceMock.Setup(service =>
                service.ProcessAsync(extractedAddresses))
                    .ReturnsAsync(persistedAddresses);

            List<Address> expectedAddresses = persistedAddresses.DeepClone();

            // When
            List<Address> actualAddresses = await this.addressCoordinationService.ProcessData(inputData);

            // Then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses);

            this.addressExtractionOrchestrationServiceMock.Verify(service =>
                service.ProcessData(inputData),
                    Times.Once());

            this.addressPersistanceOrchestrationServiceMock.Verify(service =>
                service.ProcessAsync(extractedAddresses),
                    Times.Once());

            this.addressExtractionOrchestrationServiceMock.VerifyNoOtherCalls();
            this.addressPersistanceOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}

