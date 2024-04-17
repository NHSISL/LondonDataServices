// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions;
using LHDS.Core.Services.Foundations.ResolvedAddressParsers;
using Moq;
using Xunit;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ResolvedAddressParsers
{
    public partial class ResolvedAddressParserTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessByteIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var mock = new Mock<ResolvedAddressParserService>(loggingBrokerMock.Object) { CallBase = true };
            byte[] someData = Encoding.GetEncoding("UTF-8").GetBytes(GetRandomString());
            var serviceException = new Exception();

            mock.Setup(x => x.ValidateResolvedAddressParserOnProcessCSV(It.IsAny<byte[]>()))
                .Throws(serviceException);

            ResolvedAddressParserService addressParserService = mock.Object;

            var failedResolvedAddressParserServiceException =
                new FailedResolvedAddressParserServiceException(
                    message: "Failed address parser service occurred, please contact support",
                    innerException: serviceException);

            var expectedResolvedAddressParserServiceException =
                new ResolvedAddressParserServiceException(
                    message: "ResolvedAddress parser service error occurred, contact support.",
                    innerException: failedResolvedAddressParserServiceException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData);

            ResolvedAddressParserServiceException actualResolvedAddressParserServiceException =
                await Assert.ThrowsAsync<ResolvedAddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualResolvedAddressParserServiceException.Should().BeEquivalentTo(expectedResolvedAddressParserServiceException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressParserServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnProcessStringIfServiceErrorOccursAndLogItAsync()
        {
            // given
            var mock = new Mock<ResolvedAddressParserService>(loggingBrokerMock.Object) { CallBase = true };
            string someData = GetRandomString();
            var serviceException = new Exception();

            mock.Setup(x => x.ValidateResolvedAddressParserOnProcessCSV(It.IsAny<string>()))
                .Throws(serviceException);

            ResolvedAddressParserService addressParserService = mock.Object;

            var failedResolvedAddressParserServiceException =
                new FailedResolvedAddressParserServiceException(
                    message: "Failed address parser service occurred, please contact support",
                    innerException: serviceException);

            var expectedResolvedAddressParserServiceException =
                new ResolvedAddressParserServiceException(
                    message: "ResolvedAddress parser service error occurred, contact support.",
                    innerException: failedResolvedAddressParserServiceException);

            // when
            ValueTask<List<ResolvedAddress>> processCSVTask =
                addressParserService.ProcessCsvAsync(someData);

            ResolvedAddressParserServiceException actualResolvedAddressParserServiceException =
                await Assert.ThrowsAsync<ResolvedAddressParserServiceException>(async () =>
                    await processCSVTask);

            // then
            actualResolvedAddressParserServiceException.Should().BeEquivalentTo(expectedResolvedAddressParserServiceException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedResolvedAddressParserServiceException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
