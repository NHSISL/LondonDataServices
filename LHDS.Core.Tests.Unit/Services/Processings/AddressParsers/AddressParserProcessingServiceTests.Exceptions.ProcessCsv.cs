// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Processings.AddressParsers.Exceptions;
using LHDS.Core.Services.Processings.AddressParsers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Processings.AddressParsers
{
    public partial class AddressParserProcessingServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessCsvIfServiceErrorOccursAsync()
        {
            // given
            var mock = new Mock<AddressParserProcessingService>(
                addressParserServiceMock.Object,
                loggingBrokerMock.Object)
            { CallBase = true };

            string someAddress = GetRandomString();
            string someFilename = GetRandomString();
            var serviceException = new Exception();

            mock.Setup(x => x.ValidateAddressParserArgs(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(serviceException);

            AddressParserProcessingService addressParserProcessingService = mock.Object;

            var failedAddressParserProcessingServiceException =
                new FailedAddressParserProcessingServiceException(
                    message: "Failed address parser processing service error occurred, please contact support.",
                    innerException: serviceException);

            var expectedAddresParserProcessingServiceException =
                new AddressParserProcessingServiceException(
                    message: "Address parser processing service error occurred, please contact support.",
                    innerException: failedAddressParserProcessingServiceException);

            // when
            ValueTask<List<Address>> processCsvTask =
                addressParserProcessingService.ProcessCsvAsync(someAddress, someFilename);

            AddressParserProcessingServiceException actualAddressParserProcessingServiceException =
                await Assert.ThrowsAsync<AddressParserProcessingServiceException>(
                    processCsvTask.AsTask);

            // then
            actualAddressParserProcessingServiceException.Should()
                .BeEquivalentTo(expectedAddresParserProcessingServiceException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAddresParserProcessingServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
