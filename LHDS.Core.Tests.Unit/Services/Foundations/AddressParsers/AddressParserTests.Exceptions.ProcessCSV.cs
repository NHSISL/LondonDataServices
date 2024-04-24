// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Brokers.CsvMappers;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using LHDS.Core.Services.Foundations.AddressParsers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressParsers
{
    public partial class AddressParserTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessByteIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFilename = GetRandomString();
            byte[] someData = Encoding.GetEncoding("UTF-8").GetBytes(GetRandomString());
            var serviceException = new Exception();
            Mock<ICsvMapperBroker> csvMapperBrokerMock = new Mock<ICsvMapperBroker>();

            AddressParserService addressParserService = new AddressParserService(
                csvMapperBroker: csvMapperBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);

            csvMapperBrokerMock.Setup(x => x.MapCsvToObjectAsync<Address>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, int>>(),
                It.IsAny<bool>()))
                    .Throws(serviceException);

            var failedAddressParserServiceException =
                new FailedAddressParserServiceException(
                    message: "Failed address parser service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressParserServiceException =
                new AddressParserServiceException(
                    message: "Address parser service error occurred, contact support.",
                    innerException: failedAddressParserServiceException);

            // when
            ValueTask<List<Address>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData, someFilename);

            AddressParserServiceException actualAddressParserServiceException =
                await Assert.ThrowsAsync<AddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualAddressParserServiceException.Should().BeEquivalentTo(expectedAddressParserServiceException);

            csvMapperBrokerMock.Verify(x => x.MapCsvToObjectAsync<Address>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, int>>(),
                It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressParserServiceException))),
                        Times.Once);

            csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessStringIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string someFilename = GetRandomString();
            string someData = GetRandomString();
            var serviceException = new Exception();
            Mock<ICsvMapperBroker> csvMapperBrokerMock = new Mock<ICsvMapperBroker>();

            AddressParserService addressParserService = new AddressParserService(
                csvMapperBroker: csvMapperBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);

            csvMapperBrokerMock.Setup(x => x.MapCsvToObjectAsync<Address>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, int>>(),
                It.IsAny<bool>()))
                    .Throws(serviceException);

            var failedAddressParserServiceException =
                new FailedAddressParserServiceException(
                    message: "Failed address parser service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressParserServiceException =
                new AddressParserServiceException(
                    message: "Address parser service error occurred, contact support.",
                    innerException: failedAddressParserServiceException);

            // when
            ValueTask<List<Address>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData, someFilename);

            AddressParserServiceException actualAddressParserServiceException =
                await Assert.ThrowsAsync<AddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualAddressParserServiceException.Should().BeEquivalentTo(expectedAddressParserServiceException);

            csvMapperBrokerMock.Verify(x => x.MapCsvToObjectAsync<Address>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, int>>(),
                It.IsAny<bool>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressParserServiceException))),
                        Times.Once);

            csvMapperBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
