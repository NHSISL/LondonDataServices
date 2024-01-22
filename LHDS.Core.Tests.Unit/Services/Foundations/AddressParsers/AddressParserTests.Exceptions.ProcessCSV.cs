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
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.AddressParsers
{
    public partial class AddressParserTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessCsvIfServiceErrorOccursAndLogItAsync()
        {
            // given
            byte[] someData = Encoding.GetEncoding("UTF-8").GetBytes(GetRandomString());
            var serviceException = new Exception();

            var failedAddressParserServiceException =
                new FailedAddressParserServiceException(
                    message: "Failed address parser service occurred, please contact support",
                    innerException: serviceException);

            var expectedAddressParserServiceException =
                new AddressParserServiceException(
                    message: "Address parser service error occurred, contact support.",
                    innerException: failedAddressParserServiceException);

            this.loggingBrokerMock.Setup(broker =>
                broker.LogInformation(It.IsAny<string>()))
                    .Throws(serviceException);

            // when
            ValueTask<List<Address>> processCSVTask =
                this.addressParserService.ProcessCsvAsync(someData);

            AddressParserServiceException actualAddressParserServiceException =
                await Assert.ThrowsAsync<AddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualAddressParserServiceException.Should().BeEquivalentTo(expectedAddressParserServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogInformation(It.IsAny<string>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAddressParserServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
