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
using LHDS.Core.Models.Foundations.Addresses;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressParsers
{
    public partial class AddressParserTests
    {
        [Fact]
        public async Task ShouldProcessAddressCSVAsync()
        {
            // given
            string filePath = @"c:\temp\Addresses\SP9500.csv";
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
                        Thoroughfare = index[12],
                        DoubleDependentLocality = index[13],
                        DependentLocality = index[14],
                        PostTown = index[15],
                        PostCode = index[16],
                    };

                    expectedAddresses.Add(address);
                }
            }
            
            // when
            List<Address> actualAddresses = this.addressParserService.ProcessCSV(data: inputByteAddressesCsv);

            // then
            foreach (Address actualAddress in actualAddresses)
            {
                Address expectedAddress = expectedAddresses.FirstOrDefault(address => address.Id == actualAddress.Id);
                expectedAddress.Should().NotBeNull();
            }

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
