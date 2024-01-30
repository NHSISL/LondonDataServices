// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.AddressParsers.Exceptions;
using LHDS.Core.Services.Foundations.AddressParsers;
using LHDS.Core.Services.Processings.AddressMatchers;
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
            var mock = new Mock<AddressParserService>(loggingBrokerMock.Object) { CallBase = true };
            byte[] someData = Encoding.GetEncoding("UTF-8").GetBytes(GetRandomString());
            var serviceException = new Exception();

            mock.Setup(x => x.ValidateAddressParserOnProcessCSV(It.IsAny<byte[]>()))
                .Throws(serviceException);

            AddressParserService addressParserService = mock.Object;

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
                addressParserService.ProcessCsvAsync(someData);

            AddressParserServiceException actualAddressParserServiceException =
                await Assert.ThrowsAsync<AddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualAddressParserServiceException.Should().BeEquivalentTo(expectedAddressParserServiceException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressParserServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessStringIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var mock = new Mock<AddressParserService>(loggingBrokerMock.Object) { CallBase = true };
            string someData = GetRandomString();
            var serviceException = new Exception();

            mock.Setup(x => x.ValidateAddressParserOnProcessCSV(It.IsAny<string>()))
                .Throws(serviceException);

            AddressParserService addressParserService = mock.Object;

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
                addressParserService.ProcessCsvAsync(someData);

            AddressParserServiceException actualAddressParserServiceException =
                await Assert.ThrowsAsync<AddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualAddressParserServiceException.Should().BeEquivalentTo(expectedAddressParserServiceException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressParserServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
