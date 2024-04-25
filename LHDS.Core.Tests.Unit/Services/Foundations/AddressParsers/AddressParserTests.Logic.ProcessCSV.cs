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
                        UPRN = formatValue(index[3]),
                        UPSN = formatValue(index[4]),
                        OrganisationName = formatValue(index[5]),
                        DepartmentName = formatValue(index[6]),
                        SubBuildingName = formatValue(index[7]),
                        BuildingName = formatValue(index[8]),
                        BuildingNumber = formatValue(index[9]),
                        DependentThoroughfare = formatValue(index[10]),
                        Thoroughfare = formatValue(index[11]),
                        DoubleDependentLocality = formatValue(index[12]),
                        DependentLocality = formatValue(index[13]),
                        PostTown = formatValue(index[14]),
                        PostCode = formatValue(index[15]),
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

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        private static string formatValue(string value) =>
            value.Trim('"');

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
                        UPRN = formatValue(index[3]),
                        UPSN = formatValue(index[4]),
                        OrganisationName = formatValue(index[5]),
                        DepartmentName = formatValue(index[6]),
                        SubBuildingName = formatValue(index[7]),
                        BuildingName = formatValue(index[8]),
                        BuildingNumber = formatValue(index[9]),
                        DependentThoroughfare = formatValue(index[10]),
                        Thoroughfare = formatValue(index[11]),
                        DoubleDependentLocality = formatValue(index[12]),
                        DependentLocality = formatValue(index[13]),
                        PostTown = formatValue(index[14]),
                        PostCode = formatValue(index[15]),
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

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
