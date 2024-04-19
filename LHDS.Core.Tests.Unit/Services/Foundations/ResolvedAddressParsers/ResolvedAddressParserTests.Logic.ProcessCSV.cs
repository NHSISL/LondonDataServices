// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            int randomNumber = 1; //GetRandomNumber();
            Guid randomId = Guid.NewGuid();
            bool hasHeaderRecord = true;
            string filename = GetRandomString();

            List<dynamic> randomDynamicAddreses =
                GetRandomDynamicAddreses(identifier: randomId, count: randomNumber);

            string inputStringAddresses =
                CreateStringAddressFromDynamic(data: randomDynamicAddreses, hasHeaderRecord);

            List<string[]> oututAdresses = CreateStringArrayFromDynamic(data: randomDynamicAddreses);

            byte[] inputByteResolvedAddressesCsv =
            Encoding.GetEncoding("UTF-8").GetBytes(inputStringAddresses);

            List<ResolvedAddress> outputResolvedAddress =
                CreateResolvedAddressFromDynamic(data: randomDynamicAddreses);

            List<ResolvedAddress> expectedResolvedAddresses = outputResolvedAddress.DeepClone();

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapCsvToListArrayAsync(inputStringAddresses, hasHeaderRecord))
                    .ReturnsAsync(oututAdresses);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            // when
            List<ResolvedAddress> actualResolvedAddresses = await this.addressParserService
                .ProcessCsvAsync(data: inputByteResolvedAddressesCsv, filename);

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToListArrayAsync(inputStringAddresses, hasHeaderRecord),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Exactly(randomNumber));

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldProcessStringResolvedAddressCsvAsync()
        {
            // given
            int randomNumber = 1; // GetRandomNumber();
            Guid randomId = Guid.NewGuid();
            bool hasHeaderRecord = true;
            string filename = GetRandomString();

            List<dynamic> randomDynamicAddreses =
                GetRandomDynamicAddreses(identifier: randomId, count: randomNumber);

            string inputStringAddresses =
                CreateStringAddressFromDynamic(data: randomDynamicAddreses, hasHeaderRecord);

            List<string[]> oututAdresses = CreateStringArrayFromDynamic(data: randomDynamicAddreses);

            List<ResolvedAddress> outputResolvedAddress =
                CreateResolvedAddressFromDynamic(data: randomDynamicAddreses);

            List<ResolvedAddress> expectedResolvedAddresses = outputResolvedAddress.DeepClone();

            this.csvMapperBrokerMock.Setup(broker =>
                broker.MapCsvToListArrayAsync(inputStringAddresses, hasHeaderRecord))
                    .ReturnsAsync(oututAdresses);

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifier())
                    .Returns(randomId);

            // when
            List<ResolvedAddress> actualResolvedAddresses = await this.addressParserService
                .ProcessCsvAsync(data: inputStringAddresses, filename);

            // then
            actualResolvedAddresses.Should().BeEquivalentTo(expectedResolvedAddresses);

            this.csvMapperBrokerMock.Verify(broker =>
                broker.MapCsvToListArrayAsync(inputStringAddresses, hasHeaderRecord),
                    Times.Once);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifier(),
                    Times.Exactly(randomNumber));

            this.csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.identifierBrokerMock.VerifyNoOtherCalls();
        }
    }
}
