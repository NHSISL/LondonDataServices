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
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserTests
    {
        [Fact]
        public async Task ShouldProcessByteResolvedAddressCsvAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            string assembly = Assembly.GetExecutingAssembly().Location;

            dynamic randomDynamicAddreses = GetRandomDynamicAddreses(identifier: randomId);

            string inputStringAddresses =
                CreateStringAddressFromDynamic(data: randomDynamicAddreses);


            List<ResolvedAddress> outputResolvedAddress =
                CreateResolvedAddressFromDynamic(data: randomDynamicAddreses);

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Foundations/ResolvedAddressParser/ShouldProcessResolvedAddressCsvAsync.csv");

            string randomCsvFormattedResolvedAddresses;

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                randomCsvFormattedResolvedAddresses = reader.ReadToEnd();
            }

            string inputCsvFormattedResolvedAddresses = randomCsvFormattedResolvedAddresses;

            byte[] inputByteResolvedAddressesCsv =
                Encoding.GetEncoding("UTF-8").GetBytes(inputCsvFormattedResolvedAddresses);

            List<string> lines =
                inputCsvFormattedResolvedAddresses.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<ResolvedAddress> expectedResolvedAddresses = new List<ResolvedAddress>();

            foreach (string line in lines)
            {
                string[] index = line.Split(",");

                ResolvedAddress address = new ResolvedAddress
                {
                    Id = randomId,
                    UniqueReference = Guid.Parse(index[0]),
                    PostCode = index[1],
                    UnstructuredPostalAddress = index[2],
                };

                expectedResolvedAddresses.Add(address);
            }

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            // when
            List<ResolvedAddress> actualResolvedAddresses = await this.addressParserService
                .ProcessCsvAsync(data: inputByteResolvedAddressesCsv);

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses, options =>
                options.Excluding(address => address.Id));

            var allResolvedAddressIds = actualResolvedAddresses.Select(addr => addr.Id).ToList();
            var uniqueResolvedAddressIds = allResolvedAddressIds.Distinct().ToList();
            Assert.Equal(allResolvedAddressIds.Count, uniqueResolvedAddressIds.Count);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessStringResolvedAddressCsvAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            string assembly = Assembly.GetExecutingAssembly().Location;

            string inputFilePath = Path.Combine(
                Path.GetDirectoryName(assembly),
                @"Resources/Services/Foundations/ResolvedAddressParser/ShouldProcessResolvedAddressCsvAsync.csv");

            string randomCsvFormattedResolvedAddresses;

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                randomCsvFormattedResolvedAddresses = reader.ReadToEnd();
            }

            string inputCsvFormattedResolvedAddresses = randomCsvFormattedResolvedAddresses;
            string inputStringResolvedAddressesCsv = inputCsvFormattedResolvedAddresses;

            List<string> lines =
                inputCsvFormattedResolvedAddresses.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            List<ResolvedAddress> expectedResolvedAddresses = new List<ResolvedAddress>();

            foreach (string line in lines)
            {
                string[] index = line.Split(",");

                ResolvedAddress address = new ResolvedAddress
                {
                    Id = randomId,
                    UniqueReference = Guid.Parse(index[0]),
                    PostCode = index[1],
                    UnstructuredPostalAddress = index[2],
                };

                expectedResolvedAddresses.Add(address);
            }

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            // when
            List<ResolvedAddress> actualResolvedAddresses = await this.addressParserService
                .ProcessCsvAsync(data: inputStringResolvedAddressesCsv);

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses, options =>
                options.Excluding(address => address.Id));

            var allResolvedAddressIds = actualResolvedAddresses.Select(addr => addr.Id).ToList();
            var uniqueResolvedAddressIds = allResolvedAddressIds.Distinct().ToList();
            Assert.Equal(allResolvedAddressIds.Count, uniqueResolvedAddressIds.Count);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
