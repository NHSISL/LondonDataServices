// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressParsers
{
    public partial class AddressParserTests
    {
        [Fact]
        public async Task ShouldProcessAddressCsvAsync()
        {
            // given
            string assembly = Assembly.GetExecutingAssembly().Location;

            string filePath = Path.Combine(Path.GetDirectoryName(assembly),
                @"Resources\ServicesFoundations/AddressParser/TestCsv3.csv");

            string randomCsvFormattedAddresses;

            using (StreamReader reader = new StreamReader(filePath))
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
            List<Address> actualAddresses = await this.addressParserService.ProcessCsvAsync(data: inputByteAddressesCsv);

            // then
            actualAddresses.Should().BeEquivalentTo(expectedAddresses, options =>
                options.Excluding(address => address.Id));

            var allAddressIds = actualAddresses.Select(addr => addr.Id).ToList();
            var uniqueAddressIds = allAddressIds.Distinct().ToList();
            Assert.Equal(allAddressIds.Count, uniqueAddressIds.Count);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformation(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
