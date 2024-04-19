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
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressParsers
{
    public partial class AddressParserTests
    {
        [Fact]
        public async Task ShouldProcessByteAddressCsvAsync()
        {
            // given
            string inputFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Foundations/AddressParser/ShouldProcessAddressCsvAsync.csv");

            string randomCsvFormattedAddresses;

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                randomCsvFormattedAddresses = reader.ReadToEnd();
            }

            string inputCsvFormattedAddresses = randomCsvFormattedAddresses;
            byte[] inputByteAddressesCsv = Encoding.GetEncoding("UTF-8").GetBytes(inputCsvFormattedAddresses);

            List<string> lines =
                inputCsvFormattedAddresses.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<Address> expectedAddresses = new List<Address>();

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

                    expectedAddresses.Add(address);
                }
            }

            // when
            List<Address> actualAddresses = await this.addressParserService
                .ProcessCsvAsync(data: inputByteAddressesCsv, filename: inputFilename);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            var allAddressIds = actualAddresses.Select(addr => addr.Id).ToList();
            var uniqueAddressIds = allAddressIds.Distinct().ToList();
            Assert.Equal(allAddressIds.Count, uniqueAddressIds.Count);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessStringAddressCsvAsync()
        {
            // given
            string inputFilename = GetRandomString();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Foundations/AddressParser/ShouldProcessAddressCsvAsync.csv");

            string randomCsvFormattedAddresses;

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                randomCsvFormattedAddresses = reader.ReadToEnd();
            }

            string inputCsvFormattedAddresses = randomCsvFormattedAddresses;
            string inputStringAddressesCsv = inputCsvFormattedAddresses;

            List<string> lines =
                inputCsvFormattedAddresses.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<Address> expectedAddresses = new List<Address>();

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

                    expectedAddresses.Add(address);
                }
            }

            // when
            List<Address> actualAddresses = await this.addressParserService
                .ProcessCsvAsync(data: inputStringAddressesCsv, filename: inputFilename);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            var allAddressIds = actualAddresses.Select(addr => addr.Id).ToList();
            var uniqueAddressIds = allAddressIds.Distinct().ToList();
            Assert.Equal(allAddressIds.Count, uniqueAddressIds.Count);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
